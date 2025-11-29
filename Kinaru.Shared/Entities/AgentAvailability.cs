namespace Kinaru.Shared.Entities;

public class AgentAvailability : BaseEntity
{
    public Guid AgentId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan HeureDebut { get; set; }
    public TimeSpan HeureFin { get; set; }
    public bool Disponible { get; set; } = true;
    
    public Agent Agent { get; set; } = null!;
}
