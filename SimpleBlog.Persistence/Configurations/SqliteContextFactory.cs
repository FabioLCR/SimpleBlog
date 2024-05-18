using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SimpleBlog.Persistence.Context;

namespace SimpleBlog.Persistence.Configurations
{
    public class SqliteContextFactory : IDesignTimeDbContextFactory<SqliteContext>
    {
        public SqliteContext CreateDbContext(string[] args)
        {
            var parent = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent;
            var path = Path.Combine(parent!.FullName, "SimpleBlog.Web.API");
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<SqliteContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlite(connectionString);

            return new SqliteContext(optionsBuilder.Options);
        }
    }
}
