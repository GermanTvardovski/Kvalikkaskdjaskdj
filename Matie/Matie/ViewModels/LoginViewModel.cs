using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Matie.Services;

namespace Matie.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly IAuthService authService;
    private ILoginNavigation? navigation;

    public LoginViewModel(IAuthService authService)
    {
        this.authService = authService;
    }

    public void AttachNavigation(ILoginNavigation loginNavigation)
    {
        navigation = loginNavigation;
    }

    [ObservableProperty]
    private bool isRegisterMode;

    [ObservableProperty]
    private string displayName = "";

    [ObservableProperty]
    private string email = "";

    [ObservableProperty]
    private string password = "";

    [ObservableProperty]
    private string errorMessage = "";

    [RelayCommand]
    private void ToggleRegisterMode()
    {
        IsRegisterMode = !IsRegisterMode;
        ErrorMessage = "";
    }

    [RelayCommand]
    private async Task SubmitAsync()
    {
        ErrorMessage = "";
        if (IsRegisterMode)
        {
            var (ok, err) = await authService.RegisterAsync(DisplayName, Email, Password);
            if (!ok)
            {
                ErrorMessage = err ?? "Ошибка регистрации.";
                return;
            }

            var user = authService.CurrentUser;
            if (user != null)
            {
                navigation?.OnLoggedIn(user);
            }
        }
        else
        {
            var user = await authService.LoginAsync(Email, Password);
            if (user == null)
            {
                ErrorMessage = "Неверный email или пароль.";
                return;
            }

            navigation?.OnLoggedIn(user);
        }
    }
}
