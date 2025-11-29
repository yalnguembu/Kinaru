using Kinaru.Shared.Enums;

namespace Kinaru.Shared.Entities;

public class Reservation : BaseEntity
{
    public Guid PropertyId { get; set; }
    public Guid UserId { get; set; }
    public DateTime DateReservation { get; set; }
    public TimeSpan HeureDebut { get; set; }
    public TimeSpan HeureFin { get; set; }
    public ReservationStatus Statut { get; set; } = ReservationStatus.EnAttente;
    public string? Notes { get; set; }
    
    public Property Property { get; set; } = null!;
    public User User { get; set; } = null!;
}
