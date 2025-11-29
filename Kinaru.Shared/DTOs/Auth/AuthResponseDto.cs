using Kinaru.Shared.Enums;

namespace Kinaru.Shared.DTOs.Auth;

public class AuthResponseDto
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public UserType Type { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
}
