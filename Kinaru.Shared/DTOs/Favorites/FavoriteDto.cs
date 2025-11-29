using Kinaru.Shared.DTOs.Properties;

namespace Kinaru.Shared.DTOs.Favorites;

public class FavoriteDto
{
    public Guid Id { get; set; }
    public Guid PropertyId { get; set; }
    public PropertyListDto Property { get; set; } = null!;
    public DateTime DateAjout { get; set; }
}
