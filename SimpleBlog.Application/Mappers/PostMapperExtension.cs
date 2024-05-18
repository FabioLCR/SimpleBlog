using SimpleBlog.Application.DTOs;
using SimpleBlog.Domain.Entities;

namespace SimpleBlog.Application.Mappers
{
    public static class PostMapperExtension
    {
        public static IEnumerable<PostDTO> ToDTO(this IEnumerable<PostEntity> entity) => 
            entity.Select(ToDTO);

        public static PostDTO ToDTO(this PostEntity entity) => new()
        {
            Id = entity.Id,
            Title = entity.Title,
            Content = entity.Content,
            User = entity.User.ToDTO()
        };

        public static IEnumerable<PostEntity> ToEntity(this IEnumerable<PostDTO> dto) => 
            dto.Select(ToEntity);

        public static PostEntity ToEntity(this PostDTO dto) => new()
        {
            Id = dto.Id,
            Title = dto.Title,
            Content = dto.Content,
            UserId = dto.User.Id
        };

        public static NotificationDTO ToNotificationDTO(this PostDTO dto) => new()
        {
            Timestamp = DateTime.Now,
            PostTitle = dto.Title,
            PostContent = dto.Content,
            Username = dto.User.Username!
        };
            
    }
}
