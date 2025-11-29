using Kinaru.Shared.DTOs;
using Kinaru.Shared.DTOs.Properties;

namespace Kinaru.Api.Services.Interfaces;

public interface IPropertyService
{
    Task<PropertyDetailDto> CreatePropertyAsync(CreatePropertyDto dto, Guid vendeurId);
    Task<PropertyDetailDto?> GetPropertyByIdAsync(Guid id);
    Task<PagedResult<PropertyListDto>> GetPropertiesAsync(PropertyFilterDto filter);
    Task<PropertyDetailDto> UpdatePropertyAsync(Guid id, UpdatePropertyDto dto, Guid userId);
    Task DeletePropertyAsync(Guid id, Guid userId);
    Task<List<PropertyListDto>> GetFeaturedPropertiesAsync(int count = 10);
    Task<List<PropertyListDto>> GetUserPropertiesAsync(Guid userId);
    Task IncrementViewCountAsync(Guid propertyId);
}
