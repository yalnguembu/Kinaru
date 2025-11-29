using Kinaru.Shared.DTOs.Reservations;

namespace Kinaru.Api.Services.Interfaces;

public interface IReservationService
{
    Task<ReservationDto> CreateReservationAsync(Guid userId, CreateReservationDto dto);
    Task<List<ReservationDto>> GetUserReservationsAsync(Guid userId);
    Task<List<ReservationDto>> GetPropertyReservationsAsync(Guid propertyId, Guid ownerId);
    Task<ReservationDto> UpdateReservationStatusAsync(Guid reservationId, UpdateReservationStatusDto dto, Guid userId);
}
