using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kinaru.Services;
using Kinaru.Shared.DTOs.Reservations;

namespace Kinaru.ViewModels;

[QueryProperty(nameof(PropertyId), "propertyId")]
public partial class PropertyReservationViewModel : ObservableObject
{
    private readonly IReservationService _reservationService;
    private readonly IPropertyService _propertyService;

    [ObservableProperty]
    private Guid propertyId;

    [ObservableProperty]
    private string propertyTitle = string.Empty;

    [ObservableProperty]
    private decimal propertyPrice;

    [ObservableProperty]
    private DateTime selectedDate = DateTime.Today;

    [ObservableProperty]
    private TimeSpan selectedStartTime = new TimeSpan(9, 0, 0);

    [ObservableProperty]
    private TimeSpan selectedEndTime = new TimeSpan(10, 0, 0);

    [ObservableProperty]
    private string message = string.Empty;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    public PropertyReservationViewModel(IReservationService reservationService, IPropertyService propertyService)
    {
        _reservationService = reservationService;
        _propertyService = propertyService;
    }

    partial void OnPropertyIdChanged(Guid value)
    {
        if (value != Guid.Empty)
        {
            _ = LoadPropertyInfoAsync();
        }
    }

    [RelayCommand]
    private async Task LoadPropertyInfoAsync()
    {
        try
        {
            var property = await _propertyService.GetPropertyByIdAsync(PropertyId);
            PropertyTitle = property.Titre;
            PropertyPrice = property.Prix;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading property: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task SubmitReservationAsync()
    {
        if (IsBusy) return;

        if (SelectedDate < DateTime.Today)
        {
            ErrorMessage = "La date de réservation doit être dans le futur.";
            return;
        }

        if (SelectedStartTime >= SelectedEndTime)
        {
            ErrorMessage = "L'heure de fin doit être après l'heure de début.";
            return;
        }

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            var reservation = new CreateReservationDto
            {
                PropertyId = PropertyId,
                DateReservation = SelectedDate,
                HeureDebut = SelectedStartTime,
                HeureFin = SelectedEndTime,
                Message = Message
            };

            await _reservationService.CreateReservationAsync(reservation);

            await Shell.Current.DisplayAlert("Succès", "Votre demande de réservation a été envoyée avec succès!", "OK");
            await Shell.Current.GoToAsync("../..");
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error submitting reservation: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }
}
