using Moq;
using SimpleBlog.Application.Interfaces;
using SimpleBlog.Web.API.Controllers;
using SimpleBlog.Web.API.ViewModels;
using SimpleBlog.Application.Mappers;
using System.Security.Claims;
using SimpleBlog.Domain.Entities;

namespace SimpleBlog.Web.API.Tests.Controllers
{
    public class PostsControllerTests
    {
        private Mock<IUserService> _userServiceMock;
        private Mock<IPostService> _postServiceMock;
        private PostsController _postsController;

        [SetUp]
        public void SetUp()
        {
            _userServiceMock = new Mock<IUserService>();
            _postServiceMock = new Mock<IPostService>();
            _postsController = new PostsController(_userServiceMock.Object, _postServiceMock.Object);
        }

        [Test]
        public async Task GetAll_WhenCalled_ReturnsAllPosts()
        {
            var posts = new List<PostDTO> 
            { 
                new() { Id = 1, Content = "content1", Title = "title1", User = new UserDTO() { Id = 1 } },
                new() { Id = 2, Content = "content2", Title = "title2", User = new UserDTO() { Id = 2 } }
            };
            _postServiceMock.Setup(s => s.GetAll()).ReturnsAsync(posts);

            var result = await _postsController.GetAll();
            var expected = posts.Select(p => p.ToResponse());

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.First().Id, Is.EqualTo(expected.First().Id));
                Assert.That(result.First().Title, Is.EqualTo(expected.First().Title));
                Assert.That(result.First().Content, Is.EqualTo(expected.First().Content));
                Assert.That(result.First().UserId, Is.EqualTo(expected.First().UserId));
                Assert.That(result.Last().Id, Is.EqualTo(expected.Last().Id));
                Assert.That(result.Last().Title, Is.EqualTo(expected.Last().Title));
                Assert.That(result.Last().Content, Is.EqualTo(expected.Last().Content));
                Assert.That(result.Last().UserId, Is.EqualTo(expected.Last().UserId));
            });
        }

        [Test]
        public async Task GetById_WhenCalledWithExistingId_ReturnsPost()
        {
            var post = new PostDTO() { Id = 1, Content = "content1", Title = "title1", User = new UserDTO() { Id = 1 } };
            _postServiceMock.Setup(s => s.GetById(1)).ReturnsAsync(post);

            var result = await _postsController.GetById(1);
            var expected = post.ToResponse();

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result!.Id, Is.EqualTo(expected.Id));
                Assert.That(result.Title, Is.EqualTo(expected.Title));
                Assert.That(result.Content, Is.EqualTo(expected.Content));
                Assert.That(result.UserId, Is.EqualTo(expected.UserId));
            });
        }

        [Test]
        public async Task GetById_WhenCalledWithNonExistingId_ReturnsNull()
        {
            _postServiceMock.Setup(s => s.GetById(1)).ReturnsAsync((PostDTO)null!);

            var result = await _postsController.GetById(1);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task Create_WhenCalled_CallsAddOnPostService()
        {
            var request = new PostCreateRequest();
            var user = new UserDTO();
            _userServiceMock.Setup(s => s.GetLoggedInUser(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            await _postsController.Create(request);

            _postServiceMock.Verify(s => s.Add(It.IsAny<PostDTO>()), Times.Once);
        }

        [Test]
        public async Task Update_WhenCalledWithMatchingId_CallsUpdateOnPostService()
        {
            var request = new PostUpdateRequest { Id = 1, Title = "title", Content = "content" };
            var user = new UserDTO();
            _userServiceMock.Setup(s => s.GetLoggedInUser(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            await _postsController.Update(1, request);

            _postServiceMock.Verify(s => s.Update(
                It.Is<PostDTO>(p => p.Title == request.Title && p.Content == request.Content)), Times.Once);
        }

        [Test]
        public void Update_WhenCalledWithNonMatchingId_ThrowsArgumentException()
        {
            var request = new PostUpdateRequest { Id = 1 };
            var user = new UserDTO();
            _userServiceMock.Setup(s => s.GetLoggedInUser(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            Assert.ThrowsAsync<ArgumentException>(() => _postsController.Update(2, request));
        }

        [Test]
        public async Task Delete_WhenCalled_CallsDeleteOnPostService()
        {
            var user = new UserDTO();
            _userServiceMock.Setup(s => s.GetLoggedInUser(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            await _postsController.Delete(1);

            _postServiceMock.Verify(s => s.Delete(1, user), Times.Once);
        }
    }
}