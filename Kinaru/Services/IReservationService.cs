using Kinaru.Shared.DTOs.Reservations;
using Refit;

namespace Kinaru.Services;

public interface IReservationService
{
    [Post("/api/reservations")]
    Task<ReservationDto> CreateReservationAsync([Body] CreateReservationDto reservation);

    [Get("/api/reservations/me")]
    Task<List<ReservationDto>> GetUserReservationsAsync();

    [Get("/api/reservations/property/{propertyId}")]
    Task<List<ReservationDto>> GetPropertyReservationsAsync(Guid propertyId);

    [Get("/api/reservations/agent")]
    Task<List<ReservationDto>> GetAgentReservationsAsync();

    [Put("/api/reservations/{id}/status")]
    Task UpdateReservationStatusAsync(Guid id, [Body] UpdateReservationStatusDto status);
}
