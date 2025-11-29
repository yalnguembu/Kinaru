namespace Kinaru.Shared.Entities;

public class Message : BaseEntity
{
    public Guid ExpediteurId { get; set; }
    public Guid DestinataireId { get; set; }
    public Guid? PropertyId { get; set; }
    public string Contenu { get; set; } = string.Empty;
    public bool Lu { get; set; } = false;
    
    public User Expediteur { get; set; } = null!;
    public User Destinataire { get; set; } = null!;
    public Property? Property { get; set; }
}
