namespace OUTLOUD_Test_proj.Models.Repositories
{
    public interface IRssRepository
    {
        Task<List<RSS>> GetAllRss(User user);
        Task<List<RSS>> GetUnreadRss(User user);
        Task SetRssAsRead(User user);
        Task<bool> AddRss(User user, string link);
    }
}
