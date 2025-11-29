namespace Kinaru.Shared.Entities;

public class SavedFilter : BaseEntity
{
    public Guid UserId { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string? TypeBien { get; set; }
    public decimal? PrixMin { get; set; }
    public decimal? PrixMax { get; set; }
    public string? Localisation { get; set; }
    public string? Equipements { get; set; }
    public int? NombreChambresMin { get; set; }
    public float? SuperficieMin { get; set; }
    
    public User User { get; set; } = null!;
}
