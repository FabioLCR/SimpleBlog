using SimpleBlog.Domain.Entities;
using SimpleBlog.Web.API.ViewModels;

namespace SimpleBlog.Application.Mappers
{
    public static class PostViewMapperExtension
    {
        public static PostDTO ToDTO(this PostCreateRequest request, UserDTO user) => new()
        {
            Title = request.Title,
            Content = request.Content,
            User = user
        };

        
        public static PostDTO ToDTO(this PostUpdateRequest entity, UserDTO user) => new()
        {
            Id = entity.Id,
            Title = entity.Title,
            Content = entity.Content,
            User = user
        };

        public static IEnumerable<PostResponse> ToResponse(this IEnumerable<PostDTO> dto) => 
            dto.Select(ToResponse);

        public static PostResponse ToResponse(this PostDTO dto) => new()
        {
            Id = dto.Id,
            Title = dto?.Title,
            Content = dto?.Content,
            UserId = dto?.User?.Id
        };
    }
}
