using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Beacon.ViewModels
{
    public enum AlertType
    {
        Emergency,
        Safety,
        Event,
        Announcement,
        Weather
    }

    public class AlertPostCreateViewModel
    {
        [Required]
        public AlertType Type { get; set; }

        [Required, StringLength(2000)]
        public string Content { get; set; } = string.Empty;

        [Required, StringLength(200)]
        public string Location { get; set; } = string.Empty;

        [Display(Name = "Alert Image")]
        public IFormFile? AlertImage { get; set; }
    }
}
