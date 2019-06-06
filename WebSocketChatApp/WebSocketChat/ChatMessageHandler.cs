using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketChat
{
    public class ChatMessageHandler : WebSocketHandler
    {
        public ChatMessageHandler(WebSocketObjectHolder webSocketObjectHolder) : base(webSocketObjectHolder)
        {
        }

        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);

            var socketid = WebSocketObjectHolder.GetId(socket);
            await SendMessageToAllAsync($"{socketid} is now connected");
        }

        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var socketid = WebSocketObjectHolder.GetId(socket);
            var message = $"{socketid} said: {Encoding.UTF8.GetString(buffer, 0, result.Count)}";

            await SendMessageToAllAsync(message);
        }
        
        public override async Task OnDisconnected(WebSocket socket)
        {
            var socketId = WebSocketObjectHolder.GetId(socket);

            await base.OnDisconnected(socket);
            await SendMessageToAllAsync($"{socketId} disconnected");
        }
    }
}
