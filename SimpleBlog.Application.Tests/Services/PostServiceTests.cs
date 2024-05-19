using Moq;
using NUnit.Framework;
using SimpleBlog.Application.Interfaces;
using SimpleBlog.Application.Services;
using SimpleBlog.Domain.Entities;
using SimpleBlog.Domain.Exceptions;
using SimpleBlog.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SimpleBlog.Web.API.Interfaces;
using SimpleBlog.Application.DTOs;

namespace SimpleBlog.Application.Tests.Services
{
    public class PostServiceTests
    {
        private Mock<IPostRepository> _postRepositoryMock;
        private Mock<INotificationService> _notificationServiceMock;
        private Mock<ILogger<PostService>> _loggerMock;
        private PostService _postService;

        [SetUp]
        public void Setup()
        {
            _postRepositoryMock = new Mock<IPostRepository>();
            _notificationServiceMock = new Mock<INotificationService>();
            _loggerMock = new Mock<ILogger<PostService>>();
            _postService = new PostService(_postRepositoryMock.Object, _notificationServiceMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetAll_ReturnsAllPosts()
        {
            _postRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(new List<PostEntity>());

            var result = await _postService.GetAll();

            Assert.That(result, Is.Not.Null);
            _postRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Test]
        public void GetById_WithNonExistingId_ThrowsPostNotFoundException()
        {
            _postRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((PostEntity)null!);

            Assert.ThrowsAsync<PostNotFoundException>(async () => await _postService.GetById(1));
        }

        [Test]
        public async Task Add_WithValidPost_AddsPost()
        {
            var post = new PostDTO { Id = 1, User = new UserDTO { Id = 1 } };
            _postRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((PostEntity)null!);

            await _postService.Add(post);

            _postRepositoryMock.Verify(repo => repo.Add(It.IsAny<PostEntity>()), Times.Once);
            _notificationServiceMock.Verify(service => service.SendNotificationToAll(It.IsAny<NotificationDTO>()), Times.Once);
        }

        [Test]
        public void Add_WithNullUser_ThrowsUserNotAuthorizedException()
        {
            var post = new PostDTO { User = null! };
            Assert.ThrowsAsync<UserNotAuthorizedException>(async () => await _postService.Add(post));
        }

        [Test]
        public void Add_WithExistingPostId_ThrowsPostConflictException()
        {
            var post = new PostDTO { Id = 1, User = new UserDTO { Id = 1 } };
            _postRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(new PostEntity());

            Assert.ThrowsAsync<PostConflictException>(async () => await _postService.Add(post));
        }

        [Test]
        public async Task Update_WithValidPost_UpdatesPost()
        {
            var post = new PostDTO { Id = 1, User = new UserDTO { Id = 1 } };
            _postRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(new PostEntity { UserId = 1 });

            await _postService.Update(post);

            _postRepositoryMock.Verify(repo => repo.Update(It.IsAny<PostEntity>()), Times.Once);
        }

        [Test]
        public void Update_WithNullUser_ThrowsUserNotAuthorizedException()
        {
            var post = new PostDTO { User = null! };
            Assert.ThrowsAsync<UserNotAuthorizedException>(async () => await _postService.Update(post));
        }

        [Test]
        public void Update_WithNonExistingPost_ThrowsPostNotFoundException()
        {
            var post = new PostDTO { Id = 1, User = new UserDTO { Id = 1 } };
            _postRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((PostEntity)null!);

            Assert.ThrowsAsync<PostNotFoundException>(async () => await _postService.Update(post));
        }

        [Test]
        public async Task Delete_WithValidPost_DeletesPost()
        {
            var user = new UserDTO { Id = 1 };
            _postRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(new PostEntity { UserId = 1 });

            await _postService.Delete(1, user);

            _postRepositoryMock.Verify(repo => repo.Delete(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void Delete_WithNullUser_ThrowsUserNotAuthorizedException()
        {
            Assert.ThrowsAsync<UserNotAuthorizedException>(async () => await _postService.Delete(1, null!));
        }

        [Test]
        public void Delete_WithNonExistingPost_ThrowsPostNotFoundException()
        {
            var user = new UserDTO { Id = 1 };
            _postRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((PostEntity)null!);

            Assert.ThrowsAsync<PostNotFoundException>(async () => await _postService.Delete(1, user));
        }

    }
}
