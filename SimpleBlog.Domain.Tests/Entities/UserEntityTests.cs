using SimpleBlog.Domain.Entities;

namespace SimpleBlog.Domain.Tests.Entities
{
    public class UserEntityTests
    {
        private UserEntity _user;
        private const string Key = "testKey";

        [SetUp]
        public void Setup()
        {
            _user = new UserEntity();
        }

        [Test]
        public void SetPassword_WithNullPassword_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _user.SetPassword(null!, Key));
        }

        [Test]
        public void SetPassword_WithNullKey_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _user.SetPassword("password", null!));
        }

        [Test]
        public void SetPassword_WithValidPasswordAndKey_SetsPassword()
        {
            _user.SetPassword("password", Key);
            Assert.That(_user.Password, Is.Not.Null);
        }

        [Test]
        public void IsPasswordValid_WithInvalidPassword_ReturnsFalse()
        {
            _user.SetPassword("password", Key);
            Assert.That(_user.IsPasswordValid("wrongPassword", Key), Is.False);
        }

        [Test]
        public void IsPasswordValid_WithValidPassword_ReturnsTrue()
        {
            _user.SetPassword("password", Key);
            Assert.That(_user.IsPasswordValid("password", Key), Is.True);
        }
    }
}
