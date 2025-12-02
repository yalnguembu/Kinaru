using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kinaru.Services;
using Kinaru.Shared.DTOs.Properties;

namespace Kinaru.ViewModels;

[QueryProperty(nameof(PropertyId), "id")]
public partial class PropertyDetailsViewModel : ObservableObject
{
    private readonly IPropertyService _propertyService;
    private readonly IUserService _userService;

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

    [ObservableProperty]
    private bool isOwner;

    public PropertyDetailsViewModel(IPropertyService propertyService, IUserService userService)
    {
        _propertyService = propertyService;
        _userService = userService;
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
            
            var currentUser = await _userService.GetMyProfileAsync();
            IsOwner = Property.Vendeur.Id == currentUser.Id;
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
        await Shell.Current.GoToAsync($"messaging?userId={Property.Vendeur.Id}");
    }

    [RelayCommand]
    private async Task EditPropertyAsync()
    {
        if (Property == null) return;
        await Shell.Current.GoToAsync($"addproperty?id={PropertyId}");
    }

    [RelayCommand]
    private async Task DeletePropertyAsync()
    {
        if (Property == null) return;

        bool confirm = await Shell.Current.DisplayAlert(
            "Confirmation",
            "Êtes-vous sûr de vouloir supprimer cette propriété ?",
            "Oui",
            "Non"
        );

        if (!confirm) return;

        try
        {
            IsBusy = true;
            await _propertyService.DeletePropertyAsync(PropertyId);
            await Shell.Current.DisplayAlert("Succès", "Propriété supprimée", "OK");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erreur", $"Échec de la suppression: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
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
