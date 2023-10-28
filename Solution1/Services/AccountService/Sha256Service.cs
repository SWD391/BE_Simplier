using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Utilities;
using System.Security.Cryptography;

namespace Services.AccountService
{
    public class Sha256Service
    {
        private readonly IConfiguration _config;

        public Sha256Service()
        {
            _config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", true, true)
                        .Build();
        }

        public string HashPassword(string password)
        {
            string salt = _config["Salt"] ?? ""; 

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password + salt);
                byte[] hashedBytes = sha256.ComputeHash(passwordBytes);

                string hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                return hashedPassword;
            }
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            string newHashedPassword = HashPassword(password);

            return newHashedPassword.Equals(hashedPassword, StringComparison.OrdinalIgnoreCase);
        }
    }
}
