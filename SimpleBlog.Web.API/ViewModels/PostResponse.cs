namespace SimpleBlog.Web.API.ViewModels
{
    public class PostResponse
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public int? UserId { get; set; }
    }
}