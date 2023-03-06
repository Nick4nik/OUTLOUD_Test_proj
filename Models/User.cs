namespace Web_API_tests.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Login { get; set; }
        public required string Password { get; set; }
        public Guid Token { get; set; } = Guid.NewGuid();
        public List<RSS> RSSList { get; set; } = new();
    }
}
