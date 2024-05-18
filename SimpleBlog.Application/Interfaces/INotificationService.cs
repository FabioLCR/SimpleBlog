using System.Net.WebSockets;

namespace SimpleBlog.Web.API.Interfaces
{
    public interface INotificationService
    {
        Task SendNotification(WebSocket socket, string message);
        Task SendNotificationToAll(string message);
    }
}
