using System.Net.WebSockets;

namespace SimpleBlog.Web.API.Interfaces
{
    public interface IWebSocketHandler
    {
        Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken);
        Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken);
        Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken);
        WebSocketState State { get; }

    }
}