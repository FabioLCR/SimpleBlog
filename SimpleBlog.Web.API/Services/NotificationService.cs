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

        public async Task SendNotification(WebSocket socket, string message)
        {
            if (socket.State != WebSocketState.Open)
                return;

            _logger.LogInformation($"Enviando notificação para o socket: {message}");

            await socket.SendAsync(new ArraySegment<byte>(array: Encoding.ASCII.GetBytes(message),
                                                          offset: 0,
                                                          count: message.Length),
                                   messageType: WebSocketMessageType.Text,
                                   endOfMessage: true,
                                   cancellationToken: CancellationToken.None);
        }

        public async Task SendNotificationToAll(string message)
        {
            _logger.LogInformation("Enviando notificação para todos os sockets");

            foreach (var pair in _webSocketManager.GetSockets())
            {
                if (pair.Value.State == WebSocketState.Open)
                    await SendNotification(pair.Value, message);
            }
        }
    }
}
