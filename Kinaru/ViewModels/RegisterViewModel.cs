using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kinaru.Services;
using Kinaru.Shared.DTOs.Auth;
using Kinaru.Shared.Enums;

namespace Kinaru.ViewModels;

public partial class RegisterViewModel : ObservableObject
{
    private readonly AuthService _authService;

    [ObservableProperty]
    private string nom;

    [ObservableProperty]
    private string prenom;

    [ObservableProperty]
    private string email;

    [ObservableProperty]
    private string password;

    [ObservableProperty]
    private string telephone;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string errorMessage;

    public RegisterViewModel(AuthService authService)
    {
        _authService = authService;
    }

    [RelayCommand]
    private async Task RegisterAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            var request = new RegisterRequestDto
            {
                Nom = Nom,
                Prenom = Prenom,
                Email = Email,
                Password = Password,
                Telephone = Telephone,
                Type = UserType.Acheteur // Default for now
            };

            var response = await _authService.RegisterAsync(request);
            
            // Navigate to main page or store token
            await Shell.Current.GoToAsync("//MainPage");
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Registration failed: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task GoToLoginAsync()
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }
}
