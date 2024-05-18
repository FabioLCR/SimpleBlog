namespace SimpleBlog.Application.DTOs
{
    public class NotificationDTO
    {
        public string Username { get; set; }
        public string PostTitle { get; set; }
        public string PostContent { get; set; }
        public DateTime Timestamp { get; set; }
    }
}