using Kinaru.Shared.Enums;

namespace Kinaru.Shared.Entities;

public class Property : BaseEntity
{
    public string Titre { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Prix { get; set; }
    public string Devise { get; set; } = "XAF";
    public PropertyType Type { get; set; }
    public PropertyStatus Statut { get; set; } = PropertyStatus.Disponible;
    
    public string Adresse { get; set; } = string.Empty;
    public string Ville { get; set; } = string.Empty;
    public string Quartier { get; set; } = string.Empty;
    public string? CodePostal { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    
    public float Superficie { get; set; }
    public int NombreChambres { get; set; }
    public int NombreSallesBain { get; set; }
    public int NombrePieces { get; set; }
    
    public bool Featured { get; set; } = false;
    public int ViewCount { get; set; } = 0;
    public DateTime DatePublication { get; set; }
    
    public Guid VendeurId { get; set; }
    
    public User Vendeur { get; set; } = null!;
    public ICollection<PropertyImage> Images { get; set; } = new List<PropertyImage>();
    public ICollection<PropertyFeature> Features { get; set; } = new List<PropertyFeature>();
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    public ICollection<Message> Messages { get; set; } = new List<Message>();
}
