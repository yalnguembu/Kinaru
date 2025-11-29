using Kinaru.Api.Services.Interfaces;
using Kinaru.Database;
using Kinaru.Shared.DTOs;
using Kinaru.Shared.DTOs.Properties;
using Kinaru.Shared.DTOs.Users;
using Kinaru.Shared.Entities;
using Kinaru.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace Kinaru.Api.Services.Implementations;

public class PropertyService : IPropertyService
{
    private readonly KinaruDbContext _context;

    public PropertyService(KinaruDbContext context)
    {
        _context = context;
    }

    public async Task<PropertyDetailDto> CreatePropertyAsync(CreatePropertyDto dto, Guid vendeurId)
    {
        var property = new Property
        {
            Titre = dto.Titre,
            Description = dto.Description,
            Prix = dto.Prix,
            Type = dto.Type,
            Adresse = dto.Adresse,
            Ville = dto.Ville,
            Quartier = dto.Quartier,
            CodePostal = dto.CodePostal,
            Superficie = dto.Superficie,
            NombreChambres = dto.NombreChambres,
            NombreSallesBain = dto.NombreSallesBain,
            NombrePieces = dto.NombrePieces,
            VendeurId = vendeurId,
            Statut = PropertyStatus.AVendre,
            DatePublication = DateTime.UtcNow,
            ViewCount = 0,
            Featured = false
        };

        _context.Properties.Add(property);

        if (dto.Features.Any())
        {
            foreach (var featureName in dto.Features)
            {
                var feature = new PropertyFeature
                {
                    PropertyId = property.Id,
                    Nom = featureName
                };
                _context.PropertyFeatures.Add(feature);
            }
        }

        await _context.SaveChangesAsync();

        return await GetPropertyByIdAsync(property.Id) 
            ?? throw new Exception("Failed to retrieve created property");
    }

    public async Task<PropertyDetailDto?> GetPropertyByIdAsync(Guid id)
    {
        var property = await _context.Properties
            .Include(p => p.Vendeur)
            .Include(p => p.Images)
            .Include(p => p.Features)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (property == null) return null;

        return new PropertyDetailDto
        {
            Id = property.Id,
            Titre = property.Titre,
            Description = property.Description,
            Prix = property.Prix,
            Ville = property.Ville,
            Quartier = property.Quartier,
            Adresse = property.Adresse,
            Superficie = property.Superficie,
            NombreChambres = property.NombreChambres,
            NombreSallesBain = property.NombreSallesBain,
            NombrePieces = property.NombrePieces,
            Type = property.Type,
            Statut = property.Statut,
            ImagePrincipale = property.Images.Where(i => i.IsPrincipale).Select(i => i.Url).FirstOrDefault(),
            Featured = property.Featured,
            DatePublication = property.DatePublication,
            NombreVues = property.ViewCount,
            Images = property.Images.OrderBy(i => i.Ordre).Select(i => new PropertyImageDto
            {
                Id = i.Id,
                Url = i.Url,
                Ordre = i.Ordre,
                IsPrincipale = i.IsPrincipale
            }).ToList(),
            Features = property.Features.Select(f => new PropertyFeatureDto
            {
                Nom = f.Nom,
                Icone = f.Icone
            }).ToList(),
            Vendeur = new UserBasicDto
            {
                Id = property.Vendeur.Id,
                Nom = property.Vendeur.Nom,
                Prenom = property.Vendeur.Prenom,
                Email = property.Vendeur.Email,
                Telephone = property.Vendeur.Telephone,
                PhotoProfil = property.Vendeur.PhotoProfil,
                Type = property.Vendeur.Type
            }
        };
    }

    public async Task<PagedResult<PropertyListDto>> GetPropertiesAsync(PropertyFilterDto filter)
    {
        var query = _context.Properties
            .Include(p => p.Images)
            .AsQueryable();

        if (filter.Type.HasValue)
            query = query.Where(p => p.Type == filter.Type.Value);

        if (filter.PrixMin.HasValue)
            query = query.Where(p => p.Prix >= filter.PrixMin.Value);

        if (filter.PrixMax.HasValue)
            query = query.Where(p => p.Prix <= filter.PrixMax.Value);

        if (!string.IsNullOrWhiteSpace(filter.Ville))
            query = query.Where(p => p.Ville.ToLower().Contains(filter.Ville.ToLower()));

        if (!string.IsNullOrWhiteSpace(filter.Quartier))
            query = query.Where(p => p.Quartier.ToLower().Contains(filter.Quartier.ToLower()));

        if (filter.NombreChambresMin.HasValue)
            query = query.Where(p => p.NombreChambres >= filter.NombreChambresMin.Value);

        if (filter.SuperficieMin.HasValue)
            query = query.Where(p => p.Superficie >= filter.SuperficieMin.Value);

        if (filter.FeaturedOnly == true)
            query = query.Where(p => p.Featured);

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchLower = filter.SearchTerm.ToLower();
            query = query.Where(p => 
                p.Titre.ToLower().Contains(searchLower) ||
                p.Description.ToLower().Contains(searchLower) ||
                p.Ville.ToLower().Contains(searchLower) ||
                p.Quartier.ToLower().Contains(searchLower));
        }

        query = query.Where(p => p.Statut == PropertyStatus.AVendre);

        var totalCount = await query.CountAsync();

        var properties = await query
            .OrderByDescending(p => p.Featured)
            .ThenByDescending(p => p.DatePublication)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(p => new PropertyListDto
            {
                Id = p.Id,
                Titre = p.Titre,
                Prix = p.Prix,
                Ville = p.Ville,
                Quartier = p.Quartier,
                Superficie = p.Superficie,
                NombreChambres = p.NombreChambres,
                Type = p.Type,
                Statut = p.Statut,
                ImagePrincipale = p.Images.Where(i => i.IsPrincipale).Select(i => i.Url).FirstOrDefault(),
                Featured = p.Featured,
                DatePublication = p.DatePublication
            })
            .ToListAsync();

        return new PagedResult<PropertyListDto>
        {
            Items = properties,
            TotalCount = totalCount,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }

    public async Task<PropertyDetailDto> UpdatePropertyAsync(Guid id, UpdatePropertyDto dto, Guid userId)
    {
        var property = await _context.Properties
            .FirstOrDefaultAsync(p => p.Id == id);

        if (property == null)
            throw new Exception("Property not found");

        if (property.VendeurId != userId)
            throw new UnauthorizedAccessException("You can only update your own properties");

        if (dto.Titre != null) property.Titre = dto.Titre;
        if (dto.Description != null) property.Description = dto.Description;
        if (dto.Prix.HasValue) property.Prix = dto.Prix.Value;
        if (dto.Statut.HasValue) property.Statut = dto.Statut.Value;
        if (dto.Adresse != null) property.Adresse = dto.Adresse;
        if (dto.Ville != null) property.Ville = dto.Ville;
        if (dto.Quartier != null) property.Quartier = dto.Quartier;
        if (dto.CodePostal != null) property.CodePostal = dto.CodePostal;
        if (dto.Superficie.HasValue) property.Superficie = dto.Superficie.Value;
        if (dto.NombreChambres.HasValue) property.NombreChambres = dto.NombreChambres.Value;
        if (dto.NombreSallesBain.HasValue) property.NombreSallesBain = dto.NombreSallesBain.Value;
        if (dto.NombrePieces.HasValue) property.NombrePieces = dto.NombrePieces.Value;
        if (dto.Featured.HasValue) property.Featured = dto.Featured.Value;

        await _context.SaveChangesAsync();

        return await GetPropertyByIdAsync(id) 
            ?? throw new Exception("Failed to retrieve updated property");
    }

    public async Task DeletePropertyAsync(Guid id, Guid userId)
    {
        var property = await _context.Properties
            .FirstOrDefaultAsync(p => p.Id == id);

        if (property == null)
            throw new Exception("Property not found");

        if (property.VendeurId != userId)
            throw new UnauthorizedAccessException("You can only delete your own properties");

        _context.Properties.Remove(property);
        await _context.SaveChangesAsync();
    }

    public async Task<List<PropertyListDto>> GetFeaturedPropertiesAsync(int count = 10)
    {
        return await _context.Properties
            .Include(p => p.Images)
            .Where(p => p.Featured && p.Statut == PropertyStatus.AVendre)
            .OrderByDescending(p => p.DatePublication)
            .Take(count)
            .Select(p => new PropertyListDto
            {
                Id = p.Id,
                Titre = p.Titre,
                Prix = p.Prix,
                Ville = p.Ville,
                Quartier = p.Quartier,
                Superficie = p.Superficie,
                NombreChambres = p.NombreChambres,
                Type = p.Type,
                Statut = p.Statut,
                ImagePrincipale = p.Images.Where(i => i.IsPrincipale).Select(i => i.Url).FirstOrDefault(),
                Featured = p.Featured,
                DatePublication = p.DatePublication
            })
            .ToListAsync();
    }

    public async Task<List<PropertyListDto>> GetUserPropertiesAsync(Guid userId)
    {
        return await _context.Properties
            .Include(p => p.Images)
            .Where(p => p.VendeurId == userId)
            .OrderByDescending(p => p.DatePublication)
            .Select(p => new PropertyListDto
            {
                Id = p.Id,
                Titre = p.Titre,
                Prix = p.Prix,
                Ville = p.Ville,
                Quartier = p.Quartier,
                Superficie = p.Superficie,
                NombreChambres = p.NombreChambres,
                Type = p.Type,
                Statut = p.Statut,
                ImagePrincipale = p.Images.Where(i => i.IsPrincipale).Select(i => i.Url).FirstOrDefault(),
                Featured = p.Featured,
                DatePublication = p.DatePublication
            })
            .ToListAsync();
    }

    public async Task IncrementViewCountAsync(Guid propertyId)
    {
        var property = await _context.Properties.FindAsync(propertyId);
        if (property != null)
        {
            property.ViewCount++;
            await _context.SaveChangesAsync();
        }
    }
}
