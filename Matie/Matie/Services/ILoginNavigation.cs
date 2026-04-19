using Matie.Data;

namespace Matie.Services;

public interface ILoginNavigation
{
    void OnLoggedIn(User user);
    void OnLogoutRequested();
}
