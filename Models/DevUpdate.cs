using Beacon.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beacon.Models
{
    [Table("DevUpdate")]
    public class DevUpdate
    {
        [Key]
        [Column("devUpdate_id")]
        public string DevUpdateId { get; set; } = Guid.NewGuid().ToString("N");

        [Required]
        [Column("admin_id")]
        public string AdminId { get; set; } = string.Empty;

        [Column("dev_image_url")]
        [MaxLength(1024)]
        public string? ImageUrl { get; set; }

        [Required]
        [Column("type")]
        [MaxLength(100)]
        public string Type { get; set; } = string.Empty;

        [Required]
        [Column("content")]
        [MaxLength(4000)]
        public string Content { get; set; } = string.Empty;

        [Required]
        [Column("location")]
        [MaxLength(200)]
        public string Location { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [DataType(DataType.DateTime)]
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey(nameof(AdminId))]
        public User? Admin { get; set; }
    }
}