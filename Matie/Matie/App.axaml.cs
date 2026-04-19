using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Matie.ViewModels;
using Matie.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Matie;

public partial class App : Application
{
    public static IServiceProvider Services { get; set; } = null!;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = Services.GetRequiredService<MainWindow>();
            var servicesViewModel = Services.GetRequiredService<ServicesViewModel>();
            _ = loadCategoriesSafeAsync(servicesViewModel);
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static async Task loadCategoriesSafeAsync(ServicesViewModel servicesViewModel)
    {
        try
        {
            await servicesViewModel.LoadCategoriesAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Trace.WriteLine($"Категории не загружены: {ex.Message}");
        }
    }
}
