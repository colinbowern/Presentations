using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SignalR.Hubs;

namespace SignalRChat
{
    [HubName("Chat")]
    public class ChatHub : Hub, IConnected, IDisconnect
    {
        private static readonly ConcurrentDictionary<string, Participant> participants = new ConcurrentDictionary<string, Participant>();

        public Task Connect()
        {
            var participant = new Participant
            {
                Id = Context.ConnectionId,
                ConnectionType = Context.QueryString["transport"]
            };
            participants.AddOrUpdate(participant.Id, participant, (s, p) => participant);
            return RefreshStats();
        }

        public Task Reconnect(IEnumerable<string> groups)
        {
            var participant = new Participant
            {
                Id = Context.ConnectionId,
                ConnectionType = Context.QueryString["transport"]
            };
            participants.AddOrUpdate(participant.Id, participant, (s, p) => participant);
            return RefreshStats();
        }

        public Task Disconnect()
        {
            Participant participant;
            participants.TryRemove(Context.ConnectionId, out participant);
            return RefreshStats();
        }

        public void Join(string name)
        {
            var participant = participants[Context.ConnectionId] ?? new Participant() { Id = Context.ConnectionId };
            participant.Name = name;
            participant.ConnectionType = Context.QueryString["transport"];
            participants.AddOrUpdate(participant.Id, participant, (s, p) => participant);
            RefreshStats();
        }

        public void SendMessage(string message)
        {
            var data = JsonConvert.SerializeObject(new { Name = participants[Context.ConnectionId].Name, Body = message });
            foreach (var otherParticipant in participants.Where(x => x.Key != Context.ConnectionId))
            {
                Clients[otherParticipant.Key].broadcast(data);
            }
        }

        private Task RefreshStats()
        {
            var stats = participants.GroupBy(x => x.Value.ConnectionType)
                                    .Select(x => new { transport = x.First().Value.ConnectionType, count = x.Count() });
            var data = JsonConvert.SerializeObject(stats);
            return Clients.refreshStats(data);
        }
    }
}