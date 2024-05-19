using SimpleBlog.Web.API.Interfaces;
using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace SimpleBlog.Web.API.Services.WebSockets
{
    public class WebSocketManagerService : IWebSocketManagerService
    {
        private readonly ConcurrentDictionary<string, IWebSocketHandler> _sockets = new();
        private readonly ILogger<WebSocketManagerService> _logger;

        public WebSocketManagerService(ILogger<WebSocketManagerService> logger)
        {
            _logger = logger;
        }

        public IWebSocketHandler? GetSocketById(string id)
        {
            _sockets.TryGetValue(id, out var socket);
            return socket;
        }

        public IEnumerable<KeyValuePair<string, IWebSocketHandler>> GetSockets() => _sockets;

        public string AddSocket(IWebSocketHandler socket)
        {
            string id = Guid.NewGuid().ToString();
            _sockets.TryAdd(id, socket);

            _logger.LogInformation($"Socket adicionado com ID: {id}");

            return id;
        }

        public async Task RemoveSocket(string id)
        {
            _sockets.TryRemove(id, out var socket);
            await socket!.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                    statusDescription: "Fechado pelo WebSocketManager",
                                    cancellationToken: CancellationToken.None);

            _logger.LogInformation($"Socket removido com ID: {id}");
        }
    }
}
