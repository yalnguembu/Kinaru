using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kinaru.Services;
using Kinaru.Shared.DTOs.Properties;
using System.Collections.ObjectModel;

namespace Kinaru.ViewModels;

public partial class HomeViewModel : ObservableObject
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

    public ObservableCollection<PropertyListDto> FeaturedProperties { get; } = new();
    public ObservableCollection<PropertyListDto> AllProperties { get; } = new();

    public HomeViewModel(IPropertyService propertyService)
    {
        _propertyService = propertyService;
    }

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            var featured = await _propertyService.GetFeaturedPropertiesAsync(6);
            var all = await _propertyService.GetPropertiesAsync();

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
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            var results = await _propertyService.SearchPropertiesAsync(SearchText);

            AllProperties.Clear();
            foreach (var property in results)
            {
                AllProperties.Add(property);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error searching properties: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
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
    private async Task ToggleFavoriteAsync(Guid propertyId)
    {
        // TODO: Implement favorite toggle
    }
}
