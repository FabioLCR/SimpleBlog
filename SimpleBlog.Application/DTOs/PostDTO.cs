using System.Diagnostics.CodeAnalysis;

namespace SimpleBlog.Domain.Entities
{
    [ExcludeFromCodeCoverage]
    public class PostDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public UserDTO? User { get; set; }
    }
}
