using System.Diagnostics.CodeAnalysis;

namespace SimpleBlog.Application.DTOs
{
    [ExcludeFromCodeCoverage]
    public class NotificationDTO
    {
        public string? Username { get; set; }
        public string? PostTitle { get; set; }
        public string? PostContent { get; set; }
        public DateTime Timestamp { get; set; }
    }
}