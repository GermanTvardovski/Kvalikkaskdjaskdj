using System;
using Avalonia;
using Matie.Data;
using Matie.Services;
using Matie.ViewModels;
using Matie.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Matie;

internal static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        App.Services = configureServices();
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    public static IServiceProvider configureServices()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", false)
            .Build();

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddDbContextFactory<DBCon>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Matie")!));

        services.AddSingleton<IAuthService, AuthService>();
        services.AddSingleton<IServiceRepository, ServiceRepository>();
        services.AddSingleton<IUiDialogService, UiDialogService>();
        services.AddSingleton<IImagePickerService, ImagePickerService>();
        services.AddSingleton<IAppointmentsService, AppointmentsService>();
        services.AddSingleton<IReviewsService, ReviewsService>();
        services.AddSingleton<IUsersAdminService, UsersAdminService>();
        services.AddSingleton<IBalanceService, BalanceService>();
        services.AddSingleton<IMasterProfileService, MasterProfileService>();
        services.AddSingleton<IMasterAdminService, MasterAdminService>();
        services.AddSingleton<LoginNavigationService>();
        services.AddSingleton<ILoginNavigation>(sp => sp.GetRequiredService<LoginNavigationService>());

        services.AddSingleton<ServicesViewModel>();
        services.AddSingleton<LoginViewModel>();
        services.AddSingleton<BalanceViewModel>();
        services.AddSingleton<BookingViewModel>();
        services.AddSingleton<ReviewsViewModel>();
        services.AddSingleton<AdminUsersViewModel>();
        services.AddSingleton<MasterScheduleViewModel>();
        services.AddSingleton<MasterQualificationViewModel>();
        services.AddSingleton<MasterLevelViewModel>();
        services.AddSingleton<AppShellViewModel>();
        services.AddSingleton<MainWindowViewModel>();
        services.AddTransient<ServiceEditViewModel>();
        services.AddSingleton<MainWindow>();

        return services.BuildServiceProvider();
    }

    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
    }
}
