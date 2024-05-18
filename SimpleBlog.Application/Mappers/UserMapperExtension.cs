using SimpleBlog.Domain.Entities;

namespace SimpleBlog.Application.Mappers
{
    public static class UserMapperExtension
    {
        public static UserDTO ToDTO(this UserEntity entity) => new()
        {
            Id = entity.Id,
            Username = entity.Username,
            Password = entity.Password
        };

        public static UserEntity ToEntity(this UserDTO dto) => new()
        {
            Id = dto.Id,
            Username = dto.Username,
            Password = dto.Password
        };
    }
}
