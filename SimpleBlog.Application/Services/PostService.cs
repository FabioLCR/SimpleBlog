using Microsoft.Extensions.Logging;
using SimpleBlog.Application.Interfaces;
using SimpleBlog.Application.Mappers;
using SimpleBlog.Domain.Entities;
using SimpleBlog.Domain.Exceptions;
using SimpleBlog.Domain.Interfaces;

namespace SimpleBlog.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly INotificationService _notificationService;
        private readonly ILogger<PostService> _logger;

        public PostService(
            IPostRepository postRepository,
            INotificationService notificationService,
            ILogger<PostService> logger)
        {
            _postRepository = postRepository;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task<IEnumerable<PostDTO>> GetAll()
        {
            _logger.LogInformation("Obtendo todas as postagens");
            var posts = await _postRepository.GetAll();
            return posts.ToDTO();
        }

        public async Task<PostDTO?> GetById(int id)
        {
            _logger.LogInformation("Obtendo postagem pelo ID: {Id}", id);
            var post = await _postRepository.GetById(id);
            return post != null 
                ? post.ToDTO() 
                : throw new PostNotFoundException("Postagem não encontrada");
        }

        public async Task Add(PostDTO post)
        {
            if (post.User == null)
                throw new UserNotAuthorizedException("Usuário não autorizado");

            _logger.LogInformation("Adicionando nova postagem pelo usuário: {Username}", post.User.Username);

            if (await _postRepository.GetById(post.Id) != null)
                throw new PostConflictException("Já existe uma postagem com o ID informado");

            await _postRepository.Add(post.ToEntity());
            _logger.LogInformation("Postagem adicionada com sucesso");

            await _notificationService.SendNotificationToAll(post.ToNotificationDTO());
        }

        public async Task Update(PostDTO post)
        {
            if (post.User == null)
                throw new UserNotAuthorizedException("Usuário não autorizado");

            _logger.LogInformation("Atualizando postagem pelo usuário: {Username}", post.User.Username);

            var existingPost = await _postRepository.GetById(post.User.Id)
                ?? throw new PostNotFoundException("Postagem não encontrada");

            if (!existingPost.CanEdit(post.User.ToEntity()))
                throw new UserNotAuthorizedException("Usuário não pode editar esta postagem");

            await _postRepository.Update(post.ToEntity());
            _logger.LogInformation("Postagem atualizada com sucesso");
        }

        public async Task Delete(int id, UserDTO loggedInUser)
        {
            if (loggedInUser == null)
                throw new UserNotAuthorizedException("Usuário não autorizado");

            _logger.LogInformation("Excluindo postagem pelo usuário: {Username}", loggedInUser.Username);

            var existingPost = await _postRepository.GetById(loggedInUser.Id)
                ?? throw new PostNotFoundException("Postagem não encontrada");

            if (!existingPost.CanEdit(loggedInUser.ToEntity()))
                throw new UserNotAuthorizedException("Usuário não pode excluir esta postagem");

            await _postRepository.Delete(id);
            _logger.LogInformation("Postagem excluída com sucesso");
        }
    }
}
