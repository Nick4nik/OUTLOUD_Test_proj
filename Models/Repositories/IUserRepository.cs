namespace OUTLOUD_Test_proj.Models.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsers();
        Task<User?> IsValidAuth(Guid token);
        Task<Guid?> LoginReturnsToken(string login, string password);
        Task<User> Login(string login, string password);
    }
}
