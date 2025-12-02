using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kinaru.Services;
using Kinaru.Shared.DTOs.Agents;

namespace Kinaru.ViewModels;

public partial class AgentProfileViewModel : ObservableObject
{
    private readonly IAgentService _agentService;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private bool isEditing;

    [ObservableProperty]
    private bool hasProfile;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [ObservableProperty]
    private AgentDto? agentProfile;

    [ObservableProperty]
    private string editBiographie = string.Empty;

    [ObservableProperty]
    private string editSpecialites = string.Empty;

    [ObservableProperty]
    private string editZoneIntervention = string.Empty;

    [ObservableProperty]
    private int editAnneesExperience;

    [ObservableProperty]
    private string editSiteWeb = string.Empty;

    [ObservableProperty]
    private string editFacebook = string.Empty;

    [ObservableProperty]
    private string editLinkedIn = string.Empty;

    [ObservableProperty]
    private string editInstagram = string.Empty;

    public AgentProfileViewModel(IAgentService agentService)
    {
        _agentService = agentService;
    }

    [RelayCommand]
    private async Task LoadProfileAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            AgentProfile = await _agentService.GetMyAgentProfileAsync();
            HasProfile = AgentProfile != null;
            
            if (HasProfile)
            {
                ResetEditFields();
            }
        }
        catch (Exception ex)
        {
            HasProfile = false;
            ErrorMessage = $"Error loading profile: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void ResetEditFields()
    {
        if (AgentProfile != null)
        {
            EditBiographie = AgentProfile.Biographie ?? string.Empty;
            EditSpecialites = AgentProfile.Specialites ?? string.Empty;
            EditZoneIntervention = AgentProfile.ZoneIntervention ?? string.Empty;
            EditAnneesExperience = AgentProfile.AnneesExperience;
            EditSiteWeb = AgentProfile.SiteWeb ?? string.Empty;
            EditFacebook = AgentProfile.Facebook ?? string.Empty;
            EditLinkedIn = AgentProfile.LinkedIn ?? string.Empty;
            EditInstagram = AgentProfile.Instagram ?? string.Empty;
        }
    }

    [RelayCommand]
    private void ToggleEditMode()
    {
        if (IsEditing)
        {
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

            var updateDto = new UpdateAgentDto
            {
                Biographie = EditBiographie,
                Specialites = EditSpecialites,
                ZoneIntervention = EditZoneIntervention,
                AnneesExperience = EditAnneesExperience,
                SiteWeb = string.IsNullOrWhiteSpace(EditSiteWeb) ? null : EditSiteWeb,
                Facebook = string.IsNullOrWhiteSpace(EditFacebook) ? null : EditFacebook,
                LinkedIn = string.IsNullOrWhiteSpace(EditLinkedIn) ? null : EditLinkedIn,
                Instagram = string.IsNullOrWhiteSpace(EditInstagram) ? null : EditInstagram
            };

            AgentProfile = await _agentService.UpdateMyAgentProfileAsync(updateDto);
            HasProfile = true;
            IsEditing = false;
            
            await Shell.Current.DisplayAlert("Succès", "Profil agent mis à jour avec succès", "OK");
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error saving profile: {ex.Message}";
            await Shell.Current.DisplayAlert("Erreur", ErrorMessage, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
