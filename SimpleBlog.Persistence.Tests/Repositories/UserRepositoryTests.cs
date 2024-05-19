using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleBlog.Domain.Entities;
using SimpleBlog.Persistence.Context;
using SimpleBlog.Persistence.Repositories;

namespace SimpleBlog.Persistence.Tests.Repositories
{
    public class UserRepositoryTests
    {
        private SqliteContext _context;
        private ILogger<UserRepository> _logger;
        private UserRepository _userRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<SqliteContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new SqliteContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _logger = new Logger<UserRepository>(new LoggerFactory());
            _userRepository = new UserRepository(_context, _logger);
        }

        [Test]
        public async Task GetByUsername_ReturnsUser_WhenUserExists()
        {
            var user = new UserEntity { Username = "testuser", Password = "testpassword" };
            _context.Users!.Add(user);
            await _context.SaveChangesAsync();

            var result = await _userRepository.GetByUsername("testuser");

            Assert.That(result, Is.EqualTo(user));
        }

        [Test]
        public async Task GetByUsername_ReturnsNull_WhenUserDoesNotExist()
        {
            var result = await _userRepository.GetByUsername("nonexistentuser");

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task Add_AddsNewUser()
        {
            var user = new UserEntity { Username = "newuser", Password = "newpassword" };

            await _userRepository.Add(user);
            await _context.SaveChangesAsync();

            var result = await _context.Users!.FirstOrDefaultAsync(u => u.Username == user.Username);

            Assert.That(result, Is.EqualTo(user));
        }
    }
}
