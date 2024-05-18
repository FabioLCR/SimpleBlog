using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SimpleBlog.Application.Services;
using SimpleBlog.Domain.Interfaces;
using SimpleBlog.Persistence.Context;
using SimpleBlog.Persistence.Repositories;
using SimpleBlog.Persistence.Repositories.SimpleBlog.Persistence.Repositories;
using System.Text;

namespace SimpleBlog.Web.API
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration) =>
            services.AddApplicationServices(configuration)
                    .AddPersistenceServices(configuration)
                    .AddAuthenticationServices(configuration);
                   

        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration) =>
            services.AddTransient<UserService>()
                    .AddTransient<PostService>();


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
    }
}