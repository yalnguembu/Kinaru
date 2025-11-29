namespace Kinaru.Shared.DTOs.Agents;

public class CreateAgentDto
{
    public string Biographie { get; set; } = string.Empty;
    public string Specialites { get; set; } = string.Empty;
    public string ZoneIntervention { get; set; } = string.Empty;
    public int AnneesExperience { get; set; }
    public string? SiteWeb { get; set; }
    public string? Facebook { get; set; }
    public string? LinkedIn { get; set; }
    public string? Instagram { get; set; }
}
