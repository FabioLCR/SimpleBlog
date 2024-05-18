using System.Net.WebSockets;

namespace SimpleBlog.Web.API.Interfaces
{
    public interface IWebSocketHandler
    {
        Task Echo(WebSocket webSocket);
    }
}