using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.WebSockets;
using Newtonsoft.Json;

namespace SocketChat
{
    public class Participant
    {
        private AspNetWebSocketContext context;

        public event Action<Participant, string> MessageReceived;
        public event Action<Participant> Disconnected;

        public string Name { get; private set; }

        public Participant(string name)
        {
            this.Name = name;
        }

        public async Task Receiver(AspNetWebSocketContext context)
        {
            this.context = context;
            var socket = this.context.WebSocket as AspNetWebSocket;

            var inputBuffer = new ArraySegment<byte>(new byte[1024]);

            try
            {
                while (true)
                {
                    var result = await socket.ReceiveAsync(inputBuffer, CancellationToken.None);
                    if (socket.State != WebSocketState.Open)
                    {
                        if (this.Disconnected != null)
                        {
                            this.Disconnected(this);
                        }
                        break;
                    }
                    var message = Encoding.UTF8.GetString(inputBuffer.Array, 0, result.Count);
                    if (this.MessageReceived != null)
                    {
                        this.MessageReceived(this, message);
                    }
                }
            }
            catch (Exception)
            {
                if (this.Disconnected != null)
                {
                    this.Disconnected(this);
                }
                throw;
            }
        }

        public async Task SendMessage(object message)
        {
            var messageString = JsonConvert.SerializeObject(message);
            if (this.context != null && this.context.WebSocket.State == WebSocketState.Open)
            {
                var outputBuffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(messageString));
                await this.context.WebSocket.SendAsync(outputBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        public void Close()
        {
            if (this.context != null && this.context.WebSocket.State == WebSocketState.Open)
            {
                this.context.WebSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure,
                    "Closing...", CancellationToken.None).Wait();
            }
        }
    }
}