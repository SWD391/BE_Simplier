using Microsoft.Extensions.Configuration;

namespace BusinessObjects
{
    public class ConnectionString
    {
        public static string? GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", true, true)
                        .Build();
            return config["ConnectionStrings:DefaultConnectionStringDB"];

        }

    }

    //dotnet ef dbcontext scaffold "Server=(local);uid=sa;pwd=1234567890;database=PetStore2023DB;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer --output-dir Models
}