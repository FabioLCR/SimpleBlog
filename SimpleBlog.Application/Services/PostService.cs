using SimpleBlog.Application.Interfaces;
using SimpleBlog.Domain.Entities;
using SimpleBlog.Domain.Exceptions;
using SimpleBlog.Domain.Interfaces;

namespace SimpleBlog.Application.Services
{
    public class PostService : IPostService
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

        public async Task Add(PostEntity post, UserEntity loggedInUser)
        {
            if (loggedInUser == null)
                throw new UserNotAuthorizedException("Usuário não autorizado");

            if (await _postRepository.GetById(post.Id) != null)
                throw new PostConflictException("Já existe uma postagem com o ID informado");

            await _postRepository.Add(post);
        }

        public async Task Update(PostEntity post, UserEntity loggedInUser)
        {
            if (loggedInUser == null)
                throw new UserNotAuthorizedException("Usuário não autorizado");

            var existingPost = await _postRepository.GetById(loggedInUser.Id)
                ?? throw new PostNotFoundException("Postagem não encontrada");

            if (!existingPost.CanEdit(loggedInUser))
                throw new UserNotAuthorizedException("Usuário não pode editar esta postagem");

            await _postRepository.Update(post);
        }

        public async Task Delete(int id, UserEntity loggedInUser)
        {
            if (loggedInUser == null)
                throw new UserNotAuthorizedException("Usuário não autorizado");

            var existingPost = await _postRepository.GetById(loggedInUser.Id)
                ?? throw new PostNotFoundException("Postagem não encontrada");

            if (!existingPost.CanEdit(loggedInUser))
                throw new UserNotAuthorizedException("Usuário não pode excluir esta postagem");

            await _postRepository.Delete(id);
        }
    }
}