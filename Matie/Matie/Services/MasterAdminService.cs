using Matie.Data;
using Microsoft.EntityFrameworkCore;

namespace Matie.Services;

public sealed class MasterAdminService : IMasterAdminService
{
    private readonly IDbContextFactory<DBCon> dbContextFactory;

    public MasterAdminService(IDbContextFactory<DBCon> dbContextFactory)
    {
        this.dbContextFactory = dbContextFactory;
    }

    public async Task<(bool ok, string? error)> UpdateQualificationLevelAsync(int masterId, string? newLevel, CancellationToken cancellationToken = default)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var master = await db.Masters.FirstOrDefaultAsync(m => m.Id == masterId, cancellationToken);
        if (master == null)
        {
            return (false, "Мастер не найден.");
        }

        master.Level = string.IsNullOrWhiteSpace(newLevel) ? null : newLevel.Trim();
        await db.SaveChangesAsync(cancellationToken);
        return (true, null);
    }
}
