namespace SimpleBlog.Domain.Exceptions
{
    public class PostNotFoundException : Exception
    {
        public PostNotFoundException() { }

        public PostNotFoundException(string message) : base(message) { }

        public PostNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}