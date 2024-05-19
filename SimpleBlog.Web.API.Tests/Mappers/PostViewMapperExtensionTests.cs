using SimpleBlog.Application.Mappers;
using SimpleBlog.Domain.Entities;
using SimpleBlog.Web.API.ViewModels;

namespace SimpleBlog.Application.Tests.Mappers
{
    public class PostViewMapperExtensionTests
    {
        [Test]
        public void ToDTO_ConvertsPostCreateRequestToPostDTO()
        {
            var request = new PostCreateRequest { Title = "Test Title", Content = "Test Content" };
            var user = new UserDTO { Id = 1, Username = "Test User" };

            var dto = request.ToDTO(user);
            Assert.Multiple(() =>
            {
                Assert.That(dto.Title, Is.EqualTo(request.Title));
                Assert.That(dto.Content, Is.EqualTo(request.Content));
                Assert.That(dto.User, Is.EqualTo(user));
            });
        }

        [Test]
        public void ToDTO_ConvertsPostUpdateRequestToPostDTO()
        {
            var request = new PostUpdateRequest { Id = 1, Title = "Test Title", Content = "Test Content" };
            var user = new UserDTO { Id = 1, Username = "Test User" };

            var dto = request.ToDTO(user);
            Assert.Multiple(() =>
            {
                Assert.That(dto.Id, Is.EqualTo(request.Id));
                Assert.That(dto.Title, Is.EqualTo(request.Title));
                Assert.That(dto.Content, Is.EqualTo(request.Content));
                Assert.That(dto.User, Is.EqualTo(user));
            });
        }

        [Test]
        public void ToResponse_ConvertsPostDTOToPostResponse()
        {
            var dto = new PostDTO { Id = 1, Title = "Test Title", Content = "Test Content", User = new UserDTO { Id = 1, Username = "Test User" } };

            var response = dto.ToResponse();
            Assert.Multiple(() =>
            {
                Assert.That(response.Id, Is.EqualTo(dto.Id));
                Assert.That(response.Title, Is.EqualTo(dto.Title));
                Assert.That(response.Content, Is.EqualTo(dto.Content));
                Assert.That(response.UserId, Is.EqualTo(dto.User.Id));
            });
        }

        [Test]
        public void ToResponse_ConvertsIEnumerablePostDTOToIEnumerablePostResponse()
        {
            var dtos = new List<PostDTO>
            {
                new() { Id = 1, Title = "Test Title 1", Content = "Test Content 1", User = new UserDTO { Id = 1, Username = "Test User 1" } },
                new() { Id = 2, Title = "Test Title 2", Content = "Test Content 2", User = new UserDTO { Id = 2, Username = "Test User 2" } }
            };

            var responses = dtos.ToResponse().ToList();

            Assert.That(responses.Count, Is.EqualTo(dtos.Count));

            for (int i = 0; i < dtos.Count; i++)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(responses[i].Id, Is.EqualTo(dtos[i].Id));
                    Assert.That(responses[i].Title, Is.EqualTo(dtos[i].Title));
                    Assert.That(responses[i].Content, Is.EqualTo(dtos[i].Content));
                    Assert.That(responses[i].UserId, Is.EqualTo(dtos[i].User?.Id));
                });
            }
        }
    }
}
