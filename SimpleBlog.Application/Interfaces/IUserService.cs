using SimpleBlog.Domain.Entities;
using System.Security.Claims;

namespace SimpleBlog.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> GetLoggedInUser(ClaimsPrincipal userPrincipal);
        Task<string?> Login(string username, string password);
        Task Register(string username, string password);
    }
}