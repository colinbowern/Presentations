using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocketChat
{
    public class Chat : IHttpHandler
    {
        private static readonly ConcurrentBag<Participant> participants = new ConcurrentBag<Participant>();

        public void ProcessRequest(HttpContext context)
        {
            if (context.IsWebSocketRequest)
            {
                var name = context.Request.QueryString["Name"];
                var participant = new Participant(name);
                participant.MessageReceived += OnParticipantMessageReceived;
                participant.Disconnected += OnParticipantDisconnected;

                participants.Add(participant);

                context.AcceptWebSocketRequest(participant.Receiver);
            }
        }

        private void OnParticipantMessageReceived(Participant participant, string message)
        {
            var data = new { Name = participant.Name, Body = message };
            foreach (var otherParticipant in participants.Except(new[] { participant }))
            {
                otherParticipant.SendMessage(data);
            }
        }

        private void OnParticipantDisconnected(Participant participant)
        {
            var message = new { Name = participant.Name, Body = "So long and thanks for all of the shoes!" };
            foreach (var otherParticipant in participants.Except(new[] { participant }))
            {
                otherParticipant.SendMessage(message);
            }
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}