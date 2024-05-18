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
                throw new UserNotAuthorizedException("Usu�rio n�o autorizado");

            if (await _postRepository.GetById(post.Id) != null)
                throw new PostConflictException("J� existe uma postagem com o ID informado");

            await _postRepository.Add(post);
        }

        public async Task Update(PostEntity post, UserEntity loggedInUser)
        {
            if (loggedInUser == null)
                throw new UserNotAuthorizedException("Usu�rio n�o autorizado");

            var existingPost = await _postRepository.GetById(loggedInUser.Id)
                ?? throw new PostNotFoundException("Postagem n�o encontrada");

            if (!existingPost.CanEdit(loggedInUser))
                throw new UserNotAuthorizedException("Usu�rio n�o pode editar esta postagem");

            await _postRepository.Update(post);
        }

        public async Task Delete(int id, UserEntity loggedInUser)
        {
            if (loggedInUser == null)
                throw new UserNotAuthorizedException("Usu�rio n�o autorizado");

            var existingPost = await _postRepository.GetById(loggedInUser.Id)
                ?? throw new PostNotFoundException("Postagem n�o encontrada");

            if (!existingPost.CanEdit(loggedInUser))
                throw new UserNotAuthorizedException("Usu�rio n�o pode excluir esta postagem");

            await _postRepository.Delete(id);
        }
    }
}