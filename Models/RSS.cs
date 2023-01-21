namespace OUTLOUD_Test_proj.Models
{
#nullable disable
    public class RSS
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Link { get; set; }
        public int Id { get; set; }
        public DateTimeOffset Created { get; set; }
        public bool IsRead { get; set; }
        public List<User> Users { get; set; } = new();
    }
}
