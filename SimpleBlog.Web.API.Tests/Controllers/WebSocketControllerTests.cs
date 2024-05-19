using Microsoft.AspNetCore.Http;
using Moq;
using SimpleBlog.Application.DTOs;
using SimpleBlog.Application.Interfaces;
using SimpleBlog.Web.API.Controllers;
using SimpleBlog.Web.API.Interfaces;
using System.Net.WebSockets;

namespace SimpleBlog.Web.API.Tests.Controllers
{
    public class WebSocketControllerTests
    {
        private Mock<INotificationService> _notificationServiceMock;
        private Mock<IWebSocketManagerService> _webSocketManagerServiceMock;
        private Mock<IWebSocketHandlerFactory> _webSocketHandlerFactoryMock;
        private WebSocketController _webSocketController;

        [SetUp]
        public void Setup()
        {
            _notificationServiceMock = new Mock<INotificationService>();
            _webSocketManagerServiceMock = new Mock<IWebSocketManagerService>();
            _webSocketHandlerFactoryMock = new Mock<IWebSocketHandlerFactory>();
            _webSocketController = new WebSocketController(_notificationServiceMock.Object, _webSocketManagerServiceMock.Object, _webSocketHandlerFactoryMock.Object);
        }

        [Test]
        public async Task Get_WhenWebSocketRequest_AddsSocketAndSendsNotification()
        {
            // Configura o HttpContext para ser um WebSocketRequest
            var contextMock = new Mock<HttpContext>();
            contextMock.SetupGet(x => x.WebSockets.IsWebSocketRequest).Returns(true);

            var webSocketHandlerMock = new Mock<IWebSocketHandler>();
            webSocketHandlerMock.Setup(ws => ws.State).Returns(WebSocketState.Open);
            _webSocketHandlerFactoryMock.Setup(x => x.Create(It.IsAny<HttpContext>())).Returns(webSocketHandlerMock.Object);

            _webSocketController.ControllerContext.HttpContext = contextMock.Object;

            await _webSocketController.Get();

            _webSocketManagerServiceMock.Verify(x => x.AddSocket(webSocketHandlerMock.Object), Times.Once);
            _notificationServiceMock.Verify(x => x.SendNotification(It.IsAny<string>(), It.IsAny<NotificationDTO>()), Times.Once);
        }

        [Test]
        public async Task Close_CallsRemoveSocket()
        {
            var socketId = "testSocketId";

            await _webSocketController.Close(socketId);

            _webSocketManagerServiceMock.Verify(x => x.RemoveSocket(socketId), Times.Once);
        }
    }
}
