using Matie.Data;
using Microsoft.EntityFrameworkCore;

namespace Matie.Services;

public sealed class BalanceService : IBalanceService
{
    private readonly IDbContextFactory<DBCon> dbContextFactory;

    public BalanceService(IDbContextFactory<DBCon> dbContextFactory)
    {
        this.dbContextFactory = dbContextFactory;
    }

    public async Task<decimal> GetBalanceAsync(int userId, CancellationToken cancellationToken = default)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var balance = await db.Users
            .AsNoTracking()
            .Where(u => u.Id == userId)
            .Select(u => u.Balance)
            .FirstAsync(cancellationToken);

        return balance ?? 0;
    }

    public async Task<(bool ok, string? error)> TopUpSimulatedAsync(int userId, decimal amount, CancellationToken cancellationToken = default)
    {
        if (amount <= 0)
        {
            return (false, "Сумма должна быть больше нуля.");
        }

        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var user = await db.Users.FirstAsync(u => u.Id == userId, cancellationToken);
        user.Balance = (user.Balance ?? 0) + amount;

        db.Balancetransactions.Add(new Balancetransaction
        {
            UserId = userId,
            Amount = amount,
            Date = PostgresDateTime.ForNaiveTimestamp(DateTime.UtcNow)
        });

        await db.SaveChangesAsync(cancellationToken);
        return (true, null);
    }
}
