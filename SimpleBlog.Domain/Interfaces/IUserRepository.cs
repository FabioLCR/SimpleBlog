using SimpleBlog.Domain.Entities;

namespace SimpleBlog.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<UserEntity?> GetByUsername(string username);
        Task Add(UserEntity user);
    }
}
