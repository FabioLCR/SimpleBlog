using SimpleBlog.Domain.Entities;

namespace SimpleBlog.Domain.Tests.Entities
{
    public class PostEntityTests
    {
        private PostEntity _post;
        private UserEntity _user;

        [SetUp]
        public void Setup()
        {
            _user = new UserEntity { Id = 1 };
            _post = new PostEntity { UserId = _user.Id, User = _user };
        }

        [Test]
        public void CanEdit_WithSameUserId_ReturnsTrue()
        {
            var result = _post.CanEdit(_user);
            Assert.That(result, Is.True);
        }

        [Test]
        public void CanEdit_WithDifferentUserId_ReturnsFalse()
        {
            var otherUser = new UserEntity { Id = 2 };
            var result = _post.CanEdit(otherUser);
            Assert.That(result, Is.False);
        }
    }
}
