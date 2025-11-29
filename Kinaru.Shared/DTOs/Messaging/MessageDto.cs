using Kinaru.Shared.DTOs.Users;

namespace Kinaru.Shared.DTOs.Messaging;

public class MessageDto
{
    public Guid Id { get; set; }
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public Guid? PropertyId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }
    public bool IsRead { get; set; }
    
    public UserBasicDto Sender { get; set; } = null!;
    public UserBasicDto Receiver { get; set; } = null!;
}
