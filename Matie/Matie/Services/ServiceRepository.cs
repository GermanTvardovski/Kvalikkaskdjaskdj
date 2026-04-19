using Matie.Data;
using Matie.Models;
using Microsoft.EntityFrameworkCore;

namespace Matie.Services;

public sealed class ServiceRepository : IServiceRepository
{
    private readonly IDbContextFactory<DBCon> dbContextFactory;

    public ServiceRepository(IDbContextFactory<DBCon> dbContextFactory)
    {
        this.dbContextFactory = dbContextFactory;
    }

    public async Task<IReadOnlyList<Category>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await db.Categories
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Master>> GetMastersAsync(CancellationToken cancellationToken = default)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await db.Masters
            .AsNoTracking()
            .OrderBy(m => m.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IReadOnlyList<ServiceCardModel> Items, int TotalCount)> GetServicesPageAsync(
        string? searchText,
        int? categoryId,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var query = db.Services
            .AsNoTracking()
            .Include(s => s.Category)
            .Include(s => s.Servicemasters)
            .ThenInclude(sm => sm!.Master)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            var term = searchText.Trim();
            query = query.Where(s =>
                (s.Name != null && EF.Functions.ILike(s.Name, $"%{term}%")) ||
                (s.Description != null && EF.Functions.ILike(s.Description, $"%{term}%")));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(s => s.CategoryId == categoryId.Value);
        }

        var ordered = query.OrderBy(s => s.Name);
        var totalCount = await ordered.CountAsync(cancellationToken);
        var page = await ordered
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var items = page.Select(mapToCard).ToList();
        return (items, totalCount);
    }

    public async Task<Service?> GetServiceByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await db.Services
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<int>> GetMasterIdsForServiceAsync(int serviceId, CancellationToken cancellationToken = default)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await db.Servicemasters
            .AsNoTracking()
            .Where(sm => sm.ServiceId == serviceId && sm.MasterId != null)
            .Select(sm => sm.MasterId!.Value)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> AddServiceAsync(Service service, IReadOnlyList<int> masterIds, CancellationToken cancellationToken = default)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var utcNow = PostgresDateTime.ForNaiveTimestamp(DateTime.UtcNow);
        service.UpdatedAt = utcNow;
        db.Services.Add(service);
        await db.SaveChangesAsync(cancellationToken);

        foreach (var masterId in masterIds.Distinct())
        {
            db.Servicemasters.Add(new Servicemaster
            {
                ServiceId = service.Id,
                MasterId = masterId
            });
        }

        await db.SaveChangesAsync(cancellationToken);
        return service.Id;
    }

    public async Task UpdateServiceAsync(Service service, IReadOnlyList<int> masterIds, CancellationToken cancellationToken = default)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var existing = await db.Services.FirstOrDefaultAsync(s => s.Id == service.Id, cancellationToken);
        if (existing == null)
        {
            throw new InvalidOperationException("Услуга не найдена.");
        }

        var utcNow = PostgresDateTime.ForNaiveTimestamp(DateTime.UtcNow);
        existing.Name = service.Name;
        existing.Description = service.Description;
        existing.Price = service.Price;
        existing.CategoryId = service.CategoryId;
        existing.ImageData = service.ImageData;
        existing.UpdatedAt = utcNow;

        var links = await db.Servicemasters.Where(sm => sm.ServiceId == service.Id).ToListAsync(cancellationToken);
        db.Servicemasters.RemoveRange(links);

        foreach (var masterId in masterIds.Distinct())
        {
            db.Servicemasters.Add(new Servicemaster
            {
                ServiceId = service.Id,
                MasterId = masterId
            });
        }

        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Service>> GetAllServicesAsync(CancellationToken cancellationToken = default)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await db.Services
            .AsNoTracking()
            .Include(s => s.Category)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    private static ServiceCardModel mapToCard(Service service)
    {
        var masters = service.Servicemasters
            .Select(sm => sm.Master?.Name)
            .Where(n => !string.IsNullOrWhiteSpace(n))
            .Cast<string>()
            .Distinct()
            .OrderBy(n => n)
            .ToArray();

        var mastersSummary = masters.Length == 0
            ? "—"
            : string.Join(", ", masters);

        return new ServiceCardModel
        {
            Id = service.Id,
            Name = service.Name ?? "",
            Description = string.IsNullOrWhiteSpace(service.Description) ? "—" : service.Description!,
            MastersSummary = mastersSummary,
            CategoryName = service.Category?.Name ?? "—",
            ImageData = service.ImageData
        };
    }
}
