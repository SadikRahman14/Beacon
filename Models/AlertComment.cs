using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beacon.Models
{
    [Table("alert_comments")]
    public class AlertComment
    {
        [Key]
        [Column("comment_id")]
        public string CommentId { get; set; } = Guid.NewGuid().ToString("N");

        [Required, Column("alert_id")]
        public string AlertId { get; set; } = default!;

        [Required, Column("user_id")]
        public string UserId { get; set; } = default!;

        [Required, StringLength(1500)]
        [Column("content")]
        public string Content { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // nav
        [ForeignKey(nameof(AlertId))] public AlertPost? Alert { get; set; }
        [ForeignKey(nameof(UserId))] public User? Author { get; set; }
    }
}
