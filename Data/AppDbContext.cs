using Beacon.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Beacon.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Expose the AlertPosts table
        public DbSet<AlertPost> AlertPosts { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AlertPost>(entity =>
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
        }
    }
}