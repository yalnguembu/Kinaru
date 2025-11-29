using Kinaru.Shared.Enums;

namespace Kinaru.Shared.Entities;

public class Notification : BaseEntity
{
    public Guid UserId { get; set; }
    public string Titre { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public bool Lue { get; set; } = false;
    public string? ActionUrl { get; set; }
    
    public User User { get; set; } = null!;
}
