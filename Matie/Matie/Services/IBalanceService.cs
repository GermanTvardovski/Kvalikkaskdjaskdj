namespace Matie.Services;

public interface IBalanceService
{
    Task<decimal> GetBalanceAsync(int userId, CancellationToken cancellationToken = default);
    Task<(bool ok, string? error)> TopUpSimulatedAsync(int userId, decimal amount, CancellationToken cancellationToken = default);
}
