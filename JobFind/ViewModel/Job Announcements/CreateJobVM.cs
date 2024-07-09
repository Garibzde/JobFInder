using System.ComponentModel.DataAnnotations;

namespace JobFind.ViewModel.Job_Announcements
{
    public class CreateJobVM
    {
        [Required(ErrorMessage = "Please enter your name")]
        [StringLength(50, ErrorMessage = "Your name cannot be more than 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please provide an image file")]
        public IFormFile Image { get; set; }  

        [Required(ErrorMessage = "Please enter your description")]
        [StringLength(500, ErrorMessage = "Your description cannot be more than 500 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter your city")]
        [StringLength(50, ErrorMessage = "Your city name cannot be more than 50 characters.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Salary is required.")]
        [Range(100, 1000000, ErrorMessage = "Salary should be between 100 and 1,000,000.")]
        public decimal Salary { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Please provide the posted date")]
        public DateTime PostedDate { get; set; }

        [Required(ErrorMessage = "Nature is required.")]
        [Display(Name = "Nature")]
        public int NatureId { get; set; }

        [Required(ErrorMessage = "Please enter the company name")]
        [StringLength(100, ErrorMessage = "Company name cannot be more than 100 characters.")]
        public string CompanyName { get; set; }
    }
}
