using Microsoft.Extensions.Logging;
using Moq;
using SimpleBlog.Application.DTOs;
using SimpleBlog.Web.API.Interfaces;
using SimpleBlog.Web.API.Services;
using System.Net.WebSockets;

namespace SimpleBlog.Web.API.Tests.Services
{
    public class NotificationServiceTests
    {
        private Mock<IWebSocketManagerService> _webSocketManagerMock;
        private Mock<ILogger<NotificationService>> _loggerMock;
        private NotificationService _notificationService;

        [SetUp]
        public void Setup()
        {
            _webSocketManagerMock = new Mock<IWebSocketManagerService>();
            _loggerMock = new Mock<ILogger<NotificationService>>();
            _notificationService = new NotificationService(_webSocketManagerMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task SendNotificationToAll_CallsSendNotification_ForEachOpenSocket()
        {
            // Cria mocks para os WebSockets
            var socketMock1 = new Mock<IWebSocketHandler>();
            var socketMock2 = new Mock<IWebSocketHandler>();

            // Configura os WebSockets para estarem no estado Open
            socketMock1.Setup(s => s.State).Returns(WebSocketState.Open);
            socketMock2.Setup(s => s.State).Returns(WebSocketState.Open);

            var sockets = new List<KeyValuePair<string, IWebSocketHandler>>
            {
                new KeyValuePair<string, IWebSocketHandler>("socket1", socketMock1.Object),
                new KeyValuePair<string, IWebSocketHandler>("socket2", socketMock2.Object)
            };

            _webSocketManagerMock.Setup(w => w.GetSockets()).Returns(sockets);

            var notification = new NotificationDTO { PostContent = "Test message" };

            await _notificationService.SendNotificationToAll(notification);

            // Verifica se GetSockets foi chamado uma vez
            _webSocketManagerMock.Verify(w => w.GetSockets(), Times.Once);

            // Verifica se SendAsync foi chamado uma vez para cada WebSocket
            socketMock1.Verify(s => s.SendAsync(It.IsAny<ArraySegment<byte>>(), WebSocketMessageType.Text, true, CancellationToken.None), Times.Once);
            socketMock2.Verify(s => s.SendAsync(It.IsAny<ArraySegment<byte>>(), WebSocketMessageType.Text, true, CancellationToken.None), Times.Once);
        }

    }
}
