namespace Kinaru.Shared.DTOs.Properties;

public class PropertyImageDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public int Ordre { get; set; }
    public bool IsPrincipale { get; set; }
}
