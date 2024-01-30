using Microsoft.IdentityModel.Tokens;
using SigningAPI.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SigningAPI.Services
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
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
                new Claim(ClaimTypes.Email,user.Email.ToString())
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
    }
}
