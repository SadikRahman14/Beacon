namespace Beacon.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;

    public class DevUpdateCreateViewModel
    {
        [Required, MaxLength(100)]
        public string Type { get; set; } = string.Empty;

        [Required, MaxLength(4000), DataType(DataType.MultilineText)]
        public string Content { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string Location { get; set; } = string.Empty;

        public IFormFile? Image { get; set; }
    }

    public class DevUpdateEditViewModel
    {
        [Required]
        public string DevUpdateId { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Type { get; set; } = string.Empty;

        [Required, MaxLength(4000), DataType(DataType.MultilineText)]
        public string Content { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string Location { get; set; } = string.Empty;

        public string? ExistingImageUrl { get; set; }
        public IFormFile? Image { get; set; }
        public bool RemoveImage { get; set; }
    }
}
