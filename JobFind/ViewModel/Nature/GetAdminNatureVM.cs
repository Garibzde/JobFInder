using System.ComponentModel.DataAnnotations;

namespace JobFind.ViewModel.Nature
{
    public class GetAdminNatureVM
    {
        [Required]
        public int Id { get; set; }
        [Required, MinLength(2), MaxLength(12)]
        public string Name { get; set; }
    }
}
