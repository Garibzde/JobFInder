using System.ComponentModel.DataAnnotations;

namespace JobFind.ViewModel.Account
{
    public class LoginVM
    {
        [Required]
        [Display(Name = "Username or Email")]
        public string UsernameOrEmail { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
