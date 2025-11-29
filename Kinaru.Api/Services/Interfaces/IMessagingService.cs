using Kinaru.Shared.DTOs.Messaging;

namespace Kinaru.Api.Services.Interfaces;

public interface IMessagingService
{
    Task<MessageDto> SendMessageAsync(Guid senderId, CreateMessageDto dto);
    Task<List<ConversationDto>> GetConversationsAsync(Guid userId);
    Task<List<MessageDto>> GetMessagesAsync(Guid userId, Guid otherUserId);
    Task MarkAsReadAsync(Guid userId, Guid otherUserId);
}
