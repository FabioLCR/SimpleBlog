using Microsoft.Extensions.Logging;
using Moq;
using SimpleBlog.Web.API.Interfaces;
using SimpleBlog.Web.API.Services.WebSockets;
using System.Net.WebSockets;

namespace SimpleBlog.Web.API.Tests.Services
{
    public class WebSocketManagerServiceTests
    {
        private Mock<ILogger<WebSocketManagerService>> _loggerMock;
        private WebSocketManagerService _webSocketManagerService;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<WebSocketManagerService>>();
            _webSocketManagerService = new WebSocketManagerService(_loggerMock.Object);
        }

        [Test]
        public void AddSocket_AddsSocketToCollection()
        {
            var socketMock = new Mock<IWebSocketHandler>();

            var id = _webSocketManagerService.AddSocket(socketMock.Object);

            var sockets = _webSocketManagerService.GetSockets();

            Assert.That(sockets, Has.Exactly(1).EqualTo(new KeyValuePair<string, IWebSocketHandler>(id, socketMock.Object)));
        }

        [Test]
        public async Task RemoveSocket_RemovesSocketFromCollection()
        {
            var socketMock = new Mock<IWebSocketHandler>();
            socketMock.Setup(s => s.CloseAsync(It.IsAny<WebSocketCloseStatus>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                      .Returns(Task.CompletedTask);

            var id = _webSocketManagerService.AddSocket(socketMock.Object);

            await _webSocketManagerService.RemoveSocket(id);

            var sockets = _webSocketManagerService.GetSockets();

            Assert.That(sockets, Is.Empty);
        }
    }
}
