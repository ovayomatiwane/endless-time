using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Domain
{
    public class ApplicationDataContextFactory : IDesignTimeDbContextFactory<ApplicationDataContext>
    {
        public ApplicationDataContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../WebApi")) // <-- NB! Path to API
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("ApplicationDataContext");

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDataContext>();

            optionsBuilder.UseSqlServer(connectionString);

            return new ApplicationDataContext(optionsBuilder.Options);
        }
    }
}
