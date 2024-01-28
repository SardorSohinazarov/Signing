using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SigningAPI.Data;
using SigningAPI.Entities;
using SigningAPI.Helpers;
using SigningAPI.Models;

namespace SigningAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly ApplicationDbContext _context;

        public AuthController(IPasswordHasher passwordHasher, ApplicationDbContext context)
        {
            _passwordHasher = passwordHasher;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var storedUser = await _context.Users.FirstOrDefaultAsync(x
                => x.Email == loginDTO.Email
                && x.PasswordHash == _passwordHasher.Encrypt(loginDTO.Password, x.Salt));

            if (storedUser == null) { /*Register bo'lmagan didi*/ }
            return Ok(new
            {
                Message = "Loginned successfully"
            });
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            var storageUser = _context.Users.FirstOrDefault(x => x.Email == registerDTO.Email);
            var salt = Guid.NewGuid().ToString();
            var user = new User()
            {
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.PhoneNumber,
                PasswordHash = _passwordHasher.Encrypt(registerDTO.Password, salt),
                RefreshToken = DateTime.Now.ToString() + Guid.NewGuid().ToString(),
                Salt = salt,
                Name = registerDTO.Name,
                TokenExpireTime = DateTime.Now.AddDays(1),
            };

            if (registerDTO.Roles is not null)
            {
                //rollarini topib kelib qo'shadi
            }

            return Ok("Registred successfully");
        }
    }
}
