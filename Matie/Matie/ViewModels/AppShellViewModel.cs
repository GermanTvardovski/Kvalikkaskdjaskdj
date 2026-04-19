using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Matie.Data;
using Matie.Services;

namespace Matie.ViewModels;

public partial class AppShellViewModel : ObservableObject
{
    private readonly ILoginNavigation loginNavigation;
    private readonly IAuthService authService;
    private readonly ServicesViewModel servicesViewModel;

    [ObservableProperty]
    private object? sectionContent;

    [ObservableProperty]
    private string welcomeText = "";

    [ObservableProperty]
    private bool showBooking = true;

    [ObservableProperty]
    private bool showBalance = true;

    [ObservableProperty]
    private bool showReviews = true;

    [ObservableProperty]
    private bool showUsersAdmin;

    [ObservableProperty]
    private bool showMasterSchedule;

    [ObservableProperty]
    private bool showMasterQualification;

    [ObservableProperty]
    private bool showMasterLevels;

    public AppShellViewModel(
        ILoginNavigation loginNavigation,
        IAuthService authService,
        ServicesViewModel servicesViewModel,
        BalanceViewModel balanceViewModel,
        BookingViewModel bookingViewModel,
        ReviewsViewModel reviewsViewModel,
        AdminUsersViewModel adminUsersViewModel,
        MasterScheduleViewModel masterScheduleViewModel,
        MasterQualificationViewModel masterQualificationViewModel,
        MasterLevelViewModel masterLevelViewModel)
    {
        this.loginNavigation = loginNavigation;
        this.authService = authService;
        this.servicesViewModel = servicesViewModel;
        BalanceViewModel = balanceViewModel;
        BookingViewModel = bookingViewModel;
        ReviewsViewModel = reviewsViewModel;
        AdminUsersViewModel = adminUsersViewModel;
        MasterScheduleViewModel = masterScheduleViewModel;
        MasterQualificationViewModel = masterQualificationViewModel;
        MasterLevelViewModel = masterLevelViewModel;
    }

    public BalanceViewModel BalanceViewModel { get; }
    public BookingViewModel BookingViewModel { get; }
    public ReviewsViewModel ReviewsViewModel { get; }
    public AdminUsersViewModel AdminUsersViewModel { get; }
    public MasterScheduleViewModel MasterScheduleViewModel { get; }
    public MasterQualificationViewModel MasterQualificationViewModel { get; }
    public MasterLevelViewModel MasterLevelViewModel { get; }

    public void Initialize(User user)
    {
        WelcomeText = $"{user.Name} · {user.Role?.Name ?? "роль"}";
        var isAdmin = RoleHelper.IsAdministrator(user);
        var isModerator = RoleHelper.IsModerator(user);
        var isMaster = RoleHelper.IsMaster(user);

        ShowBooking = !isMaster;
        ShowBalance = true;
        ShowReviews = true;
        ShowUsersAdmin = isAdmin;
        ShowMasterSchedule = isMaster;
        ShowMasterQualification = isMaster;
        ShowMasterLevels = isModerator && !isMaster;

        servicesViewModel.SetModerationMode(isModerator && !isMaster);
        SectionContent = servicesViewModel;
    }

    [RelayCommand]
    private async Task OpenServicesAsync()
    {
        SectionContent = servicesViewModel;
        await servicesViewModel.RefreshAsync();
    }

    [RelayCommand]
    private async Task OpenBookingAsync()
    {
        SectionContent = BookingViewModel;
        await BookingViewModel.LoadAsync();
    }

    [RelayCommand]
    private async Task OpenBalanceAsync()
    {
        SectionContent = BalanceViewModel;
        await BalanceViewModel.RefreshAsync();
    }

    [RelayCommand]
    private async Task OpenReviewsAsync()
    {
        SectionContent = ReviewsViewModel;
        await ReviewsViewModel.LoadAsync();
    }

    [RelayCommand]
    private async Task OpenUsersAdminAsync()
    {
        SectionContent = AdminUsersViewModel;
        await AdminUsersViewModel.LoadAsync();
    }

    [RelayCommand]
    private async Task OpenMasterScheduleAsync()
    {
        SectionContent = MasterScheduleViewModel;
        await MasterScheduleViewModel.LoadAsync();
    }

    [RelayCommand]
    private async Task OpenMasterQualificationAsync()
    {
        SectionContent = MasterQualificationViewModel;
        await MasterQualificationViewModel.LoadAsync();
    }

    [RelayCommand]
    private async Task OpenMasterLevelsAsync()
    {
        SectionContent = MasterLevelViewModel;
        await MasterLevelViewModel.LoadAsync();
    }

    [RelayCommand]
    private void Logout()
    {
        loginNavigation.OnLogoutRequested();
    }
}
