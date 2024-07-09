namespace JobFind.Models
{
    public class Nature
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Job> Jobs { get; set; }
    }
}
