namespace Kinaru.Shared.Entities;

public class Agent : BaseEntity
{
    public Guid UserId { get; set; }
    public string NomComplet { get; set; } = string.Empty;
    public string Agence { get; set; } = string.Empty;
    public string Specialite { get; set; } = string.Empty;
    public float NoteMoyenne { get; set; } = 0;
    public int NombreAvis { get; set; } = 0;
    public string? Bio { get; set; }
    public string ZoneIntervention { get; set; } = string.Empty;
    public int AnneesExperience { get; set; }
    public string? SiteWeb { get; set; }
    public string? Facebook { get; set; }
    public string? LinkedIn { get; set; }
    public string? Instagram { get; set; }
    public bool Verifie { get; set; }
    
    public User User { get; set; } = null!;
    public ICollection<AgentAvailability> Availabilities { get; set; } = new List<AgentAvailability>();
}
