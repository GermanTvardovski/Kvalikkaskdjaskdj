using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Matie.Data;
using Matie.Services;

namespace Matie.ViewModels;

public partial class AdminUsersViewModel : ObservableObject
{
    private readonly IUsersAdminService usersAdminService;

    [ObservableProperty]
    private ObservableCollection<User> users = new();

    [ObservableProperty]
    private ObservableCollection<Role> roles = new();

    [ObservableProperty]
    private string newEmployeeName = "";

    [ObservableProperty]
    private string newEmployeeEmail = "";

    [ObservableProperty]
    private string newEmployeePassword = "";

    [ObservableProperty]
    private Role? newEmployeeRole;

    [ObservableProperty]
    private string statusMessage = "";

    public AdminUsersViewModel(IUsersAdminService usersAdminService)
    {
        this.usersAdminService = usersAdminService;
    }

    public async Task LoadAsync()
    {
        StatusMessage = "";
        var roleList = await usersAdminService.GetRolesAsync();
        Roles = new ObservableCollection<Role>(roleList);

        var list = await usersAdminService.GetAllUsersAsync();
        foreach (var user in list)
        {
            if (user.RoleId is { } rid)
            {
                user.Role = Roles.FirstOrDefault(r => r.Id == rid);
            }
        }

        Users = new ObservableCollection<User>(list);
        NewEmployeeRole = Roles.FirstOrDefault();
    }

    [RelayCommand]
    private async Task SaveUserRoleAsync(User? user)
    {
        if (user == null)
        {
            return;
        }

        var roleId = user.Role?.Id ?? user.RoleId;
        if (roleId == null)
        {
            StatusMessage = "Выберите роль.";
            return;
        }

        var (ok, err) = await usersAdminService.UpdateUserRoleAsync(user.Id, roleId.Value);
        StatusMessage = ok ? "Роль сохранена." : err ?? "Ошибка.";
        if (ok)
        {
            await LoadAsync();
        }
    }

    [RelayCommand]
    private async Task CreateEmployeeAsync()
    {
        StatusMessage = "";
        if (NewEmployeeRole == null)
        {
            StatusMessage = "Выберите роль сотрудника.";
            return;
        }

        var (ok, err) = await usersAdminService.CreateEmployeeAsync(
            NewEmployeeName,
            NewEmployeeEmail,
            NewEmployeePassword,
            NewEmployeeRole.Id);

        if (!ok)
        {
            StatusMessage = err ?? "Ошибка.";
            return;
        }

        NewEmployeeName = "";
        NewEmployeeEmail = "";
        NewEmployeePassword = "";
        StatusMessage = "Сотрудник добавлен.";
        await LoadAsync();
    }
}
