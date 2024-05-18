using SimpleBlog.Domain.Entities;

namespace SimpleBlog.Application.Interfaces
{
    public interface IPostService
    {
        Task Add(PostEntity post, UserEntity loggedInUser);
        Task Delete(int id, UserEntity loggedInUser);
        Task<IEnumerable<PostEntity>> GetAll();
        Task<PostEntity?> GetById(int id);
        Task Update(PostEntity post, UserEntity loggedInUser);
    }
}