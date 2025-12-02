using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Kinaru.Api.Services.Interfaces;
using Kinaru.Database;
using Kinaru.Shared.DTOs.Auth;
using Kinaru.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Kinaru.Api.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly KinaruDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(KinaruDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
        {
            throw new Exception("Email already exists");
        }

        var user = new User
        {
            Nom = request.Nom,
            Prenom = request.Prenom,
            Email = request.Email,
            Telephone = request.Telephone,
            LieuHabitation = request.LieuHabitation,
            Type = request.Type,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return GenerateAuthResponse(user);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new Exception("Invalid email or password");
        }

        return GenerateAuthResponse(user);
    }

    private AuthResponseDto GenerateAuthResponse(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is missing"));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Type.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new AuthResponseDto
        {
            UserId = user.Id,
            Email = user.Email,
            Nom = user.Nom,
            Prenom = user.Prenom,
            Type = user.Type,
            Token = tokenHandler.WriteToken(token),
            Expiration = tokenDescriptor.Expires.Value
        };
    }
}
