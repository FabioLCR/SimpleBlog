using SimpleBlog.Application.Mappers;
using SimpleBlog.Domain.Entities;
using SimpleBlog.Web.API.ViewModels;

namespace SimpleBlog.Application.Tests.Mappers
{
    public class PostViewMapperExtensionTests
    {
        private PostCreateRequest _postCreateRequest;
        private PostUpdateRequest _postUpdateRequest;
        private UserDTO _userDto;
        private List<PostDTO> _postDtoList;

        [SetUp]
        public void Setup()
        {
            _userDto = new UserDTO { Id = 1, Username = "username" };
            _postCreateRequest = new PostCreateRequest { Title = "title", Content = "content" };
            _postUpdateRequest = new PostUpdateRequest { Id = 1, Title = "title", Content = "content" };
            _postDtoList = new List<PostDTO>
            {
                new() { Id = 1, Title = "title1", Content = "content1", User = _userDto },
                new() { Id = 2, Title = "title2", Content = "content2", User = _userDto }
            };
        }

        [Test]
        public void ToDTO_WithPostCreateRequestAndUserDTO_ReturnsPostDTO()
        {
            var result = _postCreateRequest.ToDTO(_userDto);
            Assert.Multiple(() =>
            {
                Assert.That(result.Title, Is.EqualTo(_postCreateRequest.Title));
                Assert.That(result.Content, Is.EqualTo(_postCreateRequest.Content));
                Assert.That(result.User, Is.EqualTo(_userDto));
            });
        }

        [Test]
        public void ToDTO_WithPostUpdateRequestAndUserDTO_ReturnsPostDTO()
        {
            var result = _postUpdateRequest.ToDTO(_userDto);
            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(_postUpdateRequest.Id));
                Assert.That(result.Title, Is.EqualTo(_postUpdateRequest.Title));
                Assert.That(result.Content, Is.EqualTo(_postUpdateRequest.Content));
                Assert.That(result.User, Is.EqualTo(_userDto));
            });
        }

        [Test]
        public void ToResponse_WithPostDTO_ReturnsPostResponse()
        {
            var postDto = _postDtoList.First();
            var result = postDto.ToResponse();
            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(postDto.Id));
                Assert.That(result.Title, Is.EqualTo(postDto.Title));
                Assert.That(result.Content, Is.EqualTo(postDto.Content));
                Assert.That(result.UserId, Is.EqualTo(postDto?.User?.Id));
            });
        }

        [Test]
        public void ToResponse_WithIEnumerablePostDTO_ReturnsIEnumerablePostResponse()
        {
            var result = _postDtoList.ToResponse();
            Assert.Multiple(() =>
            {
                Assert.That(result.Count(), Is.EqualTo(_postDtoList.Count));
                Assert.That(result.First().Id, Is.EqualTo(_postDtoList.First().Id));
                Assert.That(result.First().Title, Is.EqualTo(_postDtoList.First().Title));
                Assert.That(result.First().Content, Is.EqualTo(_postDtoList.First().Content));
                Assert.That(result.First().UserId, Is.EqualTo(_postDtoList.First().User?.Id));
            });
        }
    }
}
