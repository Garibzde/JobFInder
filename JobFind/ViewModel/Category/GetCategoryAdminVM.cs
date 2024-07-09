using JobFind.Models;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace JobFind.ViewModel.Category
{
    public class GetCategoryAdminVM
    {
        [Required]
        public int Id { get; set; }
        [Required, MinLength(3), MaxLength(32)]
        public string Name { get; set; }
        [Required]

        public string Icon { get; set; }

        public ICollection<Job> jobs {  get; set; }  
        
    }
}
