using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Beacon.ViewModels
{
    public class AlertPostEditViewModel
    {
        [Required]
        public string AlertId { get; set; } = string.Empty;

        [Required]
        public AlertType Type { get; set; }

        [Required, StringLength(2000)]
        public string Content { get; set; } = string.Empty;

        [Required, StringLength(200)]
        public string Location { get; set; } = string.Empty;

        [Display(Name = "Replace Image")]
        public IFormFile? AlertImage { get; set; }

        [Display(Name = "Remove current image")]
        public bool RemoveImage { get; set; }

        // for display only
        public string? ExistingImageUrl { get; set; }
    }
}
