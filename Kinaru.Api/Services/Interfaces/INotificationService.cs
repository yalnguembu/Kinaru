using Kinaru.Shared.DTOs.Notifications;
using Kinaru.Shared.Enums;

namespace Kinaru.Api.Services.Interfaces;

public interface INotificationService
{
    Task<List<NotificationDto>> GetUserNotificationsAsync(Guid userId);
    Task MarkAsReadAsync(Guid notificationId);
    Task DeleteNotificationAsync(Guid notificationId);
    Task CreateNotificationAsync(Guid userId, string title, string message, NotificationType type, string? actionUrl = null);
}
