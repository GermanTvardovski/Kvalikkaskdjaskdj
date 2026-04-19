using Matie.Data;
using Microsoft.EntityFrameworkCore;

namespace Matie.Services;

public sealed class MasterProfileService : IMasterProfileService
{
    private readonly IDbContextFactory<DBCon> dbContextFactory;

    public MasterProfileService(IDbContextFactory<DBCon> dbContextFactory)
    {
        this.dbContextFactory = dbContextFactory;
    }

    public async Task<int?> TryResolveMasterIdByUserNameAsync(string? userName, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userName))
        {
            return null;
        }

        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var master = await db.Masters
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Name != null && m.Name.Trim() == userName.Trim(), cancellationToken);

        return master?.Id;
    }

    public async Task<(bool ok, string? message)> SubmitQualificationRequestAsync(int masterId, CancellationToken cancellationToken = default)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var master = await db.Masters.FirstOrDefaultAsync(m => m.Id == masterId, cancellationToken);
        if (master == null)
        {
            return (false, "Профиль мастера не найден.");
        }

        var level = master.Level ?? "";
        master.Level = string.IsNullOrWhiteSpace(level) ? "Повышение запрошено" : $"{level} (заявка на повышение)";
        await db.SaveChangesAsync(cancellationToken);
        return (true, "Заявка зафиксирована: уровень в профиле обновлён пометкой.");
    }
}
