using SimpleBlog.Domain.Entities;
using SimpleBlog.Domain.Interfaces;

namespace SimpleBlog.Application.Services
{
    public class PostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<IEnumerable<PostEntity>> GetAll() => 
            await _postRepository.GetAll();

        public async Task<PostEntity?> GetById(int id) => 
            await _postRepository.GetById(id);

        public async Task Add(PostEntity post) => 
            await _postRepository.Add(post);

        public async Task Update(PostEntity post) => 
            await _postRepository.Update(post);

        public async Task Delete(int id) => 
            await _postRepository.Delete(id);
    }
}