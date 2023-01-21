namespace OUTLOUD_Test_proj.Models.Repositories
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
            //RSS rss = new(link);
            //await _db.RSS.AddAsync(rss);
            //if (await _db.SaveChangesAsync() == 1)
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

        public async Task SetRssAsRead(User user)
        {
            var rssList = await _db.RSS.Where(x => x.Users.Contains(user) && !x.IsRead).ToListAsync();
            foreach (var rss in rssList)
                rss.IsRead = true;
        }
    }
}
