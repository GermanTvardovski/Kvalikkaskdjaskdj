using Avalonia.Controls;
using Matie.ViewModels;
using Matie.Views;
using Microsoft.Extensions.DependencyInjection;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace Matie.Services;

public sealed class UiDialogService : IUiDialogService
{
    private readonly IServiceProvider serviceProvider;

    public UiDialogService(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public async Task<bool> ConfirmCloseAsync()
    {
        var owner = ResolveOwner();
        var box = MessageBoxManager.GetMessageBoxStandard(
            "Выход",
            "Закрыть приложение?",
            ButtonEnum.YesNo,
            Icon.Question,
            WindowStartupLocation.CenterOwner);

        var result = await box.ShowWindowDialogAsync(owner);
        return result == ButtonResult.Yes;
    }

    public async Task<bool> ShowServiceEditorAsync(int? serviceId)
    {
        var owner = ResolveOwner();
        var viewModel = serviceProvider.GetRequiredService<ServiceEditViewModel>();
        await viewModel.InitializeAsync(serviceId);
        var window = new ServiceEditWindow
        {
            DataContext = viewModel
        };
        var dialogResult = await window.ShowDialog<bool>(owner);
        return dialogResult;
    }

    private static Window ResolveOwner()
    {
        if (Avalonia.Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop
            && desktop.MainWindow != null)
        {
            return desktop.MainWindow;
        }

        throw new InvalidOperationException("Главное окно недоступно.");
    }
}
