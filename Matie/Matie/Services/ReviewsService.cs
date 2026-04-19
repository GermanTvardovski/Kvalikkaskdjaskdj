using Matie.Data;
using Matie.Models;
using Microsoft.EntityFrameworkCore;

namespace Matie.Services;

public sealed class ReviewsService : IReviewsService
{
    private readonly IDbContextFactory<DBCon> dbContextFactory;

    public ReviewsService(IDbContextFactory<DBCon> dbContextFactory)
    {
        this.dbContextFactory = dbContextFactory;
    }

    public async Task<IReadOnlyList<ReviewRowModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var rows = await db.Reviews
            .AsNoTracking()
            .Include(r => r.User)
            .Include(r => r.Service)
            .OrderByDescending(r => r.Id)
            .Take(200)
            .ToListAsync(cancellationToken);

        return rows.Select(r => new ReviewRowModel
        {
            Id = r.Id,
            Author = r.User?.Name ?? "—",
            ServiceName = r.Service?.Name ?? "—",
            Rating = r.Rating ?? 0,
            Comment = r.Comment ?? ""
        }).ToList();
    }

    public async Task<(bool ok, string? error)> AddAsync(
        int userId,
        int serviceId,
        int rating,
        string? comment,
        CancellationToken cancellationToken = default)
    {
        if (rating < 1 || rating > 5)
        {
            return (false, "Оценка от 1 до 5.");
        }

        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        db.Reviews.Add(new Review
        {
            UserId = userId,
            ServiceId = serviceId,
            Rating = rating,
            Comment = string.IsNullOrWhiteSpace(comment) ? null : comment.Trim()
        });

        await db.SaveChangesAsync(cancellationToken);
        return (true, null);
    }
}
