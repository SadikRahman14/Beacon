using System.ComponentModel.DataAnnotations;

namespace Beacon.Models
{
    public class DevUpdateVote
    {
        [Key] public string Id { get; set; } = Guid.NewGuid().ToString("N");

        [Required] public string DevUpdateId { get; set; } = default!;
        public DevUpdate? DevUpdate { get; set; }          // <— single nav

        [Required] public string UserId { get; set; } = default!;
        public User? User { get; set; }                    // <— single nav

        [Range(-1, 1)] public int Value { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}