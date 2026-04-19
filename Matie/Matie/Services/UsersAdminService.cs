using System.Security.Cryptography;
using System.Text;
using Matie.Data;
using Microsoft.EntityFrameworkCore;

namespace Matie.Services;

public sealed class UsersAdminService : IUsersAdminService
{
    private readonly IDbContextFactory<DBCon> dbContextFactory;

    public UsersAdminService(IDbContextFactory<DBCon> dbContextFactory)
    {
        this.dbContextFactory = dbContextFactory;
    }

    public async Task<IReadOnlyList<User>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await db.Users
            .AsNoTracking()
            .Include(u => u.Role)
            .OrderBy(u => u.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Role>> GetRolesAsync(CancellationToken cancellationToken = default)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await db.Roles.AsNoTracking().OrderBy(r => r.Name).ToListAsync(cancellationToken);
    }

    public async Task<(bool ok, string? error)> UpdateUserRoleAsync(int userId, int roleId, CancellationToken cancellationToken = default)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        if (user == null)
        {
            return (false, "Пользователь не найден.");
        }

        user.RoleId = roleId;
        await db.SaveChangesAsync(cancellationToken);
        return (true, null);
    }

    public async Task<(bool ok, string? error)> CreateEmployeeAsync(
        string name,
        string email,
        string password,
        int roleId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            return (false, "Заполните поля.");
        }

        var normalizedEmail = email.Trim().ToLowerInvariant();
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var exists = await db.Users.AnyAsync(u => u.Email != null && u.Email.ToLower() == normalizedEmail, cancellationToken);
        if (exists)
        {
            return (false, "Email уже занят.");
        }

        var hash = hashPassword(password);
        db.Users.Add(new User
        {
            Name = name.Trim(),
            Email = normalizedEmail,
            Password = hash,
            RoleId = roleId,
            Balance = 0
        });

        await db.SaveChangesAsync(cancellationToken);
        return (true, null);
    }

    private static string hashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }
}
