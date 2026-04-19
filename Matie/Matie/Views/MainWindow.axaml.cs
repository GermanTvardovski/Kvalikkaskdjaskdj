using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Platform;
using Matie.ViewModels;

namespace Matie.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        try
        {
            var uri = new Uri("avares://Matie/Assets/app.ico");
            if (AssetLoader.Exists(uri))
            {
                Icon = new WindowIcon(AssetLoader.Open(uri));
            }
        }
        catch
        {
            // ignore missing or invalid icon
        }
    }

    public MainWindow(MainWindowViewModel viewModel)
        : this()
    {
        DataContext = viewModel;
    }

    private void TitleBar_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            BeginMoveDrag(e);
        }
    }
}
