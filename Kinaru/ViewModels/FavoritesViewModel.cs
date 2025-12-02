using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kinaru.Services;
using Kinaru.Shared.DTOs.Favorites;
using System.Collections.ObjectModel;

namespace Kinaru.ViewModels;

public partial class FavoritesViewModel : ObservableObject
{
    private readonly IFavoriteService _favoriteService;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private bool isRefreshing;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    public ObservableCollection<FavoriteDto> Favorites { get; } = new();

    public FavoritesViewModel(IFavoriteService favoriteService)
    {
        _favoriteService = favoriteService;
    }

    [RelayCommand]
    private async Task LoadFavoritesAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            var favorites = await _favoriteService.GetUserFavoritesAsync();

            Favorites.Clear();
            foreach (var favorite in favorites)
            {
                Favorites.Add(favorite);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading favorites: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    private async Task RemoveFavoriteAsync(Guid propertyId)
    {
        if (IsBusy) return;

        try
        {
            await _favoriteService.RemoveFavoriteAsync(propertyId);
            
            var favoriteToRemove = Favorites.FirstOrDefault(f => f.PropertyId == propertyId);
            if (favoriteToRemove != null)
            {
                Favorites.Remove(favoriteToRemove);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to remove favorite: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task NavigateToDetailsAsync(Guid propertyId)
    {
        await Shell.Current.GoToAsync($"propertydetails?id={propertyId}");
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsRefreshing = true;
        await LoadFavoritesAsync();
    }
}
