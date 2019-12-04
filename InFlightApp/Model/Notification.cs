using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace InFlightApp.Model
{
    public class Notification
    {

        public string Content { get; set; }

        public DateTime Timestamp { get; set; }

        public string Receiver { get; set; }
    }
}
