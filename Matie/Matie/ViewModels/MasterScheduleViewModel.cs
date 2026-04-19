using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Matie.Data;
using Matie.Services;

namespace Matie.ViewModels;

public partial class MasterScheduleViewModel : ObservableObject
{
    private readonly IAppointmentsService appointmentsService;
    private readonly IMasterProfileService masterProfileService;
    private readonly IAuthService authService;

    [ObservableProperty]
    private ObservableCollection<Appointment> appointments = new();

    [ObservableProperty]
    private string infoMessage = "";

    public MasterScheduleViewModel(
        IAppointmentsService appointmentsService,
        IMasterProfileService masterProfileService,
        IAuthService authService)
    {
        this.appointmentsService = appointmentsService;
        this.masterProfileService = masterProfileService;
        this.authService = authService;
    }

    public async Task LoadAsync()
    {
        InfoMessage = "";
        var user = authService.CurrentUser;
        var masterId = await masterProfileService.TryResolveMasterIdByUserNameAsync(user?.Name);
        if (masterId == null)
        {
            InfoMessage = "Для учётной записи не найден мастер с тем же именем. Добавьте мастера в БД или совпадайте имя.";
            Appointments = new ObservableCollection<Appointment>();
            return;
        }

        var list = await appointmentsService.GetForMasterAsync(masterId.Value);
        Appointments = new ObservableCollection<Appointment>(list);
        InfoMessage = list.Count == 0 ? "Записей пока нет." : "";
    }
}
