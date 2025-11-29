using Kinaru.Shared.DTOs.Users;

namespace Kinaru.Shared.DTOs.Messaging;

public class ConversationDto
{
    public Guid OtherUserId { get; set; }
    public UserBasicDto OtherUser { get; set; } = null!;
    public MessageDto LastMessage { get; set; } = null!;
    public int UnreadCount { get; set; }
}
