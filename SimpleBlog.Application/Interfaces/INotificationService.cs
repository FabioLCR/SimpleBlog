using SimpleBlog.Application.DTOs;
using System.Net.WebSockets;

namespace SimpleBlog.Application.Interfaces
{
    public interface INotificationService
    {
        Task SendNotification(string clientId, NotificationDTO notification);
        Task SendNotificationToAll(NotificationDTO notification);
    }
}
