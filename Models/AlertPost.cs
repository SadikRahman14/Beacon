using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beacon.Models
{
    [Table("alertPost")]
    public class AlertPost
    {
        [Key]
        [Column("alert_id")]
        public string AlertId { get; set; } = Guid.NewGuid().ToString("N");

        [Required]
        [Column("admin_id")]
        public string AdminId { get; set; } = string.Empty;

        [ForeignKey(nameof(AdminId))]
        public User? Admin { get; set; }

        [Url]
        [Column("alert_image_url")]
        public string? AlertImageUrl { get; set; }

        [Required]
        [Column("type")]
        public string Type { get; set; } = string.Empty;

        [Required]
        [Column("content")]
        public string Content { get; set; } = string.Empty;

        [Required]
        [Column("location")]
        public string Location { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [DataType(DataType.DateTime)]
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
