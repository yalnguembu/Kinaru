using Kinaru.Shared.Enums;

namespace Kinaru.Shared.DTOs.Auth;

public class RegisterRequestDto
{
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Telephone { get; set; } = string.Empty;
    public UserType Type { get; set; }
}
