using Beacon.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Beacon.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<AlertPost> AlertPosts { get; set; } = default!;
        public DbSet<Faq> Faqs { get; set; } = default!;
        public DbSet<CanonicalLocation> CanonicalLocations { get; set; } = default!;
        public DbSet<LocationSeedStats> LocationSeedStats { get; set; } = default!;
        public DbSet<DevUpdate> DevUpdates { get; set; } = default!;
        public DbSet<Complain> Complaints { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Complain entity
            builder.Entity<Complain>(entity =>
            {
                entity.HasKey(c => c.ComplaintId);
                entity.HasOne(c => c.User)
                      .WithMany()
                      .HasForeignKey(c => c.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // AlertPost entity
            builder.Entity<AlertPost>(entity =>
            {
                entity.HasKey(a => a.AlertId);
                entity.Property(a => a.AlertId)
                      .ValueGeneratedNever();

                entity.Property(a => a.Type)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(a => a.Location)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(a => a.AlertImageUrl)
                      .HasMaxLength(512);

                entity.Property(a => a.Content)
                      .IsRequired();

                entity.Property(a => a.CreatedAt)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(a => a.UpdatedAt)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(a => a.Admin)
                      .WithMany()
                      .HasForeignKey(a => a.AdminId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .IsRequired();
            });

            // CanonicalLocation entity with seed data
            builder.Entity<CanonicalLocation>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.NameEn)
                      .IsRequired()
                      .HasMaxLength(120);

                entity.HasIndex(x => x.NameEn).IsUnique();

                entity.HasData(
                    new CanonicalLocation { Id = 1, NameEn = "Mirpur" },
                    new CanonicalLocation { Id = 2, NameEn = "Gulshan" },
                    new CanonicalLocation { Id = 3, NameEn = "Uttara" },
                    new CanonicalLocation { Id = 4, NameEn = "Dhanmondi" },
                    new CanonicalLocation { Id = 5, NameEn = "Jatrabari" },
                    new CanonicalLocation { Id = 6, NameEn = "Badda" },
                    new CanonicalLocation { Id = 7, NameEn = "Banani" },
                    new CanonicalLocation { Id = 8, NameEn = "Mohammadpur" },
                    new CanonicalLocation { Id = 9, NameEn = "Motijheel" },
                    new CanonicalLocation { Id = 10, NameEn = "Tejgaon" }
                );
            });

            // LocationSeedStats entity
            builder.Entity<LocationSeedStats>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.CanonicalLocationId).IsUnique();
                entity.Property(x => x.Alerts).HasDefaultValue(0);
                entity.Property(x => x.Complaints).HasDefaultValue(0);
                entity.Property(x => x.DevUpdates).HasDefaultValue(0);
                entity.HasOne(x => x.Location)
                      .WithMany()
                      .HasForeignKey(x => x.CanonicalLocationId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
