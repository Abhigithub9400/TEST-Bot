using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MediAssist.DbContext
{
    public class MediAssistDbContextFactory : IDesignTimeDbContextFactory<MediAssistDbContext>
    {
        public MediAssistDbContext CreateDbContext(string[] args)
        {
            // Check for environment variable first
            var connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");

            if (string.IsNullOrEmpty(connectionString))
            {
                // Fallback to appsettings.json
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
                    .Build();

                connectionString = configuration.GetConnectionString("SqlConnectionString");
            }

            var optionsBuilder = new DbContextOptionsBuilder<MediAssistDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new MediAssistDbContext(optionsBuilder.Options);
        }
    }
}
