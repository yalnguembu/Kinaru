using Kinaru.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kinaru.Database;

public class KinaruDbContext : DbContext
{
    public KinaruDbContext(DbContextOptions<KinaruDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Property> Properties { get; set; }
    public DbSet<PropertyImage> PropertyImages { get; set; }
    public DbSet<PropertyFeature> PropertyFeatures { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Favorite> Favorites { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Agent> Agents { get; set; }
    public DbSet<AgentAvailability> AgentAvailabilities { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<SearchHistory> SearchHistories { get; set; }
    public DbSet<SavedFilter> SavedFilters { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Nom).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Prenom).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Telephone).HasMaxLength(20).IsRequired();
            
            entity.HasMany(e => e.Properties)
                .WithOne(e => e.Vendeur)
                .HasForeignKey(e => e.VendeurId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasMany(e => e.SentMessages)
                .WithOne(e => e.Expediteur)
                .HasForeignKey(e => e.ExpediteurId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasMany(e => e.ReceivedMessages)
                .WithOne(e => e.Destinataire)
                .HasForeignKey(e => e.DestinataireId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Property>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Ville);
            entity.HasIndex(e => e.Quartier);
            entity.HasIndex(e => e.Prix);
            entity.HasIndex(e => e.Featured);
            
            entity.Property(e => e.Titre).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(5000).IsRequired();
            entity.Property(e => e.Prix).HasPrecision(18, 2);
            entity.Property(e => e.Devise).HasMaxLength(10);
            entity.Property(e => e.Ville).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Quartier).HasMaxLength(100).IsRequired();
            
            entity.HasMany(e => e.Images)
                .WithOne(e => e.Property)
                .HasForeignKey(e => e.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasMany(e => e.Features)
                .WithOne(e => e.Property)
                .HasForeignKey(e => e.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PropertyImage>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Url).HasMaxLength(500).IsRequired();
        });

        modelBuilder.Entity<PropertyFeature>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nom).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Icone).HasMaxLength(100);
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.DateReservation);
            entity.Property(e => e.Notes).HasMaxLength(1000);
            
            entity.HasOne(e => e.Property)
                .WithMany(e => e.Reservations)
                .HasForeignKey(e => e.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.User)
                .WithMany(e => e.Reservations)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Favorite>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.PropertyId }).IsUnique();
            
            entity.HasOne(e => e.User)
                .WithMany(e => e.Favorites)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.Property)
                .WithMany(e => e.Favorites)
                .HasForeignKey(e => e.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.CreatedAt);
            entity.Property(e => e.Contenu).HasMaxLength(2000).IsRequired();
        });

        modelBuilder.Entity<Agent>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.UserId).IsUnique();
            entity.Property(e => e.NomComplet).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Agence).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Specialite).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Bio).HasMaxLength(1000);
            
            entity.HasOne(e => e.User)
                .WithOne(e => e.AgentProfile)
                .HasForeignKey<Agent>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AgentAvailability>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.AgentId, e.Date });
            
            entity.HasOne(e => e.Agent)
                .WithMany(e => e.Availabilities)
                .HasForeignKey(e => e.AgentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.Lue });
            entity.Property(e => e.Titre).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Message).HasMaxLength(500).IsRequired();
            entity.Property(e => e.ActionUrl).HasMaxLength(500);
            
            entity.HasOne(e => e.User)
                .WithMany(e => e.Notifications)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SearchHistory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.UserId);
            entity.Property(e => e.TermeRecherche).HasMaxLength(200).IsRequired();
            
            entity.HasOne(e => e.User)
                .WithMany(e => e.SearchHistories)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SavedFilter>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nom).HasMaxLength(100).IsRequired();
            entity.Property(e => e.TypeBien).HasMaxLength(100);
            entity.Property(e => e.Localisation).HasMaxLength(200);
            entity.Property(e => e.Equipements).HasMaxLength(500);
            
            if (entity.Property(e => e.PrixMin).Metadata.ClrType == typeof(decimal?))
            {
                entity.Property(e => e.PrixMin).HasPrecision(18, 2);
            }
            if (entity.Property(e => e.PrixMax).Metadata.ClrType == typeof(decimal?))
            {
                entity.Property(e => e.PrixMax).HasPrecision(18, 2);
            }
            
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (BaseEntity)entry.Entity;
            
            if (entry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.UtcNow;
                entity.Id = Guid.NewGuid();
            }
            
            entity.UpdatedAt = DateTime.UtcNow;
        }
    }
}
