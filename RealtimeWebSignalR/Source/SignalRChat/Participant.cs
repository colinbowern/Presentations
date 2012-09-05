using System;
using System.Collections.Generic;
using System.Linq;

namespace SignalRChat
{
    public class Participant
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ConnectionType { get; set; }
    }
}