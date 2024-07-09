using System.ComponentModel.DataAnnotations;

namespace JobFind.ViewModel.Slider
{
    public class GetSliderVM
    {
        [Required]
        public string ImageFile { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Job { get; set; }

        [Required]
        public string Title { get; set; }
    }
}
