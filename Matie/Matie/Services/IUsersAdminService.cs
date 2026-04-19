using Matie.Data;

namespace Matie.Services;

public interface IUsersAdminService
{
    Task<IReadOnlyList<User>> GetAllUsersAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Role>> GetRolesAsync(CancellationToken cancellationToken = default);
    Task<(bool ok, string? error)> UpdateUserRoleAsync(int userId, int roleId, CancellationToken cancellationToken = default);
    Task<(bool ok, string? error)> CreateEmployeeAsync(string name, string email, string password, int roleId, CancellationToken cancellationToken = default);
}
