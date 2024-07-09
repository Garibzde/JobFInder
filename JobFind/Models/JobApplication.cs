namespace JobFind.Models
{
    public class JobApplication
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public string UserId { get; set; }
        public string CVFileName { get; set; }
        public string CVContentType { get; set; }
        public byte[] CVData { get; set; }
        public DateTime AppliedDate { get; set; }

        public Job Job { get; set; }
        public AppUser User { get; set; }
    }
}
