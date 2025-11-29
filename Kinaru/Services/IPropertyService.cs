using Kinaru.Shared.DTOs.Properties;
using Refit;

namespace Kinaru.Services;

public interface IPropertyService
{
    [Get("/api/properties")]
    Task<List<PropertyListDto>> GetPropertiesAsync();

    [Get("/api/properties/featured")]
    Task<List<PropertyListDto>> GetFeaturedPropertiesAsync([Query] int count = 6);

    [Get("/api/properties/{id}")]
    Task<PropertyDetailDto> GetPropertyByIdAsync(Guid id);

    [Get("/api/properties/search")]
    Task<List<PropertyListDto>> SearchPropertiesAsync([Query] string? query = null);
}
