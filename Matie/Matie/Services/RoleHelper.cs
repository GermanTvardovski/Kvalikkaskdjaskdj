using Matie.Data;

namespace Matie.Services;

public static class RoleHelper
{
    public static bool IsGuest(User? user)
    {
        return user == null;
    }

    public static bool IsAdministrator(User? user)
    {
        return containsRole(user, "администратор", "administrator", "admin");
    }

    public static bool IsModerator(User? user)
    {
        return IsAdministrator(user) || containsRole(user, "модератор", "moderator");
    }

    public static bool IsMaster(User? user)
    {
        return containsRole(user, "мастер", "master");
    }

    public static bool IsCustomer(User? user)
    {
        return user != null && !IsModerator(user) && !IsMaster(user);
    }

    private static bool containsRole(User? user, params string[] keys)
    {
        var name = user?.Role?.Name;
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        var lower = name.Trim().ToLowerInvariant();
        foreach (var key in keys)
        {
            if (lower.Contains(key, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }
}
