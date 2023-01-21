namespace OUTLOUD_Test_proj.Models.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsers();
        Task<User?> IsValidAuth(Guid token);
        Task<Guid?> Login(string login, string password);
    }
}
