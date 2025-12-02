using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kinaru.Services;
using Kinaru.Shared.DTOs.Agents;
using System.Collections.ObjectModel;

namespace Kinaru.ViewModels;

public partial class AgentCalendarViewModel : ObservableObject
{
    private readonly IAgentService _agentService;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [ObservableProperty]
    private DateTime selectedDate = DateTime.Today;

    [ObservableProperty]
    private TimeSpan newStartTime = new(9, 0, 0);

    [ObservableProperty]
    private TimeSpan newEndTime = new(17, 0, 0);

    public ObservableCollection<AgentAvailabilityDto> Availabilities { get; } = new();

    public AgentCalendarViewModel(IAgentService agentService)
    {
        _agentService = agentService;
    }

    partial void OnSelectedDateChanged(DateTime value)
    {
        _ = LoadAvailabilitiesAsync();
    }

    [RelayCommand]
    private async Task LoadAvailabilitiesAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            // Load for the whole month of the selected date
            var from = new DateTime(SelectedDate.Year, SelectedDate.Month, 1);
            var to = from.AddMonths(1).AddDays(-1);

            var availabilities = await _agentService.GetMyAvailabilityAsync(from, to);

            Availabilities.Clear();
            foreach (var item in availabilities.OrderBy(a => a.Date).ThenBy(a => a.HeureDebut))
            {
                // Filter for selected date in the list view if needed, or show all
                // For now, let's show only selected date in the list
                if (item.Date.Date == SelectedDate.Date)
                {
                    Availabilities.Add(item);
                }
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading availabilities: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task AddAvailabilityAsync()
    {
        if (IsBusy) return;

        if (NewStartTime >= NewEndTime)
        {
            await Shell.Current.DisplayAlert("Erreur", "L'heure de début doit être avant l'heure de fin", "OK");
            return;
        }

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            var dto = new CreateAvailabilityDto
            {
                Date = SelectedDate,
                HeureDebut = NewStartTime,
                HeureFin = NewEndTime
            };

            await _agentService.CreateAvailabilityAsync(dto);
            await LoadAvailabilitiesAsync();
            
            await Shell.Current.DisplayAlert("Succès", "Disponibilité ajoutée", "OK");
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error adding availability: {ex.Message}";
            await Shell.Current.DisplayAlert("Erreur", ErrorMessage, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task DeleteAvailabilityAsync(Guid id)
    {
        if (IsBusy) return;

        bool confirm = await Shell.Current.DisplayAlert("Confirmation", "Voulez-vous supprimer ce créneau ?", "Oui", "Non");
        if (!confirm) return;

        try
        {
            IsBusy = true;
            await _agentService.DeleteAvailabilityAsync(id);
            await LoadAvailabilitiesAsync();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erreur", $"Failed to delete: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task NavigateBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
