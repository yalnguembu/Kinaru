using Kinaru.Api.Services.Interfaces;
using Kinaru.Database;
using Kinaru.Shared.DTOs.Favorites;
using Kinaru.Shared.DTOs.Properties;
using Kinaru.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kinaru.Api.Services.Implementations;

public class FavoriteService : IFavoriteService
{
    private readonly KinaruDbContext _context;

    public FavoriteService(KinaruDbContext context)
    {
        _context = context;
    }

    public async Task<List<FavoriteDto>> GetUserFavoritesAsync(Guid userId)
    {
        return await _context.Favorites
            .Include(f => f.Property)
            .ThenInclude(p => p.Images)
            .Where(f => f.UserId == userId)
            .OrderByDescending(f => f.CreatedAt)
            .Select(f => new FavoriteDto
            {
                Id = f.Id,
                PropertyId = f.PropertyId,
                DateAjout = f.CreatedAt,
                Property = new PropertyListDto
                {
                    Id = f.Property.Id,
                    Titre = f.Property.Titre,
                    Prix = f.Property.Prix,
                    Ville = f.Property.Ville,
                    Quartier = f.Property.Quartier,
                    Superficie = f.Property.Superficie,
                    NombreChambres = f.Property.NombreChambres,
                    Type = f.Property.Type,
                    Statut = f.Property.Statut,
                    ImagePrincipale = f.Property.Images.Where(i => i.IsPrincipale).Select(i => i.Url).FirstOrDefault(),
                    Featured = f.Property.Featured,
                    DatePublication = f.Property.DatePublication
                }
            })
            .ToListAsync();
    }

    public async Task<FavoriteDto> AddFavoriteAsync(Guid userId, Guid propertyId)
    {
        var existingFavorite = await _context.Favorites
            .FirstOrDefaultAsync(f => f.UserId == userId && f.PropertyId == propertyId);

        if (existingFavorite != null)
            throw new Exception("Property already in favorites");

        var favorite = new Favorite
        {
            UserId = userId,
            PropertyId = propertyId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Favorites.Add(favorite);
        await _context.SaveChangesAsync();

        // Reload to get property details
        return await _context.Favorites
            .Include(f => f.Property)
            .ThenInclude(p => p.Images)
            .Where(f => f.Id == favorite.Id)
            .Select(f => new FavoriteDto
            {
                Id = f.Id,
                PropertyId = f.PropertyId,
                DateAjout = f.CreatedAt,
                Property = new PropertyListDto
                {
                    Id = f.Property.Id,
                    Titre = f.Property.Titre,
                    Prix = f.Property.Prix,
                    Ville = f.Property.Ville,
                    Quartier = f.Property.Quartier,
                    Superficie = f.Property.Superficie,
                    NombreChambres = f.Property.NombreChambres,
                    Type = f.Property.Type,
                    Statut = f.Property.Statut,
                    ImagePrincipale = f.Property.Images.Where(i => i.IsPrincipale).Select(i => i.Url).FirstOrDefault(),
                    Featured = f.Property.Featured,
                    DatePublication = f.Property.DatePublication
                }
            })
            .FirstAsync();
    }

    public async Task RemoveFavoriteAsync(Guid userId, Guid propertyId)
    {
        var favorite = await _context.Favorites
            .FirstOrDefaultAsync(f => f.UserId == userId && f.PropertyId == propertyId);

        if (favorite != null)
        {
            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> IsFavoriteAsync(Guid userId, Guid propertyId)
    {
        return await _context.Favorites
            .AnyAsync(f => f.UserId == userId && f.PropertyId == propertyId);
    }
}
