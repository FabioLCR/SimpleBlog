using Moq;
using NUnit.Framework;
using SimpleBlog.Application.Services;
using SimpleBlog.Domain.Entities;
using SimpleBlog.Domain.Exceptions;
using SimpleBlog.Domain.Interfaces;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SimpleBlog.Application.Tests.Services
{
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IConfiguration> _configurationMock;
        private Mock<ILogger<UserService>> _loggerMock;
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _configurationMock = new Mock<IConfiguration>();
            _loggerMock = new Mock<ILogger<UserService>>();
            _userService = new UserService(_userRepositoryMock.Object, _configurationMock.Object, _loggerMock.Object);
        }

        [Test]
        public void Register_WithExistingUsername_ThrowsUserAlreadyExistsException()
        {
            _userRepositoryMock.Setup(repo => repo.GetByUsername(It.IsAny<string>())).ReturnsAsync(new UserEntity());

            Assert.ThrowsAsync<UserAlreadyExistsException>(async () => await _userService.Register("username", "password"));
        }

        [Test]
        public void Register_WithMissingUserSecret_ThrowsConfigurationException()
        {
            _userRepositoryMock.Setup(repo => repo.GetByUsername(It.IsAny<string>())).ReturnsAsync((UserEntity)null!);
            _configurationMock.Setup(config => config["UserSecret"]).Returns((string)null!);

            Assert.ThrowsAsync<ConfigurationException>(async () => await _userService.Register("username", "password"));
        }

        [Test]
        public async Task Register_WithValidUsernameAndPassword_RegistersUser()
        {
            _userRepositoryMock.Setup(repo => repo.GetByUsername(It.IsAny<string>())).ReturnsAsync((UserEntity)null!);
            _configurationMock.Setup(config => config["UserSecret"]).Returns("secret");

            await _userService.Register("username", "password");

            _userRepositoryMock.Verify(repo => repo.Add(It.IsAny<UserEntity>()), Times.Once);
        }

        [Test]
        public void GetLoggedInUser_WithNonExistingUser_ThrowsUserNotAuthorizedException()
        {
            _userRepositoryMock.Setup(repo => repo.GetByUsername(It.IsAny<string>())).ReturnsAsync((UserEntity)null!);

            Assert.ThrowsAsync<UserNotAuthorizedException>(async () => await _userService.GetLoggedInUser(new ClaimsPrincipal()));
        }

        [Test]
        public async Task GetLoggedInUser_WithExistingUser_ReturnsUser()
        {
            var user = new UserEntity { Username = "username" };
            _userRepositoryMock.Setup(repo => repo.GetByUsername(It.IsAny<string>())).ReturnsAsync(user);

            var result = await _userService.GetLoggedInUser(new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.Name, "username")
            })));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Username, Is.EqualTo(user.Username));
        }

        [Test]
        public void Login_WithNonExistingUsername_ThrowsUserNotFoundException()
        {
            _userRepositoryMock.Setup(repo => repo.GetByUsername(It.IsAny<string>())).ReturnsAsync((UserEntity)null!);

            Assert.ThrowsAsync<UserNotFoundException>(async () => await _userService.Login("username", "password"));
        }

        [Test]
        public void Login_WithInvalidPassword_ThrowsInvalidPasswordException()
        {
            _userRepositoryMock.Setup(repo => repo.GetByUsername(It.IsAny<string>())).ReturnsAsync(new UserEntity());
            _configurationMock.Setup(config => config["UserSecret"]).Returns("secret");

            Assert.ThrowsAsync<InvalidPasswordException>(async () => await _userService.Login("username", "wrongpassword"));
        }


        [Test]
        public void Login_WithMissingUserSecret_ThrowsConfigurationException()
        {
            var user = new UserEntity { Username = "username" };
            user.SetPassword("password", "secret");
            _userRepositoryMock.Setup(repo => repo.GetByUsername(It.IsAny<string>())).ReturnsAsync(user);
            _configurationMock.Setup(config => config["UserSecret"]).Returns((string)null!);

            Assert.ThrowsAsync<ConfigurationException>(async () => await _userService.Login("username", "password"));
        }

        [Test]
        public void Login_WithMissingJwtKey_ThrowsConfigurationException()
        {
            var user = new UserEntity { Username = "username" };
            user.SetPassword("password", "secret");
            _userRepositoryMock.Setup(repo => repo.GetByUsername(It.IsAny<string>())).ReturnsAsync(user);
            _configurationMock.Setup(config => config["UserSecret"]).Returns("secret");
            _configurationMock.Setup(config => config["Jwt:Key"]).Returns((string)null!);

            Assert.ThrowsAsync<ConfigurationException>(async () => await _userService.Login("username", "password"));
        }

        [Test]
        public async Task Login_WithValidUsernameAndPassword_ReturnsToken()
        {
            var user = new UserEntity { Username = "username" };
            user.SetPassword("password", "secret");
            _userRepositoryMock.Setup(repo => repo.GetByUsername(It.IsAny<string>())).ReturnsAsync(user);
            _configurationMock.Setup(config => config["UserSecret"]).Returns("secret");
            _configurationMock.Setup(config => config["Jwt:Key"]).Returns(Guid.NewGuid().ToString("N"));

            var result = await _userService.Login("username", "password");

            Assert.That(result, Is.Not.Null);
        }
    }
}
