using Kinaru.Shared.Enums;

namespace Kinaru.Shared.DTOs.Properties;

public class PropertyListDto
{
    public Guid Id { get; set; }
    public string Titre { get; set; } = string.Empty;
    public decimal Prix { get; set; }
    public string Ville { get; set; } = string.Empty;
    public string Quartier { get; set; } = string.Empty;
    public float Superficie { get; set; }
    public int NombreChambres { get; set; }
    public PropertyType Type { get; set; }
    public PropertyStatus Statut { get; set; }
    public string? ImagePrincipale { get; set; }
    public bool Featured { get; set; }
    public DateTime DatePublication { get; set; }
}
