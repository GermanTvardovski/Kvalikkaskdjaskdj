using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Matie.Models;
using Matie.Services;

namespace Matie.ViewModels;

public partial class ServicesViewModel : ObservableObject
{
    private const int pageSize = 3;
    private const string customKeyword = "Кастом";
    private const string cosplayKeyword = "Косплей";

    private readonly IServiceRepository serviceRepository;
    private readonly IUiDialogService uiDialogService;

    [ObservableProperty]
    private ObservableCollection<ServiceCardModel> services = new();

    [ObservableProperty]
    private ObservableCollection<CategoryFilterItem> categoryOptions = new();

    [ObservableProperty]
    private CategoryFilterItem? selectedCategoryFilter;

    [ObservableProperty]
    private string searchText = "";

    [ObservableProperty]
    private int currentPage = 1;

    [ObservableProperty]
    private int totalCount;

    [ObservableProperty]
    private string pageIndicatorText = "";

    [ObservableProperty]
    private bool canGoPrevious;

    [ObservableProperty]
    private bool canGoNext;

    [ObservableProperty]
    private bool canEditServices;

    public ServicesViewModel(IServiceRepository serviceRepository, IUiDialogService uiDialogService)
    {
        this.serviceRepository = serviceRepository;
        this.uiDialogService = uiDialogService;
    }

    public void SetModerationMode(bool enabled)
    {
        CanEditServices = enabled;
    }

    public async Task LoadCategoriesAsync()
    {
        var categories = await serviceRepository.GetCategoriesAsync();
        var options = new List<CategoryFilterItem>
        {
            new() { Id = null, DisplayName = "Все" }
        };

        foreach (var category in categories)
        {
            options.Add(new CategoryFilterItem
            {
                Id = category.Id,
                DisplayName = category.Name ?? "—"
            });
        }

        CategoryOptions = new ObservableCollection<CategoryFilterItem>(options);
        SelectedCategoryFilter = CategoryOptions.FirstOrDefault();
    }

    public async Task RefreshAsync()
    {
        var categoryId = SelectedCategoryFilter?.Id;
        var pageIndex = Math.Max(0, CurrentPage - 1);
        var (items, total) = await serviceRepository.GetServicesPageAsync(
            string.IsNullOrWhiteSpace(SearchText) ? null : SearchText,
            categoryId,
            pageIndex,
            pageSize);

        var totalPages = Math.Max(1, (int)Math.Ceiling(total / (double)pageSize));
        if (CurrentPage > totalPages)
        {
            CurrentPage = totalPages;
            pageIndex = Math.Max(0, CurrentPage - 1);
            (items, total) = await serviceRepository.GetServicesPageAsync(
                string.IsNullOrWhiteSpace(SearchText) ? null : SearchText,
                categoryId,
                pageIndex,
                pageSize);
        }

        TotalCount = total;
        Services = new ObservableCollection<ServiceCardModel>(items);
        updatePaginationUi(total);
    }

    [RelayCommand]
    private async Task SearchAsync()
    {
        CurrentPage = 1;
        await RefreshAsync();
    }

    [RelayCommand]
    private async Task GoPreviousAsync()
    {
        if (CurrentPage <= 1)
        {
            return;
        }

        CurrentPage--;
        await RefreshAsync();
    }

    [RelayCommand]
    private async Task GoNextAsync()
    {
        var totalPages = Math.Max(1, (int)Math.Ceiling(TotalCount / (double)pageSize));
        if (CurrentPage >= totalPages)
        {
            return;
        }

        CurrentPage++;
        await RefreshAsync();
    }

    [RelayCommand]
    private async Task AddServiceAsync()
    {
        var saved = await uiDialogService.ShowServiceEditorAsync(null);
        if (saved)
        {
            await RefreshAsync();
        }
    }

    [RelayCommand]
    private async Task EditServiceAsync(ServiceCardModel? card)
    {
        if (card == null)
        {
            return;
        }

        var saved = await uiDialogService.ShowServiceEditorAsync(card.Id);
        if (saved)
        {
            await RefreshAsync();
        }
    }

    [RelayCommand]
    private async Task ApplyCustomFilterAsync()
    {
        await applyKeywordFilterAsync(customKeyword);
    }

    [RelayCommand]
    private async Task ApplyCosplayFilterAsync()
    {
        await applyKeywordFilterAsync(cosplayKeyword);
    }

    [RelayCommand]
    private async Task ApplyDirectionFilterAsync(string? keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            return;
        }

        await applyKeywordFilterAsync(keyword.Trim());
    }

    partial void OnSelectedCategoryFilterChanged(CategoryFilterItem? value)
    {
        if (value == null)
        {
            return;
        }

        CurrentPage = 1;
        _ = RefreshAsync();
    }

    private async Task applyKeywordFilterAsync(string keyword)
    {
        var categories = await serviceRepository.GetCategoriesAsync();
        var match = categories.FirstOrDefault(c =>
            c.Name != null && c.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase));

        CategoryFilterItem? next = match == null
            ? CategoryOptions.FirstOrDefault(o => o.Id == null)
            : CategoryOptions.FirstOrDefault(o => o.Id == match.Id)
              ?? CategoryOptions.FirstOrDefault(o => o.Id == null);

        if (next == null)
        {
            return;
        }

        if (!ReferenceEquals(SelectedCategoryFilter, next))
        {
            SelectedCategoryFilter = next;
        }
        else
        {
            CurrentPage = 1;
            await RefreshAsync();
        }
    }

    private void updatePaginationUi(int total)
    {
        var totalPages = Math.Max(1, (int)Math.Ceiling(total / (double)pageSize));
        CanGoPrevious = CurrentPage > 1;
        CanGoNext = CurrentPage < totalPages;

        if (total == 0)
        {
            PageIndicatorText = "Нет записей";
            return;
        }

        var startItem = (CurrentPage - 1) * pageSize + 1;
        var endItem = Math.Min(CurrentPage * pageSize, total);
        PageIndicatorText = $"{startItem}–{endItem} из {total}";
    }
}
