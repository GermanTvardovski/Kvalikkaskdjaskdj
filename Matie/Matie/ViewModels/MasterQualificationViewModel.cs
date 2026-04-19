using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Matie.Services;

namespace Matie.ViewModels;

public partial class MasterQualificationViewModel : ObservableObject
{
    private readonly IMasterProfileService masterProfileService;
    private readonly IAuthService authService;

    [ObservableProperty]
    private string message = "";

    public MasterQualificationViewModel(IMasterProfileService masterProfileService, IAuthService authService)
    {
        this.masterProfileService = masterProfileService;
        this.authService = authService;
    }

    public async Task LoadAsync()
    {
        Message = "Отправьте заявку на повышение квалификации. В профиле мастера будет зафиксирована пометка.";
        await Task.CompletedTask;
    }

    [RelayCommand]
    private async Task SubmitAsync()
    {
        Message = "";
        var user = authService.CurrentUser;
        var masterId = await masterProfileService.TryResolveMasterIdByUserNameAsync(user?.Name);
        if (masterId == null)
        {
            Message = "Профиль мастера не сопоставлен с пользователем.";
            return;
        }

        var (ok, text) = await masterProfileService.SubmitQualificationRequestAsync(masterId.Value);
        Message = ok ? text ?? "Готово." : text ?? "Ошибка.";
    }
}
