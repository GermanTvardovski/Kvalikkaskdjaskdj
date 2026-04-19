using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Matie.Data;
using Matie.Services;

namespace Matie.ViewModels;

public partial class BookingViewModel : ObservableObject
{
    private readonly IServiceRepository serviceRepository;
    private readonly IAppointmentsService appointmentsService;
    private readonly IAuthService authService;

    [ObservableProperty]
    private ObservableCollection<Service> services = new();

    [ObservableProperty]
    private Service? selectedService;

    [ObservableProperty]
    private ObservableCollection<Master> mastersForService = new();

    [ObservableProperty]
    private Master? selectedMaster;

    [ObservableProperty]
    private DateTimeOffset? selectedDate = DateTimeOffset.Now.AddDays(1).Date.AddHours(10);

    [ObservableProperty]
    private string timeText = "10:00";

    [ObservableProperty]
    private string resultMessage = "";

    public BookingViewModel(
        IServiceRepository serviceRepository,
        IAppointmentsService appointmentsService,
        IAuthService authService)
    {
        this.serviceRepository = serviceRepository;
        this.appointmentsService = appointmentsService;
        this.authService = authService;
    }

    public async Task LoadAsync()
    {
        ResultMessage = "";
        var list = await serviceRepository.GetAllServicesAsync();
        Services = new ObservableCollection<Service>(list);
        SelectedService = Services.FirstOrDefault();
        await reloadMastersAsync();
    }

    partial void OnSelectedServiceChanged(Service? value)
    {
        _ = reloadMastersAsync();
    }

    private async Task reloadMastersAsync()
    {
        MastersForService.Clear();
        SelectedMaster = null;
        if (SelectedService == null)
        {
            return;
        }

        var ids = await serviceRepository.GetMasterIdsForServiceAsync(SelectedService.Id);
        var all = await serviceRepository.GetMastersAsync();
        var filtered = all.Where(m => ids.Contains(m.Id)).ToList();
        MastersForService = new ObservableCollection<Master>(filtered);
        SelectedMaster = MastersForService.FirstOrDefault();
    }

    [RelayCommand]
    private async Task BookAsync()
    {
        ResultMessage = "";
        var user = authService.CurrentUser;
        if (user == null)
        {
            ResultMessage = "Войдите в систему, чтобы записаться.";
            return;
        }

        if (SelectedService == null || SelectedMaster == null)
        {
            ResultMessage = "Выберите услугу и мастера.";
            return;
        }

        if (!tryParseTime(TimeText, out var timeOfDay))
        {
            ResultMessage = "Укажите время в формате ЧЧ:ММ (например 10:30).";
            return;
        }

        var date = (SelectedDate ?? DateTimeOffset.Now).DateTime.Date;
        var local = date.Add(timeOfDay);
        var utc = local.ToUniversalTime();
        var (ok, queue, err) = await appointmentsService.BookAsync(user.Id, SelectedService.Id, SelectedMaster.Id, utc);
        if (!ok)
        {
            ResultMessage = err ?? "Не удалось создать запись.";
            return;
        }

        ResultMessage = $"Вы записаны. Номер в очереди: {queue}. Дата: {local:g}";
    }

    private static bool tryParseTime(string text, out TimeSpan time)
    {
        time = default;
        var parts = text.Trim().Split(':', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2)
        {
            return false;
        }

        if (!int.TryParse(parts[0], out var hours) || !int.TryParse(parts[1], out var minutes))
        {
            return false;
        }

        if (hours is < 0 or > 23 || minutes is < 0 or > 59)
        {
            return false;
        }

        time = new TimeSpan(hours, minutes, 0);
        return true;
    }
}
