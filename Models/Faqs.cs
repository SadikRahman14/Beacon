using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beacon.Models
{
    [Table("faq")]
    public class Faq
    {
        [Key]
        [Column("faq_id")]
        public string FaqId { get; set; } = Guid.NewGuid().ToString("N");

        [Required]
        [Column("question")]
        [StringLength(500)]
        public string Question { get; set; } = string.Empty;

        [Required]
        [Column("answer")]
        [StringLength(4000)]
        public string Answer { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
