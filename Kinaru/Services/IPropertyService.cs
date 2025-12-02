using Kinaru.Shared.DTOs.Properties;
using Refit;

namespace Kinaru.Services;

public interface IPropertyService
{
    [Get("/api/properties")]
    Task<List<PropertyListDto>> GetPropertiesAsync([Query] PropertyFilterDto? filter = null);

    [Get("/api/properties/featured")]
    Task<List<PropertyListDto>> GetFeaturedPropertiesAsync([Query] int count = 6);

    [Get("/api/properties/{id}")]
    Task<PropertyDetailDto> GetPropertyByIdAsync(Guid id);

    [Get("/api/properties/search")]
    Task<List<PropertyListDto>> SearchPropertiesAsync([Query] string? query = null);

    [Get("/api/properties/user/{userId}")]
    Task<List<PropertyListDto>> GetUserPropertiesAsync(Guid userId);

    [Post("/api/properties")]
    Task<PropertyDetailDto> CreatePropertyAsync([Body] CreatePropertyDto property);

    [Put("/api/properties/{id}")]
    Task<PropertyDetailDto> UpdatePropertyAsync(Guid id, [Body] UpdatePropertyDto property);

    [Patch("/api/properties/{id}/status")]
    Task UpdatePropertyStatusAsync(Guid id, [Body] UpdatePropertyStatusDto dto);

    [Delete("/api/properties/{id}")]
    Task DeletePropertyAsync(Guid id);
}
