using Microsoft.AspNetCore.Mvc;
using UserService.Models;
using UserService.Repositories;
using UserService.Services;
using Microsoft.AspNetCore.Identity;
namespace UserService.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IAuthService _authService;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthController(IUserRepository userRepo, IAuthService authService)
        {
            _userRepo = userRepo;
            _authService = authService;
            _passwordHasher = new PasswordHasher<User>(); 
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            user.PasswordHash = _passwordHasher.HashPassword(user, user.PasswordHash);
            await _userRepo.AddUser(user);
            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var existingUser = await _userRepo.GetUserByEmail(user.Email);
            if (existingUser == null) return Unauthorized("Invalid credentials");

      
            var result = _passwordHasher.VerifyHashedPassword(existingUser, existingUser.PasswordHash, user.PasswordHash);
            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Invalid credentials");

            var token = _authService.GenerateJwtToken(existingUser);
            return Ok(new { Token = token });
        }
    }
}
