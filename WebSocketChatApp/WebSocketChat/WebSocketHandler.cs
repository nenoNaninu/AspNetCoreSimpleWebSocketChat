using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketChat
{
    public abstract class WebSocketHandler
    {
        protected WebSocketObjectHolder WebSocketObjectHolder { get; set; }

        public WebSocketHandler(WebSocketObjectHolder webSocketObjectHolder)
        {
            WebSocketObjectHolder = webSocketObjectHolder;
        }

        public virtual async Task OnConnected(WebSocket socket)
        {
            WebSocketObjectHolder.AddSocket(socket);
        }

        public virtual async Task OnDisconnected(WebSocket socket)
        {
            await WebSocketObjectHolder.RemoveSocket(WebSocketObjectHolder.GetId(socket));
        }

        public async Task SendMessageAsync(WebSocket socket, string message)
        {
            if(socket.State != WebSocketState.Open) return;

            var buffer = Encoding.UTF8.GetBytes(message);

            await socket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true,
                CancellationToken.None);

        }

        public async Task SendMessageAsync(string socketId, string message)
        {
           await SendMessageAsync(WebSocketObjectHolder.GetSocketById(socketId), message);
        }

        public async Task SendMessageToAllAsync(string message)
        {
            foreach (var pair in WebSocketObjectHolder.GetAll())
            {
                if (pair.Value.State == WebSocketState.Open)
                {
                    await SendMessageAsync(pair.Value, message);
                }
            }
        }

        public abstract Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
    }
}
