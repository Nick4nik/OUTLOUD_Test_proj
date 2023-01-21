namespace OUTLOUD_Test_proj.Models.Repositories
{
    public interface IRssRepository
    {
        Task<List<RSS>> GetAllRss(User user);
        Task<List<RSS>> GetUnreadRss(User user);
        Task<List<RSS>> GetUnreadRssWithDate(User user, DateTime date);
        Task<bool> SetRssAsRead(User user, string link);
        Task<bool> AddRss(User user, string link);
    }
}
