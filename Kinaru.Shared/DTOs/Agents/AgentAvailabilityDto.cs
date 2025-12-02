namespace Kinaru.Shared.DTOs.Agents;

public class AgentAvailabilityDto
{
    public Guid Id { get; set; }
    public Guid AgentId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan HeureDebut { get; set; }
    public TimeSpan HeureFin { get; set; }
    public bool Disponible { get; set; }
}
