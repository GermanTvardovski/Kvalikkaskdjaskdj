using Matie.Data;

namespace Matie.Services;

public interface IAuthService
{
    User? CurrentUser { get; }
    Task<User?> LoginAsync(string email, string password, CancellationToken cancellationToken = default);
    Task<(bool ok, string? error)> RegisterAsync(string name, string email, string password, CancellationToken cancellationToken = default);
    void Logout();
}
