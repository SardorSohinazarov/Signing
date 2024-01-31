using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SigningAPI.Data;
using SigningAPI.Entities;
using SigningAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SigningAPI.Services
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AuthService(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<string> GenerateToken(User user)
        {
            /* var roles = user.Roles;
             var persmissions = roles.SelectMany(x => x.Permissions);
             var stringPermission = persmissions.Select(x => x.Name);

             var claims = new List<Claim>()
             {
                 new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())
             };

             foreach (var p in stringPermission)
             {
                 claims.Add(new Claim(ClaimTypes.Role, p));
             }
             */

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim("Email",user.Email.ToString())
            };

            var signingCredentials = new SigningCredentials(
                    key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"])),
                    algorithm: SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JWT:ExpireInMinutes"])),
                signingCredentials: signingCredentials);

            var accesToken = new JwtSecurityTokenHandler().WriteToken(token);
            return accesToken;
        }

        public async ValueTask<ClaimsPrincipal> GetClaimsFromExpiredTokenAsync(string token)
        {
            var validationParametrs = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = _configuration["JWT:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["JWT:Audience"],
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"])),
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var claimsPrincipal = tokenHandler.ValidateToken(token, validationParametrs, out SecurityToken securityToken);

            var jwtsecurityToken = securityToken as JwtSecurityToken;

            if (jwtsecurityToken == null)
                throw new Exception("Invalid token");

            return claimsPrincipal;
        }

        public async ValueTask<TokenDTO> RefreshToken(TokenDTOForCreation tokenDTO)
        {
            var claims = await GetClaimsFromExpiredTokenAsync(tokenDTO.AccessToken);

            var id = Convert.ToInt32(claims.FindFirst(ClaimTypes.NameIdentifier).Value);

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user.RefreshToken != tokenDTO.RefreshToken)
                throw new Exception("Refresh token is not valid");

            /*  if (user.TokenExpireTime >= DateTime.Now)
                  throw new Exception("Refresh token has already been expired");*/

            var newAccessToken = await GenerateToken(user);

            return new TokenDTO()
            {
                AccessToken = newAccessToken,
                RefreshToken = user.RefreshToken,
                ExpireDate = user.TokenExpireTime
            };
        }
    }
}
