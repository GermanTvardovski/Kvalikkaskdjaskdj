using System.Security.Cryptography;
using System.Text;
using Matie.Data;
using Microsoft.EntityFrameworkCore;

namespace Matie.Services;

public sealed class AuthService : IAuthService
{
    private readonly IDbContextFactory<DBCon> dbContextFactory;
    private User? currentUser;

    public AuthService(IDbContextFactory<DBCon> dbContextFactory)
    {
        this.dbContextFactory = dbContextFactory;
    }

    public User? CurrentUser => currentUser;

    public async Task<User?> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim().ToLower();

        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var entity = await db.Users
            .Include(u => u.Role)
            .AsNoTracking()
            .FirstOrDefaultAsync(
                u => u.Email != null && u.Email.Trim().ToLower() == normalizedEmail,
                cancellationToken);

        if (entity == null || !passwordMatches(password, entity.Password))
        {
            return null;
        }

        currentUser = entity;
        return currentUser;
    }

    public async Task<(bool ok, string? error)> RegisterAsync(string name, string email, string password, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            return (false, "Заполните все поля.");
        }

        var normalizedEmail = email.Trim().ToLower();
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var exists = await db.Users.AnyAsync(
            u => u.Email != null && u.Email.Trim().ToLower() == normalizedEmail,
            cancellationToken);
        if (exists)
        {
            return (false, "Такой email уже зарегистрирован.");
        }

        var roleId = await db.Roles
            .Where(r => r.Name != null && r.Name.ToLower().Contains("пользовател"))
            .Select(r => r.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (roleId == 0)
        {
            roleId = await db.Roles.Select(r => r.Id).FirstOrDefaultAsync(cancellationToken);
        }

        if (roleId == 0)
        {
            return (false, "В базе нет ролей. Добавьте роль «Пользователь».");
        }

        var user = new User
        {
            Name = name.Trim(),
            Email = normalizedEmail,
            Password = hashPassword(password),
            RoleId = roleId,
            Balance = 0
        };

        db.Users.Add(user);
        await db.SaveChangesAsync(cancellationToken);

        await using var db2 = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var created = await db2.Users
            .Include(u => u.Role)
            .AsNoTracking()
            .FirstAsync(u => u.Id == user.Id, cancellationToken);

        currentUser = created;
        return (true, null);
    }

    public void Logout()
    {
        currentUser = null;
    }

    private static string hashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }

    private static bool passwordMatches(string plainPassword, string? stored)
    {
        if (string.IsNullOrEmpty(stored))
        {
            return false;
        }

        var expectedHash = hashPassword(plainPassword);
        if (string.Equals(stored, expectedHash, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return stored == plainPassword;
    }
}
