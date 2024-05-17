using Microsoft.EntityFrameworkCore;
using SimpleBlog.Application.Services;
using SimpleBlog.Domain.Interfaces;
using SimpleBlog.Persistence.Context;
using SimpleBlog.Persistence.Repositories.SimpleBlog.Persistence.Repositories;

namespace SimpleBlog.Web.API
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration) =>
            services.AddApplicationServices(configuration)
                    .AddPersistenceServices(configuration);

        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration) =>
            services.AddScoped<UserService>();


        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<IDatabase, SqliteContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")))
                    .AddScoped<IUserRepository, UserRepository>();
            

    }
}