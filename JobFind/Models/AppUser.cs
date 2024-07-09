using Microsoft.AspNetCore.Identity;

namespace JobFind.Models
{
    public class AppUser:IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsApproved { get; set; } = false;
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
