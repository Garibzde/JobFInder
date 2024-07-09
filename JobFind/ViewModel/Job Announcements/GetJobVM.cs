namespace JobFind.ViewModel.Job_Announcements
{
    public class GetJobVM
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public decimal Salary { get; set; }
        public DateTime PostedDate { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int NatureId { get; set; }
        public string NatureName { get; set; }
        public string CompanyName { get; set; }

    }
}
