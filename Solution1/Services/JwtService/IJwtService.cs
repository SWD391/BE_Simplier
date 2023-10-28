using BusinessObjects.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Services.JwtService
{
    public interface IJwtService
    {
        public string? GetAccountId(HttpContext httpContext);
        public string GenerateToken(Account account);
    }
}
