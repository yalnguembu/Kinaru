using Kinaru.Api.Services.Interfaces;
using Kinaru.Database;
using Kinaru.Shared.DTOs.Properties;
using Kinaru.Shared.DTOs.Reservations;
using Kinaru.Shared.DTOs.Users;
using Kinaru.Shared.Entities;
using Kinaru.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace Kinaru.Api.Services.Implementations;

public class ReservationService : IReservationService
{
    private readonly KinaruDbContext _context;

    public ReservationService(KinaruDbContext context)
    {
        _context = context;
    }

    public async Task<ReservationDto> CreateReservationAsync(Guid userId, CreateReservationDto dto)
    {
        var property = await _context.Properties.FindAsync(dto.PropertyId);
        if (property == null)
            throw new Exception("Property not found");

        if (property.VendeurId == userId)
            throw new Exception("You cannot reserve your own property");

        var reservation = new Reservation
        {
            UserId = userId,
            PropertyId = dto.PropertyId,
            DateReservation = dto.DateReservation,
            HeureDebut = dto.HeureDebut,
            HeureFin = dto.HeureFin,
            Statut = ReservationStatus.EnAttente,
            Notes = dto.Message,
            CreatedAt = DateTime.UtcNow
        };

        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();

        return await GetReservationByIdAsync(reservation.Id) 
            ?? throw new Exception("Failed to retrieve created reservation");
    }

    public async Task<List<ReservationDto>> GetUserReservationsAsync(Guid userId)
    {
        return await _context.Reservations
            .Include(r => r.Property)
            .ThenInclude(p => p.Images)
            .Include(r => r.User)
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.DateReservation)
            .Select(r => MapToDto(r))
            .ToListAsync();
    }

    public async Task<List<ReservationDto>> GetPropertyReservationsAsync(Guid propertyId, Guid ownerId)
    {
        var property = await _context.Properties.FindAsync(propertyId);
        if (property == null)
            throw new Exception("Property not found");

        if (property.VendeurId != ownerId)
            throw new UnauthorizedAccessException("You can only view reservations for your own properties");

        return await _context.Reservations
            .Include(r => r.Property)
            .ThenInclude(p => p.Images)
            .Include(r => r.User)
            .Where(r => r.PropertyId == propertyId)
            .OrderByDescending(r => r.DateReservation)
            .Select(r => MapToDto(r))
            .ToListAsync();
    }

    public async Task<ReservationDto> UpdateReservationStatusAsync(Guid reservationId, UpdateReservationStatusDto dto, Guid userId)
    {
        var reservation = await _context.Reservations
            .Include(r => r.Property)
            .FirstOrDefaultAsync(r => r.Id == reservationId);

        if (reservation == null)
            throw new Exception("Reservation not found");

        // Only the property owner can update the status
        if (reservation.Property.VendeurId != userId)
            throw new UnauthorizedAccessException("Only the property owner can update reservation status");

        reservation.Statut = dto.Statut;
        await _context.SaveChangesAsync();

        return await GetReservationByIdAsync(reservationId)
            ?? throw new Exception("Failed to retrieve updated reservation");
    }

    private async Task<ReservationDto?> GetReservationByIdAsync(Guid id)
    {
        var reservation = await _context.Reservations
            .Include(r => r.Property)
            .ThenInclude(p => p.Images)
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == id);

        return reservation == null ? null : MapToDto(reservation);
    }

    private static ReservationDto MapToDto(Reservation r)
    {
        return new ReservationDto
        {
            Id = r.Id,
            PropertyId = r.PropertyId,
            UserId = r.UserId,
            DateReservation = r.DateReservation,
            HeureDebut = r.HeureDebut,
            HeureFin = r.HeureFin,
            Statut = r.Statut,
            Message = r.Notes,
            Property = new PropertyListDto
            {
                Id = r.Property.Id,
                Titre = r.Property.Titre,
                Prix = r.Property.Prix,
                Ville = r.Property.Ville,
                Quartier = r.Property.Quartier,
                Superficie = r.Property.Superficie,
                NombreChambres = r.Property.NombreChambres,
                Type = r.Property.Type,
                Statut = r.Property.Statut,
                ImagePrincipale = r.Property.Images.Where(i => i.IsPrincipale).Select(i => i.Url).FirstOrDefault(),
                Featured = r.Property.Featured,
                DatePublication = r.Property.DatePublication
            },
            User = new UserBasicDto
            {
                Id = r.User.Id,
                Nom = r.User.Nom,
                Prenom = r.User.Prenom,
                Email = r.User.Email,
                Telephone = r.User.Telephone,
                PhotoProfil = r.User.PhotoProfil,
                Type = r.User.Type
            }
        };
    }
}
