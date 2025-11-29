using Kinaru.Shared.Enums;

namespace Kinaru.Shared.DTOs.Properties;

public class CreatePropertyDto
{
    public string Titre { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Prix { get; set; }
    public PropertyType Type { get; set; }
    public string Adresse { get; set; } = string.Empty;
    public string Ville { get; set; } = string.Empty;
    public string Quartier { get; set; } = string.Empty;
    public string? CodePostal { get; set; }
    public float Superficie { get; set; }
    public int NombreChambres { get; set; }
    public int NombreSallesBain { get; set; }
    public int NombrePieces { get; set; }
    public List<string> Features { get; set; } = new();
}
