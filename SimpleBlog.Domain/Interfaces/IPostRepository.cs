using SimpleBlog.Domain.Entities;

namespace SimpleBlog.Domain.Interfaces
{
    public interface IPostRepository
    {
        Task<IEnumerable<PostEntity>> GetAll();
        Task<PostEntity?> GetById(int id);
        Task Add(PostEntity post);
        Task Update(PostEntity post);
        Task Delete(int id);
    }
}