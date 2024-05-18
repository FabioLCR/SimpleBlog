using SimpleBlog.Domain.Entities;

namespace SimpleBlog.Web.API.ViewModels
{
    public class PostUpdateRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}