using System.Net.WebSockets;

namespace SimpleBlog.Web.API.Interfaces
{
    public interface IWebSocketManagerService
    {
        IEnumerable<KeyValuePair<string, WebSocket>> GetSockets();
        string AddSocket(WebSocket socket);
        Task RemoveSocket(string id);
    }
}
