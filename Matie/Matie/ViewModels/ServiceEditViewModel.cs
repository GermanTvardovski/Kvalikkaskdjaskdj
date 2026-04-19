using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Matie.Data;
using Matie.Models;
using Matie.Services;

namespace Matie.ViewModels;

public partial class ServiceEditViewModel : ObservableObject
{
    private readonly IServiceRepository serviceRepository;
    private readonly IImagePickerService imagePickerService;

    [ObservableProperty]
    private int serviceId;

    [ObservableProperty]
    private string serviceName = "";

    [ObservableProperty]
    private string description = "";

    [ObservableProperty]
    private string priceText = "";

    [ObservableProperty]
    private ObservableCollection<Category> categories = new();

    [ObservableProperty]
    private Category? selectedCategory;

    [ObservableProperty]
    private ObservableCollection<MasterPickModel> masters = new();

    [ObservableProperty]
    private string errorMessage = "";

    [ObservableProperty]
    private byte[]? selectedImageBytes;

    public event EventHandler<bool>? CloseRequested;

    public ServiceEditViewModel(IServiceRepository serviceRepository, IImagePickerService imagePickerService)
    {
        this.serviceRepository = serviceRepository;
        this.imagePickerService = imagePickerService;
    }

    public async Task InitializeAsync(int? serviceId)
    {
        ErrorMessage = "";
        var categoryList = await serviceRepository.GetCategoriesAsync();
        Categories = new ObservableCollection<Category>(categoryList);
        var masterList = await serviceRepository.GetMastersAsync();
        var masterPicks = masterList
            .Select(m => new MasterPickModel
            {
                Id = m.Id,
                Name = m.Name ?? "",
                IsSelected = false
            })
            .ToList();

        Masters = new ObservableCollection<MasterPickModel>(masterPicks);

        if (serviceId.HasValue)
        {
            var entity = await serviceRepository.GetServiceByIdAsync(serviceId.Value);
            if (entity == null)
            {
                ErrorMessage = "Услуга не найдена.";
                return;
            }

            ServiceId = entity.Id;
            ServiceName = entity.Name ?? "";
            Description = entity.Description ?? "";
            PriceText = entity.Price?.ToString(CultureInfo.CurrentCulture) ?? "";
            SelectedCategory = Categories.FirstOrDefault(c => c.Id == entity.CategoryId);

            var linked = await serviceRepository.GetMasterIdsForServiceAsync(entity.Id);
            foreach (var pick in Masters)
            {
                pick.IsSelected = linked.Contains(pick.Id);
            }

            SelectedImageBytes = entity.ImageData?.ToArray();
        }
        else
        {
            ServiceId = 0;
            ServiceName = "";
            Description = "";
            PriceText = "";
            SelectedCategory = Categories.FirstOrDefault();
            SelectedImageBytes = null;
            foreach (var pick in Masters)
            {
                pick.IsSelected = false;
            }
        }
    }

    [RelayCommand]
    private async Task PickPhotoAsync()
    {
        ErrorMessage = "";
        try
        {
            var bytes = await imagePickerService.PickImageBytesAsync();
            if (bytes != null)
            {
                SelectedImageBytes = bytes;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    [RelayCommand]
    private void ClearPhoto()
    {
        ErrorMessage = "";
        SelectedImageBytes = null;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        ErrorMessage = "";
        if (string.IsNullOrWhiteSpace(ServiceName))
        {
            ErrorMessage = "Укажите название.";
            return;
        }

        if (!decimal.TryParse(PriceText, NumberStyles.Number, CultureInfo.CurrentCulture, out var price))
        {
            ErrorMessage = "Некорректная цена.";
            return;
        }

        if (SelectedCategory == null)
        {
            ErrorMessage = "Выберите коллекцию.";
            return;
        }

        var selectedMasterIds = Masters.Where(m => m.IsSelected).Select(m => m.Id).ToList();

        try
        {
            if (ServiceId == 0)
            {
                var entity = new Service
                {
                    Name = ServiceName.Trim(),
                    Description = string.IsNullOrWhiteSpace(Description) ? null : Description.Trim(),
                    Price = price,
                    CategoryId = SelectedCategory.Id,
                    ImageData = SelectedImageBytes
                };

                await serviceRepository.AddServiceAsync(entity, selectedMasterIds);
            }
            else
            {
                var entity = new Service
                {
                    Id = ServiceId,
                    Name = ServiceName.Trim(),
                    Description = string.IsNullOrWhiteSpace(Description) ? null : Description.Trim(),
                    Price = price,
                    CategoryId = SelectedCategory.Id,
                    ImageData = SelectedImageBytes
                };

                await serviceRepository.UpdateServiceAsync(entity, selectedMasterIds);
            }

            CloseRequested?.Invoke(this, true);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        CloseRequested?.Invoke(this, false);
    }
}
