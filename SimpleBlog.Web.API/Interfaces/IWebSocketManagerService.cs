using System.Net.WebSockets;

namespace SimpleBlog.Web.API.Interfaces
{
    public interface IWebSocketManagerService
    {
        IWebSocketHandler? GetSocketById(string id);
        IEnumerable<KeyValuePair<string, IWebSocketHandler>> GetSockets();
        string AddSocket(IWebSocketHandler socket);
        Task RemoveSocket(string id);
    }
}
