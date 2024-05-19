using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Application.DTOs;
using SimpleBlog.Application.Interfaces;
using SimpleBlog.Web.API.Interfaces;
using System.Net.WebSockets;

namespace SimpleBlog.Web.API.Controllers
{
    [ApiController]
    public class WebSocketController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly IWebSocketManagerService _webSocketConnectionManager;
        private readonly IWebSocketHandlerFactory _webSocketHandlerFactory;

        public WebSocketController(
            INotificationService notificationService,
            IWebSocketManagerService webSocketConnectionManager,
            IWebSocketHandlerFactory webSocketHandlerFactory)
        {
            _notificationService = notificationService;
            _webSocketConnectionManager = webSocketConnectionManager;
            _webSocketHandlerFactory = webSocketHandlerFactory;
        }

        [HttpGet("/ws")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                IWebSocketHandler webSocketHandler = _webSocketHandlerFactory.Create(HttpContext);
                string socketId = _webSocketConnectionManager.AddSocket(webSocketHandler);

                await _notificationService.SendNotification(socketId, new NotificationDTO
                {
                    Timestamp = DateTime.UtcNow,
                    PostTitle = "Bem-vindo!",
                    PostContent = "Você está conectado ao WebSocket"
                });

                WebSocketReceiveResult result = await webSocketHandler.ReceiveAsync(new ArraySegment<byte>(new byte[1024]), CancellationToken.None);

                while (result != null && !result.CloseStatus.HasValue)
                {
                    result = await webSocketHandler.ReceiveAsync(new ArraySegment<byte>(new byte[1024]), CancellationToken.None);
                }

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