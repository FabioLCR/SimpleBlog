using SimpleBlog.Domain.Entities;

namespace SimpleBlog.Application.Interfaces
{
    public interface IPostService
    {
        Task<IEnumerable<PostDTO>> GetAll();
        Task<PostDTO?> GetById(int id);
        Task Add(PostDTO post);
        Task Update(PostDTO post);
        Task Delete(int id, UserDTO loggedInUser);
    }
}