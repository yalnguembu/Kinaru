using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kinaru.Services;
using Kinaru.Shared.DTOs.Users;

namespace Kinaru.ViewModels;

public partial class ProfileViewModel : ObservableObject
{
    private readonly IUserService _userService;
    private readonly AuthService _authService;

    [ObservableProperty]
    private UserProfileDto userProfile;

    [ObservableProperty]
    private bool isEditing;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    // Editable fields
    [ObservableProperty]
    private string editNom;

    [ObservableProperty]
    private string editPrenom;

    [ObservableProperty]
    private string editTelephone;

    [ObservableProperty]
    private string editLieuHabitation;

    public ProfileViewModel(IUserService userService, AuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }

    [RelayCommand]
    private async Task LoadProfileAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            UserProfile = await _userService.GetMyProfileAsync();
            ResetEditFields();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading profile: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void ResetEditFields()
    {
        if (UserProfile != null)
        {
            EditNom = UserProfile.Nom;
            EditPrenom = UserProfile.Prenom;
            EditTelephone = UserProfile.Telephone;
            EditLieuHabitation = UserProfile.LieuHabitation;
        }
    }

    [RelayCommand]
    private void ToggleEditMode()
    {
        if (IsEditing)
        {
            // Cancel editing
            ResetEditFields();
        }
        IsEditing = !IsEditing;
    }

    [RelayCommand]
    private async Task SaveProfileAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            var updateDto = new UpdateUserDto
            {
                Nom = EditNom,
                Prenom = EditPrenom,
                Telephone = EditTelephone,
                LieuHabitation = EditLieuHabitation
            };

            UserProfile = await _userService.UpdateMyProfileAsync(updateDto);
            IsEditing = false;
            await Shell.Current.DisplayAlert("Success", "Profile updated successfully", "OK");
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error updating profile: {ex.Message}";
            await Shell.Current.DisplayAlert("Error", ErrorMessage, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        _authService.Logout();
        await Shell.Current.GoToAsync("//Login");
    }

    [RelayCommand]
    private async Task ChangePasswordAsync()
    {
        // TODO: Show change password modal
        await Shell.Current.DisplayAlert("Info", "Change Password functionality to be implemented", "OK");
    }

    [RelayCommand]
    private async Task NavigateToAgentDashboardAsync()
    {
        await Shell.Current.GoToAsync("agentdashboard");
    }
}
