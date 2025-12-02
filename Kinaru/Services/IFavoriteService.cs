using Kinaru.Shared.DTOs.Favorites;
using Refit;

namespace Kinaru.Services;

public interface IFavoriteService
{
    [Get("/api/favorites")]
    Task<List<FavoriteDto>> GetUserFavoritesAsync();

    [Post("/api/favorites/{propertyId}")]
    Task<FavoriteDto> AddFavoriteAsync(Guid propertyId);

    [Delete("/api/favorites/{propertyId}")]
    Task RemoveFavoriteAsync(Guid propertyId);

    [Get("/api/favorites/check/{propertyId}")]
    Task<bool> IsFavoriteAsync(Guid propertyId);
}
