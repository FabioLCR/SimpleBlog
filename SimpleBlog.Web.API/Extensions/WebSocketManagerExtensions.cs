using SimpleBlog.Web.API.Interfaces;
using SimpleBlog.Web.API.Services;
using System.Net.WebSockets;

namespace SimpleBlog.Web.API.Extensions
{
    public static class WebSocketManagerExtensions
    {
        public static IApplicationBuilder UseWebSocketManager(
            this IApplicationBuilder app, 
            IConfiguration config,
            IWebSocketHandler webSocketHandler)
        {
            var webSocketPath = config.GetSection("WebSocket:Path").Value;

            app.UseWebSockets();

            app.Use(async (context, next) =>
            {
                if (context.Request.Path == webSocketPath)
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

                        await webSocketHandler.Echo(webSocket);
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                }
                else
                {
                    await next();
                }
            });

            return app;
        }
    }
}
