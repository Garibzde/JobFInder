using System.ComponentModel.DataAnnotations;

namespace JobFind.ViewModel.CV
{
    public class GetCvVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter your name")]
        [StringLength(50, ErrorMessage = "Your name cannot be more than 50 characters.")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Please enter your surname")]
        [StringLength(50, ErrorMessage = "Your surname cannot be more than 50 characters.")]
        public string Surname { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Your surname cannot be more than 50 characters.")]

        public string Uni { get; set; }

        [Required(ErrorMessage = "Please enter your certificate")]
        [StringLength(100, ErrorMessage = "Your certificate cannot be more than 100 characters.")]
        public string Certificate { get; set; }


        [Required(ErrorMessage = "Please enter your graduation date")]
        public DateTime GraduatedAt { get; set; }


        [Required(ErrorMessage = "Please enter a description")]
        [StringLength(1000, ErrorMessage = "Your description cannot be more than 1000 characters.")]
        public string Description { get; set; }


        [Required(ErrorMessage = "Please enter your skills")]
        [StringLength(50, ErrorMessage = "Your skills cannot be more than 50 characters.")]
        public string Skills1 { get; set; }
        [Required(ErrorMessage = "Please enter your skills")]
        [StringLength(50, ErrorMessage = "Your skills cannot be more than 50 characters.")]
        public string Skills2 { get; set; }
        [Required(ErrorMessage = "Please enter your skills")]
        [StringLength(50, ErrorMessage = "Your skills cannot be more than 50 characters.")]
        public string Skills3 { get; set; }

        [Required]

        [Display(Name = "Profile Image")]

        public string UrlImage { get; set; }


        [Required(ErrorMessage = "Please enter your phone number")]
        [StringLength(20, ErrorMessage = "Your phone number cannot be more than 20 characters.")]
        public string Phone { get; set; }


        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Please enter your city")]
        [StringLength(100, ErrorMessage = "Your city cannot be more than 100 characters.")]
        public string City { get; set; }


        [StringLength(200, ErrorMessage = "Your employer address cannot be more than 200 characters.")]
        public string EmployerAddress { get; set; }

        [Required, DataType(DataType.PhoneNumber)]
        [StringLength(50, ErrorMessage = "Your reference number cannot be more than 50 characters.")]
        public string ReferanceNum { get; set; }

        [StringLength(1000, ErrorMessage = "Your reference number cannot be more than 1000 characters.")]
        public string? MoreInformation { get; set; }
    }
}
