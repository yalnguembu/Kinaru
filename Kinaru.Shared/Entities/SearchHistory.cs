namespace Kinaru.Shared.Entities;

public class SearchHistory : BaseEntity
{
    public Guid UserId { get; set; }
    public string TermeRecherche { get; set; } = string.Empty;
    
    public User User { get; set; } = null!;
}
