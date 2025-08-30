using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Beacon.Models
{
    public class User : IdentityUser
    {

        [StringLength(100)]
        public string? FirstName { get; set; }


        [StringLength(100)]
        public string? LastName { get; set; }

        public string? ProfileImageUrl { get; set; }

        [StringLength(50)]
        public string Role { get; set; } = "citizen";

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [DataType(DataType.DateTime)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
