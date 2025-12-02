using Kinaru.Api.Services.Interfaces;
using Kinaru.Database;
using Kinaru.Shared.DTOs.Users;
using Microsoft.EntityFrameworkCore;

namespace Kinaru.Api.Services.Implementations;

public class UserService : IUserService
{
    private readonly KinaruDbContext _context;

    public UserService(KinaruDbContext context)
    {
        _context = context;
    }

    public async Task<UserProfileDto?> GetUserProfileAsync(Guid userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return null;

        return new UserProfileDto
        {
            Id = user.Id,
            Nom = user.Nom,
            Prenom = user.Prenom,
            Email = user.Email,
            Telephone = user.Telephone,
            LieuHabitation = user.LieuHabitation,
            PhotoProfil = user.PhotoProfil,
            Type = user.Type,
            DateInscription = user.CreatedAt
        };
    }

    public async Task<UserProfileDto> UpdateProfileAsync(Guid userId, UpdateUserDto dto)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) throw new Exception("User not found");

        if (dto.Nom != null) user.Nom = dto.Nom;
        if (dto.Prenom != null) user.Prenom = dto.Prenom;
        if (dto.Telephone != null) user.Telephone = dto.Telephone;
        if (dto.LieuHabitation != null) user.LieuHabitation = dto.LieuHabitation;
        if (dto.PhotoProfil != null) user.PhotoProfil = dto.PhotoProfil;

        await _context.SaveChangesAsync();

        return new UserProfileDto
        {
            Id = user.Id,
            Nom = user.Nom,
            Prenom = user.Prenom,
            Email = user.Email,
            Telephone = user.Telephone,
            LieuHabitation = user.LieuHabitation,
            PhotoProfil = user.PhotoProfil,
            Type = user.Type,
            DateInscription = user.CreatedAt
        };
    }

    public async Task ChangePasswordAsync(Guid userId, ChangePasswordDto dto)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) throw new Exception("User not found");

        if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash))
        {
            throw new Exception("Invalid current password");
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        await _context.SaveChangesAsync();
    }
}
