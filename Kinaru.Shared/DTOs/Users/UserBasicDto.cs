using Kinaru.Shared.Enums;

namespace Kinaru.Shared.DTOs.Users;

public class UserBasicDto
{
    public Guid Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telephone { get; set; } = string.Empty;
    public string? PhotoProfil { get; set; }
    public UserType Type { get; set; }
}
