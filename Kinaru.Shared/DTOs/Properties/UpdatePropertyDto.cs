using Kinaru.Shared.Enums;

namespace Kinaru.Shared.DTOs.Properties;

public class UpdatePropertyDto
{
    public string? Titre { get; set; }
    public string? Description { get; set; }
    public decimal? Prix { get; set; }
    public PropertyStatus? Statut { get; set; }
    public string? Adresse { get; set; }
    public string? Ville { get; set; }
    public string? Quartier { get; set; }
    public string? CodePostal { get; set; }
    public float? Superficie { get; set; }
    public int? NombreChambres { get; set; }
    public int? NombreSallesBain { get; set; }
    public int? NombrePieces { get; set; }
    public bool? Featured { get; set; }
}
