namespace Kinaru.Shared.DTOs.Agents;

public class CreateAvailabilityDto
{
    public DateTime Date { get; set; }
    public TimeSpan HeureDebut { get; set; }
    public TimeSpan HeureFin { get; set; }
}
