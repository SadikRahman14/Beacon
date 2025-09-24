using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beacon.Models
{
    [Table("canonical_location")]
    public class CanonicalLocation
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required, StringLength(120)]
        [Column("name_en")]
        public string NameEn { get; set; } = default!;

        // keep it minimal—add more fields later if needed
    }
}
