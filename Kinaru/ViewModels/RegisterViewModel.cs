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
    private int currentStep = 1;

    [ObservableProperty]
    private string nom = string.Empty;

    [ObservableProperty]
    private string prenom = string.Empty;

    [ObservableProperty]
    private string lieuHabitation = string.Empty;

    [ObservableProperty]
    private string telephone = string.Empty;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    public RegisterViewModel(AuthService authService)
    {
        _authService = authService;
    }

    [RelayCommand]
    private void NextStep()
    {
        if (CurrentStep == 1)
        {
            if (string.IsNullOrWhiteSpace(Nom) || string.IsNullOrWhiteSpace(Prenom) ||
                string.IsNullOrWhiteSpace(LieuHabitation) || string.IsNullOrWhiteSpace(Telephone))
            {
                ErrorMessage = "Veuillez remplir tous les champs.";
                return;
            }
            CurrentStep = 2;
            ErrorMessage = string.Empty;
        }
    }

    [RelayCommand]
    private void PreviousStep()
    {
        if (CurrentStep == 2)
        {
            CurrentStep = 1;
            ErrorMessage = string.Empty;
        }
    }

    [RelayCommand]
    private async Task RegisterAsync()
    {
        if (IsBusy) return;

        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Veuillez remplir tous les champs.";
            return;
        }

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
                Type = UserType.Acheteur
            };

            var response = await _authService.RegisterAsync(request);
            
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
