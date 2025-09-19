using System.ComponentModel.DataAnnotations;

namespace Beacon.ViewModels
{
    public class FaqCreateViewModel
    {
        [Required, StringLength(500)]
        public string Question { get; set; } = string.Empty;

        [Required, StringLength(4000)]
        public string Answer { get; set; } = string.Empty;
    }

    public class FaqEditViewModel : FaqCreateViewModel
    {
        [Required]
        public string FaqId { get; set; } = string.Empty;
    }
}
