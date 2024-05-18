using SimpleBlog.Application.DTOs;
using System.Net.WebSockets;

namespace SimpleBlog.Web.API.Interfaces
{
    public interface INotificationService
    {
        Task SendNotification(WebSocket socket, NotificationDTO notification);
        Task SendNotificationToAll(NotificationDTO notification);
    }
}
