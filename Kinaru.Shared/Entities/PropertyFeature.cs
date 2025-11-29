namespace Kinaru.Shared.Entities;

public class PropertyFeature : BaseEntity
{
    public Guid PropertyId { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string? Icone { get; set; }
    
    public Property Property { get; set; } = null!;
}
