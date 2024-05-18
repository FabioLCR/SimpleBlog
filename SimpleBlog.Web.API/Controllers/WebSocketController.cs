using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Application.DTOs;
using SimpleBlog.Web.API.Interfaces;
using System.Net.WebSockets;

namespace SimpleBlog.Web.API.Controllers
{
    [ApiController]
    public class WebSocketController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly IWebSocketManagerService _webSocketConnectionManager;

        public WebSocketController(
            INotificationService notificationService,
            IWebSocketManagerService webSocketConnectionManager)
        {
            _notificationService = notificationService;
            _webSocketConnectionManager = webSocketConnectionManager;
        }

        [HttpGet("/ws")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                string socketId = _webSocketConnectionManager.AddSocket(webSocket);

                await _notificationService.SendNotification(webSocket, new NotificationDTO
                {
                    Timestamp = DateTime.UtcNow,
                    PostTitle = "Bem-vindo!",
                    PostContent = "Você está conectado ao WebSocket"
                });

                // Aguarde o cliente fechar a conexão WebSocket
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(new byte[1024]), CancellationToken.None);
                while (!result.CloseStatus.HasValue)
                {
                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(new byte[1024]), CancellationToken.None);
                }

                // Quando a conexão WebSocket é fechada, remova o socket do WebSocketConnectionManager
                await _webSocketConnectionManager.RemoveSocket(socketId);
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
            }
        }

        [HttpGet("/ws/close/{socketId}")]
        public async Task Close(string socketId)
        {
            await _webSocketConnectionManager.RemoveSocket(socketId);
        }
    }
}