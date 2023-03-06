namespace Web_API_tests.Models.Repositories
{
    public class RssRepository : IRssRepository
    {
        private readonly ApplicationContext _db;

        public RssRepository(ApplicationContext db)
        {
            _db = db;
        }

        public async Task<bool> AddRss(User user, string link)
        {
            RSS rss = new()
            {
                Link = link,
                Title = "New title",
                Content = "New content",
                Created = DateTime.Now,
                Users = new() { user }
            };
            _db.RSS.Add(rss);
            if (await _db.SaveChangesAsync() == 2)
                return true;

            return false;
        }

        public async Task<List<RSS>> GetAllRss(User user)
        {
            return await _db.RSS.Where(x => x.Users.Contains(user)).ToListAsync();
        }

        public async Task<List<RSS>> GetUnreadRss(User user)
        {
            return await _db.RSS.Where(x => x.Users.Contains(user) && !x.IsRead).ToListAsync();
        }

        public async Task<List<RSS>> GetUnreadRssWithDate(User user, DateTime date)
        {
            var q = await _db.RSS.Where(x => x.Users.Contains(user) && !x.IsRead && x.Created.Date >= date.Date).ToListAsync();
            return q;
        }

        public async Task<bool> SetRssAsRead(User user, string link)
        {
            List<RSS> rssList = new();
            if (string.IsNullOrWhiteSpace(link) || string.Equals(link, "string"))
                rssList = await _db.RSS.Where(x => x.Users.Contains(user) && !x.IsRead).ToListAsync();
            else
                rssList = await _db.RSS.Where(x => x.Users.Contains(user) && x.Link == link).ToListAsync();

            foreach (var rss in rssList)
                rss.IsRead = true;

            await _db.SaveChangesAsync();

            return true;
        }
    }
}
