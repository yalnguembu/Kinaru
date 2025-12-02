using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kinaru.Services;

namespace Kinaru.ViewModels;

public partial class AgentDashboardViewModel : ObservableObject
{
    private readonly IPropertyService _propertyService;
    private readonly IReservationService _reservationService;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private int totalProperties;

    [ObservableProperty]
    private int pendingReservations;

    public AgentDashboardViewModel(IPropertyService propertyService, IReservationService reservationService)
    {
        _propertyService = propertyService;
        _reservationService = reservationService;
    }

    [RelayCommand]
    private async Task LoadStatisticsAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;

            var propertiesTask = _propertyService.GetPropertiesAsync();
            var reservationsTask = _reservationService.GetAgentReservationsAsync();

            await Task.WhenAll(propertiesTask, reservationsTask);

            TotalProperties = propertiesTask.Result.Count;
            PendingReservations = reservationsTask.Result.Count(r => r.Statut == Shared.Enums.ReservationStatus.EnAttente);
        }
        catch (Exception)
        {
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task NavigateToMyPropertiesAsync()
    {
        await Shell.Current.GoToAsync("myproperties");
    }

    [RelayCommand]
    private async Task NavigateToReservationsAsync()
    {
        await Shell.Current.GoToAsync("agentreservations");
    }

    [RelayCommand]
    private async Task NavigateToProfileAsync()
    {
        await Shell.Current.GoToAsync("agentprofile");
    }

    [RelayCommand]
    private async Task NavigateToCalendarAsync()
    {
        await Shell.Current.GoToAsync("agentcalendar");
    }
}
