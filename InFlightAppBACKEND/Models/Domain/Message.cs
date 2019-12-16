using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InFlightAppBACKEND.Models.Domain
{
    public class Message
    {
        public int MessageId { get; set; }
        public int ConversationId { get; set; }
        public string Content { get; set; }
        public DateTime DateSent { get; set; }

        public Passenger Sender { get; set; }
        public int SenderId { get; set; }

        protected Message()
        {

        }

        public Message(Conversation conversation, Passenger sender, string content)
        {
            ConversationId = conversation.ConversationId;
            Sender = sender;
            Content = content;
            DateSent = DateTime.Now;
        }
    }
}
