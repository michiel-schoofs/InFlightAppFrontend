using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InFlightApp.Model {
    public class Message {
        public int MessageId { get; set; }
        public Persoon Sender { get; set; }
        public string Content { get; set; }
        public DateTime DateSent { get; set; }
    }
}
