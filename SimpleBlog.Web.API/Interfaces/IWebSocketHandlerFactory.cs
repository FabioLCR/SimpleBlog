namespace SimpleBlog.Web.API.Interfaces
{
    public interface IWebSocketHandlerFactory
    {
        IWebSocketHandler Create(HttpContext context);
    }
}