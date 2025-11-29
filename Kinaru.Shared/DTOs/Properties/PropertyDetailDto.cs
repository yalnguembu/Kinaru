using Kinaru.Shared.DTOs.Users;

namespace Kinaru.Shared.DTOs.Properties;

public class PropertyDetailDto : PropertyListDto
{
    public string Description { get; set; } = string.Empty;
    public string Adresse { get; set; } = string.Empty;
    public int NombreSallesBain { get; set; }
    public int NombrePieces { get; set; }
    public int NombreVues { get; set; }
    public List<PropertyImageDto> Images { get; set; } = new();
    public List<PropertyFeatureDto> Features { get; set; } = new();
    public UserBasicDto Vendeur { get; set; } = null!;
}
