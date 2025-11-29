using Kinaru.Api.Services.Interfaces;
using Kinaru.Database;
using Kinaru.Shared.DTOs.Messaging;
using Kinaru.Shared.DTOs.Users;
using Kinaru.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kinaru.Api.Services.Implementations;

public class MessagingService : IMessagingService
{
    private readonly KinaruDbContext _context;

    public MessagingService(KinaruDbContext context)
    {
        _context = context;
    }

    public async Task<MessageDto> SendMessageAsync(Guid senderId, CreateMessageDto dto)
    {
        if (senderId == dto.ReceiverId)
            throw new Exception("You cannot send a message to yourself");

        var receiver = await _context.Users.FindAsync(dto.ReceiverId);
        if (receiver == null)
            throw new Exception("Receiver not found");

        var message = new Message
        {
            ExpediteurId = senderId,
            DestinataireId = dto.ReceiverId,
            PropertyId = dto.PropertyId,
            Contenu = dto.Content,
            Lu = false,
            CreatedAt = DateTime.UtcNow
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        return await GetMessageByIdAsync(message.Id)
            ?? throw new Exception("Failed to retrieve sent message");
    }

    public async Task<List<ConversationDto>> GetConversationsAsync(Guid userId)
    {
        // Get all messages where user is sender or receiver
        var messages = await _context.Messages
            .Include(m => m.Expediteur)
            .Include(m => m.Destinataire)
            .Where(m => m.ExpediteurId == userId || m.DestinataireId == userId)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();

        // Group by the other user
        var conversations = messages
            .GroupBy(m => m.ExpediteurId == userId ? m.DestinataireId : m.ExpediteurId)
            .Select(g =>
            {
                var otherUserId = g.Key;
                var lastMessage = g.First();
                var otherUser = lastMessage.ExpediteurId == userId ? lastMessage.Destinataire : lastMessage.Expediteur;
                var unreadCount = g.Count(m => m.DestinataireId == userId && !m.Lu);

                return new ConversationDto
                {
                    OtherUserId = otherUserId,
                    OtherUser = new UserBasicDto
                    {
                        Id = otherUser.Id,
                        Nom = otherUser.Nom,
                        Prenom = otherUser.Prenom,
                        Email = otherUser.Email,
                        Telephone = otherUser.Telephone,
                        PhotoProfil = otherUser.PhotoProfil,
                        Type = otherUser.Type
                    },
                    LastMessage = MapToDto(lastMessage),
                    UnreadCount = unreadCount
                };
            })
            .ToList();

        return conversations;
    }

    public async Task<List<MessageDto>> GetMessagesAsync(Guid userId, Guid otherUserId)
    {
        return await _context.Messages
            .Include(m => m.Expediteur)
            .Include(m => m.Destinataire)
            .Where(m => (m.ExpediteurId == userId && m.DestinataireId == otherUserId) ||
                        (m.ExpediteurId == otherUserId && m.DestinataireId == userId))
            .OrderBy(m => m.CreatedAt)
            .Select(m => MapToDto(m))
            .ToListAsync();
    }

    public async Task MarkAsReadAsync(Guid userId, Guid otherUserId)
    {
        var unreadMessages = await _context.Messages
            .Where(m => m.DestinataireId == userId && m.ExpediteurId == otherUserId && !m.Lu)
            .ToListAsync();

        if (unreadMessages.Any())
        {
            foreach (var message in unreadMessages)
            {
                message.Lu = true;
            }
            await _context.SaveChangesAsync();
        }
    }

    private async Task<MessageDto?> GetMessageByIdAsync(Guid id)
    {
        var message = await _context.Messages
            .Include(m => m.Expediteur)
            .Include(m => m.Destinataire)
            .FirstOrDefaultAsync(m => m.Id == id);

        return message == null ? null : MapToDto(message);
    }

    private static MessageDto MapToDto(Message m)
    {
        return new MessageDto
        {
            Id = m.Id,
            SenderId = m.ExpediteurId,
            ReceiverId = m.DestinataireId,
            PropertyId = m.PropertyId,
            Content = m.Contenu,
            SentAt = m.CreatedAt,
            IsRead = m.Lu,
            Sender = new UserBasicDto
            {
                Id = m.Expediteur.Id,
                Nom = m.Expediteur.Nom,
                Prenom = m.Expediteur.Prenom,
                Email = m.Expediteur.Email,
                Telephone = m.Expediteur.Telephone,
                PhotoProfil = m.Expediteur.PhotoProfil,
                Type = m.Expediteur.Type
            },
            Receiver = new UserBasicDto
            {
                Id = m.Destinataire.Id,
                Nom = m.Destinataire.Nom,
                Prenom = m.Destinataire.Prenom,
                Email = m.Destinataire.Email,
                Telephone = m.Destinataire.Telephone,
                PhotoProfil = m.Destinataire.PhotoProfil,
                Type = m.Destinataire.Type
            }
        };
    }
}
