using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Matie.Data;
using Matie.Models;
using Matie.Services;

namespace Matie.ViewModels;

public partial class ReviewsViewModel : ObservableObject
{
    private readonly IReviewsService reviewsService;
    private readonly IServiceRepository serviceRepository;
    private readonly IAuthService authService;

    [ObservableProperty]
    private ObservableCollection<ReviewRowModel> items = new();

    [ObservableProperty]
    private ObservableCollection<Service> services = new();

    [ObservableProperty]
    private Service? selectedService;

    [ObservableProperty]
    private int rating = 5;

    [ObservableProperty]
    private string comment = "";

    [ObservableProperty]
    private string statusMessage = "";

    public ReviewsViewModel(
        IReviewsService reviewsService,
        IServiceRepository serviceRepository,
        IAuthService authService)
    {
        this.reviewsService = reviewsService;
        this.serviceRepository = serviceRepository;
        this.authService = authService;
    }

    public async Task LoadAsync()
    {
        StatusMessage = "";
        var rows = await reviewsService.GetAllAsync();
        Items = new ObservableCollection<ReviewRowModel>(rows);
        var list = await serviceRepository.GetAllServicesAsync();
        Services = new ObservableCollection<Service>(list);
        SelectedService = Services.FirstOrDefault();
    }

    [RelayCommand]
    private async Task AddReviewAsync()
    {
        StatusMessage = "";
        var user = authService.CurrentUser;
        if (user == null)
        {
            StatusMessage = "Войдите, чтобы оставить отзыв.";
            return;
        }

        if (SelectedService == null)
        {
            StatusMessage = "Выберите услугу.";
            return;
        }

        var (ok, err) = await reviewsService.AddAsync(user.Id, SelectedService.Id, Rating, Comment);
        if (!ok)
        {
            StatusMessage = err ?? "Ошибка.";
            return;
        }

        Comment = "";
        StatusMessage = "Отзыв добавлен.";
        await LoadAsync();
    }
}
