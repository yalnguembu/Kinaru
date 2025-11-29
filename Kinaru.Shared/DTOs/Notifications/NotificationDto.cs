using Kinaru.Shared.Enums;

namespace Kinaru.Shared.DTOs.Notifications;

public class NotificationDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Titre { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public Guid? ReferenceId { get; set; }
    public bool Lu { get; set; }
    public DateTime DateCreation { get; set; }
}
