using CommunityToolkit.Mvvm.ComponentModel;

namespace Matie.Models;

public partial class MasterPickModel : ObservableObject
{
    [ObservableProperty]
    private int id;

    [ObservableProperty]
    private string name = "";

    [ObservableProperty]
    private bool isSelected;
}
