using System.ComponentModel.DataAnnotations;

namespace JobFind.ViewModel.Account
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Parol və təsdiq parolu uyğun gəlmir.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "User Role")]
        public string Role { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]

        public string Username { get; set; }
        [Required,DataType(DataType.PhoneNumber)]

        public string PhoneNumber { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        
        

    }
}
