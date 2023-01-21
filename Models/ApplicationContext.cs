namespace OUTLOUD_Test_proj.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<RSS> RSS { get; set; }
        public DbSet<User> Users { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            const string url = "https://google.com";


            List<RSS> rssList = new()
            {
                new RSS { Title = "1 feed", Content = "Description of the 1 feed", Link = url, Id = 1, Created = DateTime.Now, IsRead = false },
                new RSS { Title = "2 feed", Content = "Description of the 2 feed", Link = url, Id = 2, Created = DateTime.Now.AddDays(-1), IsRead = false },
                new RSS { Title = "3 feed", Content = "Description of the 3 feed", Link = url, Id = 3, Created = DateTime.Now.AddDays(-2), IsRead = false },
                new RSS { Title = "4 feed", Content = "Description of the 4 feed", Link = url, Id = 4, Created = DateTime.Now.AddDays(-3), IsRead = false }
            };

            
            List<User> userList = new()
            {
                new User { Id = 1, Login = "FirstUser", Password = "qwe" },
                new User { Id = 2, Login = "SecondUser", Password = "asd" }
            };
            modelBuilder.Entity<RSS>()
                .HasData(rssList);

            modelBuilder.Entity<User>()
                .HasData(userList);

            modelBuilder.Entity<User>()
                .HasMany(p => p.RSSList)
                .WithMany(p => p.Users)
                .UsingEntity(j => 
                    j.HasData(
                        new { UsersId = 1, RSSListId = 1 },
                        new { UsersId = 1, RSSListId = 2 },
                        new { UsersId = 1, RSSListId = 3 },
                        new { UsersId = 1, RSSListId = 4 },
                        new { UsersId = 2, RSSListId = 1 },
                        new { UsersId = 2, RSSListId = 2 },
                        new { UsersId = 2, RSSListId = 3 },
                        new { UsersId = 2, RSSListId = 4 }
            ));
        }
    }
}