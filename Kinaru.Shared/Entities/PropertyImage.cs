namespace Kinaru.Shared.Entities;

public class PropertyImage : BaseEntity
{
    public Guid PropertyId { get; set; }
    public string Url { get; set; } = string.Empty;
    public int Ordre { get; set; }
    public bool IsPrincipale { get; set; } = false;
    
    public Property Property { get; set; } = null!;
}
