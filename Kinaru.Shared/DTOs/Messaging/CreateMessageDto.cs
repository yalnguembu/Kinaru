namespace Kinaru.Shared.DTOs.Messaging;

public class CreateMessageDto
{
    public Guid ReceiverId { get; set; }
    public Guid? PropertyId { get; set; }
    public string Content { get; set; } = string.Empty;
}
