namespace SimpleBlog.Domain.Exceptions
{
    public class UserNotAuthorizedException : Exception
    {
        public UserNotAuthorizedException() { }

        public UserNotAuthorizedException(string message) : base(message) { }

        public UserNotAuthorizedException(string message, Exception inner) : base(message, inner) { }
    }
}