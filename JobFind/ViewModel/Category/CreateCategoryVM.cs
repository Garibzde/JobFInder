
using JobFind.Models;
using System.ComponentModel.DataAnnotations;

namespace JobFind.ViewModel.Category
{
    public class CreateCategoryVM
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public IFormFile? IconFile { get; set; }
        public  ICollection<Job> Jobs { get; set; }
            }
}
