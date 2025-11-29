using Kinaru.Shared.DTOs.Favorites;

namespace Kinaru.Api.Services.Interfaces;

public interface IFavoriteService
{
    Task<List<FavoriteDto>> GetUserFavoritesAsync(Guid userId);
    Task<FavoriteDto> AddFavoriteAsync(Guid userId, Guid propertyId);
    Task RemoveFavoriteAsync(Guid userId, Guid propertyId);
    Task<bool> IsFavoriteAsync(Guid userId, Guid propertyId);
}
