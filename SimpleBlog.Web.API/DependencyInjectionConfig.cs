using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SimpleBlog.Application.Interfaces;
using SimpleBlog.Application.Services;
using SimpleBlog.Domain.Interfaces;
using SimpleBlog.Persistence.Context;
using SimpleBlog.Persistence.Repositories;
using SimpleBlog.Web.API.Interfaces;
using SimpleBlog.Web.API.Services;
using System.Text;

namespace SimpleBlog.Web.API
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration) =>
            services.AddApplicationServices()
                    .AddPersistenceServices(configuration)
                    .AddAuthenticationServices(configuration)
                    .AddOtherServices();

        public static IServiceCollection AddApplicationServices(this IServiceCollection services) =>
            services.AddTransient<IUserService, UserService>()
                    .AddTransient<IPostService, PostService>();


        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<IDatabase, SqliteContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")))
                    .AddTransient<IUserRepository, UserRepository>()
                    .AddTransient<IPostRepository, PostRepository>();

        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            return services;
        }

        public static IServiceCollection AddOtherServices(this IServiceCollection services) => 
            services.AddSingleton<INotificationService, NotificationService>()
                    .AddSingleton<IWebSocketManagerService, WebSocketManagerService>();
    }
}