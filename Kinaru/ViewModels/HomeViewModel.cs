using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kinaru.Services;
using Kinaru.Shared.DTOs.Properties;
using System.Collections.ObjectModel;

namespace Kinaru.ViewModels;

public partial class HomeViewModel : ObservableObject, IQueryAttributable
{
    private readonly IPropertyService _propertyService;

    [ObservableProperty]
    private string searchText = string.Empty;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private bool isRefreshing;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [ObservableProperty]
    private PropertyFilterDto currentFilter = new();

    public ObservableCollection<PropertyListDto> FeaturedProperties { get; } = new();
    public ObservableCollection<PropertyListDto> AllProperties { get; } = new();

    public HomeViewModel(IPropertyService propertyService)
    {
        _propertyService = propertyService;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("FilterResult") && query["FilterResult"] is PropertyFilterDto filterResult)
        {
            CurrentFilter = filterResult;
            // Update search text if it was changed in filter
            if (!string.IsNullOrEmpty(CurrentFilter.SearchTerm))
            {
                SearchText = CurrentFilter.SearchTerm;
            }
            // Trigger reload with new filter
            IsRefreshing = true;
            _ = LoadDataAsync();
        }
    }

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            // Update filter with current search text
            CurrentFilter.SearchTerm = SearchText;

            var featuredTask = _propertyService.GetFeaturedPropertiesAsync(6);
            var allTask = _propertyService.GetPropertiesAsync(CurrentFilter);

            await Task.WhenAll(featuredTask, allTask);

            var featured = await featuredTask;
            var all = await allTask;

            FeaturedProperties.Clear();
            foreach (var property in featured)
            {
                FeaturedProperties.Add(property);
            }

            AllProperties.Clear();
            foreach (var property in all)
            {
                AllProperties.Add(property);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading properties: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    private async Task SearchAsync()
    {
        // Reset other filters when searching manually, or keep them?
        // Let's keep them and just update the search term
        CurrentFilter.SearchTerm = SearchText;
        await LoadDataAsync();
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsRefreshing = true;
        await LoadDataAsync();
    }

    [RelayCommand]
    private async Task NavigateToDetailsAsync(Guid propertyId)
    {
        await Shell.Current.GoToAsync($"propertydetails?id={propertyId}");
    }

    [RelayCommand]
    private async Task NavigateToFiltersAsync()
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { "Filter", CurrentFilter }
        };
        await Shell.Current.GoToAsync("filters", navigationParameter);
    }

    [RelayCommand]
    private async Task ToggleFavoriteAsync(Guid propertyId)
    {
        // TODO: Implement favorite toggle
    }
}
