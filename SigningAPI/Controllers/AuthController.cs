using Microsoft.AspNetCore.Mvc;
using SigningAPI.Entities;

namespace SigningAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            return Ok(new
            {
                Message = "Loginned successfully"
            });
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            return Ok("Registred successfully");
        }
    }
}
