using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Matie.Services;

namespace Matie.ViewModels;

public partial class BalanceViewModel : ObservableObject
{
    private readonly IBalanceService balanceService;
    private readonly IAuthService authService;

    [ObservableProperty]
    private string balanceText = "0";

    [ObservableProperty]
    private string topUpAmountText = "1000";

    [ObservableProperty]
    private string statusMessage = "";

    public BalanceViewModel(IBalanceService balanceService, IAuthService authService)
    {
        this.balanceService = balanceService;
        this.authService = authService;
    }

    public async Task RefreshAsync()
    {
        var user = authService.CurrentUser;
        if (user == null)
        {
            return;
        }

        var balance = await balanceService.GetBalanceAsync(user.Id);
        BalanceText = balance.ToString("N2");
        StatusMessage = "";
    }

    [RelayCommand]
    private async Task TopUpAsync()
    {
        StatusMessage = "";
        var user = authService.CurrentUser;
        if (user == null)
        {
            StatusMessage = "Вход не выполнен.";
            return;
        }

        if (!decimal.TryParse(TopUpAmountText, out var amount) || amount <= 0)
        {
            StatusMessage = "Укажите корректную сумму пополнения.";
            return;
        }

        var (ok, err) = await balanceService.TopUpSimulatedAsync(user.Id, amount);
        if (!ok)
        {
            StatusMessage = err ?? "Ошибка пополнения.";
            return;
        }

        StatusMessage = "Баланс пополнён (имитация оплаты картой).";
        await RefreshAsync();
    }
}
