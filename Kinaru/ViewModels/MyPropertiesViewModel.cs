using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kinaru.Services;
using Kinaru.Shared.DTOs.Properties;
using Kinaru.Shared.Enums;
using System.Collections.ObjectModel;

namespace Kinaru.ViewModels;

public partial class MyPropertiesViewModel : ObservableObject
{
    private readonly IPropertyService _propertyService;
    private readonly IUserService _userService;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private bool isRefreshing;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [ObservableProperty]
    private PropertyStatus? filterStatus;

    public ObservableCollection<PropertyListDto> Properties { get; } = new();

    public MyPropertiesViewModel(IPropertyService propertyService, IUserService userService)
    {
        _propertyService = propertyService;
        _userService = userService;
    }

    [RelayCommand]
    private async Task FilterByStatusAsync(string status)
    {
        if (string.IsNullOrEmpty(status) || status == "All")
        {
            FilterStatus = null;
        }
        else if (Enum.TryParse<PropertyStatus>(status, out var result))
        {
            FilterStatus = result;
        }
        
        await LoadPropertiesAsync();
    }

    [RelayCommand]
    private async Task LoadPropertiesAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            var userProfile = await _userService.GetMyProfileAsync();
            var properties = await _propertyService.GetUserPropertiesAsync(userProfile.Id);

            Properties.Clear();
            foreach (var property in properties)
            {
                if (FilterStatus == null || property.Statut == FilterStatus)
                {
                    Properties.Add(property);
                }
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
    private async Task DeletePropertyAsync(Guid propertyId)
    {
        if (IsBusy) return;

        bool confirm = await Shell.Current.DisplayAlert("Confirmation", "Are you sure you want to delete this property?", "Yes", "No");
        if (!confirm) return;

        try
        {
            IsBusy = true;
            await _propertyService.DeletePropertyAsync(propertyId);
            
            var propertyToRemove = Properties.FirstOrDefault(p => p.Id == propertyId);
            if (propertyToRemove != null)
            {
                Properties.Remove(propertyToRemove);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to delete property: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task NavigateToAddPropertyAsync()
    {
        await Shell.Current.GoToAsync("addproperty");
    }

    [RelayCommand]
    private async Task NavigateToEditPropertyAsync(Guid propertyId)
    {
        await Shell.Current.GoToAsync($"addproperty?id={propertyId}");
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsRefreshing = true;
        await LoadPropertiesAsync();
    }

    [RelayCommand]
    private async Task UpdatePropertyStatusAsync(PropertyListDto property)
    {
        if (IsBusy) return;

        var action = await Shell.Current.DisplayActionSheet(
            "Changer le statut",
            "Annuler",
            null,
            "Disponible",
            "Vendu",
            "Loué"
        );

        if (action == "Annuler" || action == null) return;

        PropertyStatus newStatus = action switch
        {
            "Disponible" => PropertyStatus.Disponible,
            "Vendu" => PropertyStatus.Vendu,
            "Loué" => PropertyStatus.Loue,
            _ => property.Statut
        };

        if (newStatus == property.Statut) return;

        try
        {
            IsBusy = true;
            var dto = new UpdatePropertyStatusDto { Statut = newStatus };
            await _propertyService.UpdatePropertyStatusAsync(property.Id, dto);
            
            await LoadPropertiesAsync();
            await Shell.Current.DisplayAlert("Succès", "Statut mis à jour", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erreur", $"Échec de la mise à jour: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
