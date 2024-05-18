using Newtonsoft.Json;
using SimpleBlog.Application.DTOs;
using SimpleBlog.Web.API.Interfaces;
using System.Net.WebSockets;
using System.Text;

namespace SimpleBlog.Web.API.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IWebSocketManagerService _webSocketManager;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            IWebSocketManagerService webSocketManager, 
            ILogger<NotificationService> logger)
        {
            _webSocketManager = webSocketManager;
            _logger = logger;
        }

        public async Task SendNotification(WebSocket socket, NotificationDTO notification)
        {
            if (socket.State != WebSocketState.Open)
                return;

            var message = JsonConvert.SerializeObject(notification);

            _logger.LogInformation($"Enviando notificação para o socket: {message}");

            var messageBytes = Encoding.UTF8.GetBytes(message);

            await socket.SendAsync(new ArraySegment<byte>(array: messageBytes,
                                                          offset: 0,
                                                          count: messageBytes.Length),
                                           messageType: WebSocketMessageType.Text,
                                   endOfMessage: true,
                                   cancellationToken: CancellationToken.None);
        }

        public async Task SendNotificationToAll(NotificationDTO notification)
        {
            _logger.LogInformation("Enviando notificação para todos os sockets");

            foreach (var pair in _webSocketManager.GetSockets())
            {
                if (pair.Value.State == WebSocketState.Open)
                    await SendNotification(pair.Value, notification);
            }
        }
    }
}
