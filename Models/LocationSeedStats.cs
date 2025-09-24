using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beacon.Models
{
    [Table("location_seed_stats")]
    public class LocationSeedStats
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("location_id")]
        public int CanonicalLocationId { get; set; }

        [ForeignKey(nameof(CanonicalLocationId))]
        public CanonicalLocation Location { get; set; } = default!;

        [Column("alerts")]
        public int Alerts { get; set; }

        [Column("complaints")]
        public int Complaints { get; set; }

        [Column("dev_updates")]
        public int DevUpdates { get; set; }
    }
}
