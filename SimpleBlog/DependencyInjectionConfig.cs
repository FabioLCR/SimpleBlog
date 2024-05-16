using Microsoft.EntityFrameworkCore;
using SimpleBlog.Domain.Interfaces;
using SimpleBlog.Persistence.Context;

namespace SimpleBlog.Web.API
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration) =>
            services.AddApplicationServices(configuration)
                    .AddPersistenceServices(configuration);

        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }

        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddDbContext<IDatabase, SqliteContext>(options => 
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
        }

    }
}