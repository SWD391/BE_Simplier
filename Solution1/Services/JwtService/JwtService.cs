using BusinessObjects.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace Services.JwtService
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;

        public JwtService()
        {
            _config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", true, true)
                        .Build();
        }

        public string? GetAccountId(HttpContext httpContext)
        {
            var identity = httpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                Console.Write(userClaims.Count());

                return userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
            }
            return null;
        }

        public string GenerateToken(Account account)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? ""));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.AccountId),
                new Claim(ClaimTypes.Role, account.Role.ToString())
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,

                expires: DateTime.Now.AddMinutes(1000),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
