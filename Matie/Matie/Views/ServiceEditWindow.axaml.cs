using System;
using Avalonia.Controls;
using Matie.ViewModels;

namespace Matie.Views;

public partial class ServiceEditWindow : Window
{
    public ServiceEditWindow()
    {
        InitializeComponent();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        if (DataContext is ServiceEditViewModel viewModel)
        {
            viewModel.CloseRequested -= OnCloseRequested;
            viewModel.CloseRequested += OnCloseRequested;
        }
    }

    private void OnCloseRequested(object? sender, bool result)
    {
        Close(result);
    }
}
