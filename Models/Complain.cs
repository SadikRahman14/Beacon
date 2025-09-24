using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beacon.Models
{
    public class Complain
    {
        [Key]
        public string ComplaintId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public string ComplaintImageUrl { get; set; }

        [Required]
        [StringLength(100)]
        public string Type { get; set; }

        [Required]
        [StringLength(2000)]
        public string Content { get; set; }

        [Required]
        [StringLength(500)]
        public string Location { get; set; }

        public int VoteCount { get; set; } = 0;
        public bool Resolved { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Coordinates (optional)
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        // For file uploads in the view (not stored in DB)
        [NotMapped]
        public List<IFormFile> ImageFiles { get; set; } = new List<IFormFile>();
    }
}
