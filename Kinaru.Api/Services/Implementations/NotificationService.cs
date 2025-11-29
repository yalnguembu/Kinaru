using Kinaru.Api.Services.Interfaces;
using Kinaru.Database;
using Kinaru.Shared.DTOs.Notifications;
using Kinaru.Shared.Entities;
using Kinaru.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace Kinaru.Api.Services.Implementations;

public class NotificationService : INotificationService
{
    private readonly KinaruDbContext _context;

    public NotificationService(KinaruDbContext context)
    {
        _context = context;
    }

    public async Task<List<NotificationDto>> GetUserNotificationsAsync(Guid userId)
    {
        return await _context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Select(n => new NotificationDto
            {
                Id = n.Id,
                UserId = n.UserId,
                Titre = n.Titre,
                Message = n.Message,
                Type = n.Type,
                Lu = n.Lue,
                DateCreation = n.CreatedAt,
                // We map ActionUrl to ReferenceId if it's a Guid, otherwise null or we should update DTO
                // For now let's try to parse if possible, or just ignore ReferenceId if not needed by frontend yet
                // Or better, let's assume ActionUrl holds the ID string for now
                ReferenceId = ParseGuid(n.ActionUrl)
            })
            .ToListAsync();
    }

    public async Task MarkAsReadAsync(Guid notificationId)
    {
        var notification = await _context.Notifications.FindAsync(notificationId);
        if (notification != null)
        {
            notification.Lue = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteNotificationAsync(Guid notificationId)
    {
        var notification = await _context.Notifications.FindAsync(notificationId);
        if (notification != null)
        {
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
        }
    }

    public async Task CreateNotificationAsync(Guid userId, string title, string message, NotificationType type, string? actionUrl = null)
    {
        var notification = new Notification
        {
            UserId = userId,
            Titre = title,
            Message = message,
            Type = type,
            ActionUrl = actionUrl,
            Lue = false,
            CreatedAt = DateTime.UtcNow
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
    }

    private static Guid? ParseGuid(string? input)
    {
        if (Guid.TryParse(input, out var result))
            return result;
        return null;
    }
}
