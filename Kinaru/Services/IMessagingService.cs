using Kinaru.Shared.DTOs.Messaging;
using Refit;

namespace Kinaru.Services;

public interface IMessagingService
{
    [Post("/api/messaging")]
    Task<MessageDto> SendMessageAsync([Body] CreateMessageDto message);

    [Get("/api/messaging/conversations")]
    Task<List<ConversationDto>> GetConversationsAsync();

    [Get("/api/messaging/{otherUserId}")]
    Task<List<MessageDto>> GetMessagesAsync(Guid otherUserId);

    [Put("/api/messaging/{otherUserId}/read")]
    Task MarkAsReadAsync(Guid otherUserId);
}
