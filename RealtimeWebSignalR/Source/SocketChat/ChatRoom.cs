using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SocketChat
{
    public class ChatRoom
    {
        private readonly ConcurrentBag<Participant> participants = new ConcurrentBag<Participant>();

        public void Join(Participant participant)
        {
            participant.JoinRoom(this);
            participant.MessageReceived += OnParticipantMessageReceived;
            participant.Disconnected += OnParticipantDisconnected;

            this.participants.Add(participant);
        }

        private void OnParticipantMessageReceived(Participant participant, string message)
        {
            var data = new Message { Name = participant.Name, Body = message };
            foreach (var otherParticipant in this.participants.Except(new[] { participant }))
            {
                otherParticipant.SendMessage(data);
            }
        }

        private void OnParticipantDisconnected(Participant participant)
        {
            var message = new Message { Name = participant.Name, Body = "So long and thanks for all of the shoes!" };
            foreach (var otherParticipant in this.participants.Except(new[] { participant }))
            {
                otherParticipant.SendMessage(message);
            }
        }
    }
}