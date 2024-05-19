using System.Diagnostics.CodeAnalysis;

namespace SimpleBlog.Domain.Entities
{
    [ExcludeFromCodeCoverage]
    public class UserDTO
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
