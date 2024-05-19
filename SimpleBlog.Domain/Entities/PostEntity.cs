namespace SimpleBlog.Domain.Entities
{
    public class PostEntity
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public int? UserId { get; set; }
        public UserEntity? User { get; set; }

        public bool CanEdit(UserEntity user) => UserId == user.Id;
    }
}