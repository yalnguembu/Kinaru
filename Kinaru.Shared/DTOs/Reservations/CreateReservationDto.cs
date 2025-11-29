namespace Kinaru.Shared.DTOs.Reservations;

public class CreateReservationDto
{
    public Guid PropertyId { get; set; }
    public DateTime DateReservation { get; set; }
    public TimeSpan HeureDebut { get; set; }
    public TimeSpan HeureFin { get; set; }
    public string? Message { get; set; }
}
