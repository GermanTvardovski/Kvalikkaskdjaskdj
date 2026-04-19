using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Matie.Data;
using Matie.Models;
using Matie.Services;

namespace Matie.ViewModels;

public partial class MasterLevelViewModel : ObservableObject
{
    private readonly IMasterAdminService masterAdminService;
    private readonly IServiceRepository serviceRepository;

    [ObservableProperty]
    private ObservableCollection<MasterLevelRowModel> rows = new();

    [ObservableProperty]
    private string statusMessage = "";

    public MasterLevelViewModel(IMasterAdminService masterAdminService, IServiceRepository serviceRepository)
    {
        this.masterAdminService = masterAdminService;
        this.serviceRepository = serviceRepository;
    }

    public async Task LoadAsync()
    {
        StatusMessage = "";
        var masters = await serviceRepository.GetMastersAsync();
        var list = masters.Select(m => new MasterLevelRowModel
        {
            Id = m.Id,
            Name = m.Name ?? "",
            Level = m.Level ?? ""
        }).ToList();

        Rows = new ObservableCollection<MasterLevelRowModel>(list);
    }

    [RelayCommand]
    private async Task SaveRowAsync(MasterLevelRowModel? row)
    {
        if (row == null)
        {
            return;
        }

        var (ok, err) = await masterAdminService.UpdateQualificationLevelAsync(row.Id, row.Level);
        StatusMessage = ok ? "Квалификация обновлена." : err ?? "Ошибка.";
    }
}
