using Microsoft.Extensions.Logging;
using SimpleBlog.Application.Interfaces;
using SimpleBlog.Domain.Entities;
using SimpleBlog.Domain.Exceptions;
using SimpleBlog.Domain.Interfaces;
using SimpleBlog.Web.API.Interfaces;

namespace SimpleBlog.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly ILogger<PostService> _logger;
        private readonly INotificationService _notificationService;

        public PostService(IPostRepository postRepository, ILogger<PostService> logger, INotificationService notificationService)
        {
            _postRepository = postRepository;
            _logger = logger;
            _notificationService = notificationService;
        }

        public async Task<IEnumerable<PostEntity>> GetAll()
        {
            _logger.LogInformation("Obtendo todas as postagens");
            return await _postRepository.GetAll();
        }

        public async Task<PostEntity?> GetById(int id)
        {
            _logger.LogInformation("Obtendo postagem pelo ID: {Id}", id);
            return await _postRepository.GetById(id);
        }

        public async Task Add(PostEntity post, UserEntity loggedInUser)
        {
            _logger.LogInformation("Adicionando nova postagem pelo usu�rio: {Username}", loggedInUser.Username);

            if (loggedInUser == null)
                throw new UserNotAuthorizedException("Usu�rio n�o autorizado");

            if (await _postRepository.GetById(post.Id) != null)
                throw new PostConflictException("J� existe uma postagem com o ID informado");

            await _postRepository.Add(post);
            _logger.LogInformation("Postagem adicionada com sucesso");

            await _notificationService.SendNotificationToAll($"Nova postagem criada: {post.Title}");
        }

        public async Task Update(PostEntity post, UserEntity loggedInUser)
        {
            _logger.LogInformation("Atualizando postagem pelo usu�rio: {Username}", loggedInUser.Username);

            if (loggedInUser == null)
                throw new UserNotAuthorizedException("Usu�rio n�o autorizado");

            var existingPost = await _postRepository.GetById(loggedInUser.Id)
                ?? throw new PostNotFoundException("Postagem n�o encontrada");

            if (!existingPost.CanEdit(loggedInUser))
                throw new UserNotAuthorizedException("Usu�rio n�o pode editar esta postagem");

            await _postRepository.Update(post);
            _logger.LogInformation("Postagem atualizada com sucesso");
        }

        public async Task Delete(int id, UserEntity loggedInUser)
        {
            _logger.LogInformation("Excluindo postagem pelo usu�rio: {Username}", loggedInUser.Username);

            if (loggedInUser == null)
                throw new UserNotAuthorizedException("Usu�rio n�o autorizado");

            var existingPost = await _postRepository.GetById(loggedInUser.Id)
                ?? throw new PostNotFoundException("Postagem n�o encontrada");

            if (!existingPost.CanEdit(loggedInUser))
                throw new UserNotAuthorizedException("Usu�rio n�o pode excluir esta postagem");

            await _postRepository.Delete(id);
            _logger.LogInformation("Postagem exclu�da com sucesso");
        }
    }
}
