using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Matie.Services;

namespace Matie.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly LoginViewModel loginViewModel;
    private readonly AppShellViewModel appShellViewModel;
    private readonly IAuthService authService;
    private readonly IUiDialogService uiDialogService;

    public MainWindowViewModel(
        LoginNavigationService navigation,
        LoginViewModel loginViewModel,
        AppShellViewModel appShellViewModel,
        IAuthService authService,
        IUiDialogService uiDialogService)
    {
        this.loginViewModel = loginViewModel;
        this.appShellViewModel = appShellViewModel;
        this.authService = authService;
        this.uiDialogService = uiDialogService;

        navigation.SetHandlers(
            user =>
            {
                MainContent = appShellViewModel;
                appShellViewModel.Initialize(user);
            },
            () =>
            {
                authService.Logout();
                MainContent = loginViewModel;
            });

        loginViewModel.AttachNavigation(navigation);
        MainContent = loginViewModel;
    }

    [ObservableProperty]
    private object? mainContent;

    [RelayCommand]
    private static void Minimize()
    {
        if (Avalonia.Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
            && desktop.MainWindow != null)
        {
            desktop.MainWindow.WindowState = Avalonia.Controls.WindowState.Minimized;
        }
    }

    [RelayCommand]
    private async Task ExitApplicationAsync()
    {
        if (await uiDialogService.ConfirmCloseAsync())
        {
            if (Avalonia.Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.Shutdown();
            }
        }
    }
}
