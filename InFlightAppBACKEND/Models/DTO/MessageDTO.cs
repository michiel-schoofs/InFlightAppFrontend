using InFlightAppBACKEND.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InFlightAppBACKEND.Models.DTO {
    public class MessageDTO {

        public int MessageId { get; set; }
        public PersonDTO Sender { get; set; }
        public string Content { get; set; }
        public string DateSent { get; set; }


        public MessageDTO() {}
        public MessageDTO(Message message) {
            MessageId = message.MessageId;
            Sender = new PersonDTO(message.Sender);
            Content = message.Content;
            DateSent = message.DateSent.ToString("dd/MM/yyyy HH:mm:ss");
        }
    }
}
