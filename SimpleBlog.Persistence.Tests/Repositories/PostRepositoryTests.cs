using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleBlog.Domain.Entities;
using SimpleBlog.Persistence.Context;
using SimpleBlog.Persistence.Repositories;

namespace SimpleBlog.Persistence.Tests.Repositories
{
    public class PostRepositoryTests
    {
        private SqliteContext _context;
        private ILogger<PostRepository> _logger;
        private PostRepository _postRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<SqliteContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new SqliteContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _logger = new Logger<PostRepository>(new LoggerFactory());
            _postRepository = new PostRepository(_context, _logger);
        }

        [Test]
        public async Task GetAll_ReturnsAllPosts()
        {
            var posts = new List<PostEntity>
            {
                new() { Id = 1, Title = "Post 1", Content = "Content 1" },
                new() { Id = 2, Title = "Post 2", Content = "Content 2" }
            };

            foreach (var post in posts)
            {
                _context.Posts!.Add(post);
            }
            await _context.SaveChangesAsync();

            var result = await _postRepository.GetAll();

            Assert.That(result, Is.EqualTo(posts));
        }


        [Test]
        public async Task GetById_ReturnsPostById()
        {
            var post = new PostEntity { Id = 1, Title = "Post 1", Content = "Content 1" };
            _context.Posts!.Add(post);
            await _context.SaveChangesAsync();

            var result = await _postRepository.GetById(1);

            Assert.That(result, Is.EqualTo(post));
        }



        [Test]
        public async Task GetById_ReturnsNull_WhenIdDoesNotExist()
        {
            var result = await _postRepository.GetById(3);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task Add_AddsNewPost()
        {
            var post = new PostEntity { Id = 3, Title = "Post 3", Content = "Content 3" };

            await _postRepository.Add(post);
            await _context.SaveChangesAsync();

            var result = await _context.Posts!.FindAsync(3);

            Assert.That(result, Is.EqualTo(post));
        }

        [Test]
        public async Task Add_ThrowsException_WhenIdAlreadyExists()
        {
            var post1 = new PostEntity { Id = 1, Title = "Post 1", Content = "Content 1" };
            var post2 = new PostEntity { Id = 1, Title = "Post 2", Content = "Content 2" };

            _context.Posts!.Add(post1);
            await _context.SaveChangesAsync();

            // Act
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => _postRepository.Add(post2));

            // Assert
            StringAssert.Contains(
                "The instance of entity type 'PostEntity' cannot be tracked because another instance with the same key value for {'Id'} is already being tracked. When attaching existing entities, ensure that only one entity instance with a given key value is attached.",
                ex.Message);
        }

        [Test]
        public async Task Update_UpdatesExistingPost()
        {
            var post = new PostEntity { Id = 1, Title = "Post 1", Content = "Content 1" };
            _context.Posts!.Add(post);
            await _context.SaveChangesAsync();

            post.Title = "Updated Post";
            post.Content = "Updated Content";

            await _postRepository.Update(post);
            await _context.SaveChangesAsync();

            var result = await _context.Posts!.FindAsync(1);
            Assert.Multiple(() =>
            {
                Assert.That(result?.Title, Is.EqualTo("Updated Post"));
                Assert.That(result?.Content, Is.EqualTo("Updated Content"));
            });
        }


        [Test]
        public async Task Delete_DeletesPostById()
        {
            await _postRepository.Delete(1);
            await _context.SaveChangesAsync();

            var result = await _context.Posts!.FindAsync(1);

            Assert.That(result, Is.Null);
        }
    }
}
