using CommunityToolkit.Mvvm.ComponentModel;

namespace Matie.Models;

public partial class MasterLevelRowModel : ObservableObject
{
    public int Id { get; init; }

    public string Name { get; init; } = "";

    [ObservableProperty]
    private string level = "";
}
