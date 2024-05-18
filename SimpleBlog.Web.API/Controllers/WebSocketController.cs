using Microsoft.AspNetCore.Mvc;
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

                await _notificationService.SendNotification(webSocket, "Conexão estabelecida");

                // Agora você pode usar o socketId para referenciar este socket em particular

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