using System.ComponentModel.DataAnnotations;

namespace JobFind.ViewModel.Nature
{
    public class EditNatureVM
    {
        [Required, MinLength(2), MaxLength(12)]
        public string Name { get; set; }
    }
}
