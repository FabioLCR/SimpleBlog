using SimpleBlog.Web.API.Interfaces;
using System.Net.WebSockets;

namespace SimpleBlog.Web.API.Services.WebSockets
{
    public class WebSocketHandlerFactory : IWebSocketHandlerFactory
    {
        public IWebSocketHandler Create(HttpContext context)
        {
            WebSocket webSocket = context.WebSockets.AcceptWebSocketAsync().Result;
            return new WebSocketHandler(webSocket);
        }
    }
}