// Models/AlertPostVote.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beacon.Models
{
    [Table("alert_post_votes")]
    public class AlertPostVote
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        [Required] public string AlertPostId { get; set; } = default!;
        [Required] public string UserId { get; set; } = default!;

        // +1 for upvote, -1 for downvote
        [Range(-1, 1)]
        public int Value { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(AlertPostId))] public AlertPost? AlertPost { get; set; }
        [ForeignKey(nameof(UserId))] public User? User { get; set; }
    }
}
