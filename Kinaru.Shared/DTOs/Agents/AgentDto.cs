using Kinaru.Shared.DTOs.Users;

namespace Kinaru.Shared.DTOs.Agents;

public class AgentDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Biographie { get; set; } = string.Empty;
    public string Specialites { get; set; } = string.Empty;
    public string ZoneIntervention { get; set; } = string.Empty;
    public int AnneesExperience { get; set; }
    public string? SiteWeb { get; set; }
    public string? Facebook { get; set; }
    public string? LinkedIn { get; set; }
    public string? Instagram { get; set; }
    public double NoteMoyenne { get; set; }
    public int NombreAvis { get; set; }
    public bool Verifie { get; set; }
    
    public UserBasicDto User { get; set; } = null!;
}
