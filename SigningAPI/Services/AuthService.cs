using Microsoft.IdentityModel.Tokens;
using SigningAPI.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SigningAPI.Services
{
    public class AuthService
    {
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
                new SymmetricSecurityKey(
                    key: Encoding.UTF8.GetBytes("Mening-JWt-Keyim-Shu-Mana-Shu-Edi,O'zingiz-yaxshimi-ishlar-bolyaptimi?")),
                    algorithm: SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "Issuer",
                audience: "Audience",
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: signingCredentials
                );

            var accesToken = new JwtSecurityTokenHandler().WriteToken(token);
            return accesToken;
        }
    }
}
