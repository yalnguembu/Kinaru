using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kinaru.Services;
using Kinaru.Shared.DTOs.Properties;

namespace Kinaru.ViewModels;

[QueryProperty(nameof(PropertyId), "id")]
public partial class PropertyDetailsViewModel : ObservableObject
{
    private readonly IPropertyService _propertyService;

    [ObservableProperty]
    private Guid propertyId;

    [ObservableProperty]
    private PropertyDetailDto? property;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [ObservableProperty]
    private int currentImageIndex;

    public PropertyDetailsViewModel(IPropertyService propertyService)
    {
        _propertyService = propertyService;
    }

    partial void OnPropertyIdChanged(Guid value)
    {
        if (value != Guid.Empty)
        {
            _ = LoadPropertyAsync();
        }
    }

    [RelayCommand]
    private async Task LoadPropertyAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            Property = await _propertyService.GetPropertyByIdAsync(PropertyId);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading property: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task NavigateToReservationAsync()
    {
        if (Property == null) return;
        await Shell.Current.GoToAsync($"reservation?propertyId={PropertyId}");
    }

    [RelayCommand]
    private async Task NavigateToMessagingAsync()
    {
        if (Property == null) return;
        await Shell.Current.GoToAsync($"messaging?userId={Property.ProprietaireId}");
    }

    [RelayCommand]
    private async Task ToggleFavoriteAsync()
    {
        // TODO: Implement favorite toggle
    }

    [RelayCommand]
    private async Task ShareAsync()
    {
        // TODO: Implement share functionality
    }
}
