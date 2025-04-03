using UserService.Models;

namespace UserService.Services
{
    public interface IAuthService
    {
        string GenerateJwtToken(ApplicationUser user);

    }
}
