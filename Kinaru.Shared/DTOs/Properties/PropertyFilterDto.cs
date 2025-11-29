using Kinaru.Shared.Enums;

namespace Kinaru.Shared.DTOs.Properties;

public class PropertyFilterDto
{
    public PropertyType? Type { get; set; }
    public decimal? PrixMin { get; set; }
    public decimal? PrixMax { get; set; }
    public string? Ville { get; set; }
    public string? Quartier { get; set; }
    public int? NombreChambresMin { get; set; }
    public float? SuperficieMin { get; set; }
    public bool? FeaturedOnly { get; set; }
    public string? SearchTerm { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
