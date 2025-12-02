using Kinaru.Shared.Enums;

namespace Kinaru.Shared.Entities;

public class User : BaseEntity
{
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Telephone { get; set; } = string.Empty;
    public string LieuHabitation { get; set; } = string.Empty;
    public string? PhotoProfil { get; set; }
    public UserType Type { get; set; }
    public bool IsEmailVerified { get; set; }
    public bool IsActive { get; set; } = true;
    
    public ICollection<Property> Properties { get; set; } = new List<Property>();
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    public ICollection<Message> SentMessages { get; set; } = new List<Message>();
    public ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public ICollection<SearchHistory> SearchHistories { get; set; } = new List<SearchHistory>();
    public Agent? AgentProfile { get; set; }
}
