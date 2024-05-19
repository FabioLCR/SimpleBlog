namespace SimpleBlog.Domain.Exceptions
{
    public class PostConflictException : Exception
    {
        public PostConflictException() { }

        public PostConflictException(string message) : base(message) { }

        public PostConflictException(string message, Exception inner) : base(message, inner) { }
    }
}