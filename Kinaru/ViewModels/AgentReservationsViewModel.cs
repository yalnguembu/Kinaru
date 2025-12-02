using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kinaru.Services;
using Kinaru.Shared.DTOs.Reservations;
using Kinaru.Shared.Enums;
using System.Collections.ObjectModel;

namespace Kinaru.ViewModels;

public partial class AgentReservationsViewModel : ObservableObject
{
    private readonly IReservationService _reservationService;
    private readonly IUserService _userService;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private bool isRefreshing;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    public ObservableCollection<ReservationDto> PendingReservations { get; } = new();
    public ObservableCollection<ReservationDto> ConfirmedReservations { get; } = new();
    public ObservableCollection<ReservationDto> PastReservations { get; } = new();

    public AgentReservationsViewModel(IReservationService reservationService, IUserService userService)
    {
        _reservationService = reservationService;
        _userService = userService;
    }

    [RelayCommand]
    private async Task LoadReservationsAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            var reservations = await _reservationService.GetAgentReservationsAsync();

            PendingReservations.Clear();
            ConfirmedReservations.Clear();
            PastReservations.Clear();

            foreach (var reservation in reservations)
            {
                if (reservation.Statut == ReservationStatus.EnAttente)
                {
                    PendingReservations.Add(reservation);
                }
                else if (reservation.Statut == ReservationStatus.Confirmee && reservation.DateReservation >= DateTime.Today)
                {
                    ConfirmedReservations.Add(reservation);
                }
                else
                {
                    PastReservations.Add(reservation);
                }
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading reservations: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    private async Task AcceptReservationAsync(Guid reservationId)
    {
        await UpdateStatusAsync(reservationId, ReservationStatus.Confirmee);
    }

    [RelayCommand]
    private async Task RejectReservationAsync(Guid reservationId)
    {
        await UpdateStatusAsync(reservationId, ReservationStatus.Annulee);
    }

    private async Task UpdateStatusAsync(Guid reservationId, ReservationStatus status)
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            var dto = new UpdateReservationStatusDto { Statut = status };
            await _reservationService.UpdateReservationStatusAsync(reservationId, dto);
            
            // Reload to refresh lists
            await LoadReservationsAsync();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to update status: {ex.Message}", "OK");
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsRefreshing = true;
        await LoadReservationsAsync();
    }
}
