using Matie.Data;

namespace Matie.Services;

public sealed class LoginNavigationService : ILoginNavigation
{
    private Action<User>? onLoggedIn;
    private Action? onLogoutRequested;

    public void SetHandlers(Action<User> loggedIn, Action logoutRequested)
    {
        onLoggedIn = loggedIn;
        onLogoutRequested = logoutRequested;
    }

    public void OnLoggedIn(User user)
    {
        onLoggedIn?.Invoke(user);
    }

    public void OnLogoutRequested()
    {
        onLogoutRequested?.Invoke();
    }
}
