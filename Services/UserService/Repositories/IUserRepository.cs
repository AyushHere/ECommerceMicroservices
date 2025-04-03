using UserService.Models;

namespace UserService.Repositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetUserByEmail(string email);
        Task AddUser(ApplicationUser user);
    }
}
