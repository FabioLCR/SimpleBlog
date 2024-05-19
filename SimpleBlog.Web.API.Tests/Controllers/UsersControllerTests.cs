using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using SimpleBlog.Application.Interfaces;
using SimpleBlog.Domain.Exceptions;
using SimpleBlog.Web.API.Controllers;
using SimpleBlog.Web.API.ViewModels;

namespace SimpleBlog.Web.API.Tests.Controllers
{
    public class UsersControllerTests
    {
        private Mock<IUserService> _userServiceMock;
        private UsersController _usersController;

        [SetUp]
        public void SetUp()
        {
            _userServiceMock = new Mock<IUserService>();
            _usersController = new UsersController(_userServiceMock.Object);
        }

        [Test]
        public async Task Register_WhenCalled_CallsRegisterOnUserService()
        {
            var request = new UserRegisterRequest { Username = "test", Password = "password" };

            await _usersController.Register(request);

            _userServiceMock.Verify(s => s.Register(request.Username, request.Password), Times.Once);
        }

        [Test]
        public async Task Login_WhenCalledWithValidCredentials_ReturnsOkWithToken()
        {
            var request = new UserLoginRequest { Username = "test", Password = "password" };
            var expectedToken = "token";
            _userServiceMock.Setup(s => s.Login(request.Username, request.Password)).ReturnsAsync(expectedToken);

            var result = await _usersController.Login(request) as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            var resultJson = JsonConvert.SerializeObject(result.Value);
            var expectedJson = JsonConvert.SerializeObject(new { token = expectedToken });
            Assert.That(resultJson, Is.EqualTo(expectedJson));
        }

        [Test]
        public async Task Login_WhenUserNotFound_ReturnsUnauthorized()
        {
            var request = new UserLoginRequest { Username = "test", Password = "password" };
            _userServiceMock.Setup(s => s.Login(request.Username, request.Password)).Throws<UserNotFoundException>();

            var result = await _usersController.Login(request) as UnauthorizedObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo("Credenciais inválidas"));
        }

        [Test]
        public async Task Login_WhenInvalidPassword_ReturnsUnauthorized()
        {
            var request = new UserLoginRequest { Username = "test", Password = "password" };
            _userServiceMock.Setup(s => s.Login(request.Username, request.Password)).Throws<InvalidPasswordException>();

            var result = await _usersController.Login(request) as UnauthorizedObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo("Credenciais inválidas"));
        }
    }
}