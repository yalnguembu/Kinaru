using Kinaru.Shared.DTOs.Properties;
using Kinaru.Shared.DTOs.Users;
using Kinaru.Shared.Enums;

namespace Kinaru.Shared.DTOs.Reservations;

public class ReservationDto
{
    public Guid Id { get; set; }
    public Guid PropertyId { get; set; }
    public Guid UserId { get; set; }
    public DateTime DateReservation { get; set; }
    public TimeSpan HeureDebut { get; set; }
    public TimeSpan HeureFin { get; set; }
    public ReservationStatus Statut { get; set; }
    public string? Message { get; set; }
    
    public PropertyListDto Property { get; set; } = null!;
    public UserBasicDto User { get; set; } = null!;
}
