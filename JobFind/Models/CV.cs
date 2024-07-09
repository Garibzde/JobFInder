namespace JobFind.Models
{
    public class CV
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UniversityName { get; set; }
        public string Certificate { get; set; }
        public DateTime GraduatedAt { get; set; }
        public string Description { get; set; }
        
        public string Skills1 { get; set; }
        public string Skills2 { get; set; }
        public string Skills3 { get; set; }

        public string UrlImage { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string EmployerAddress { get; set; }
        public string ReferanceNum { get; set; }
        public string? İnformationAboutYourself { get; set; }
        public string UserId { get; set; }
    }
}
