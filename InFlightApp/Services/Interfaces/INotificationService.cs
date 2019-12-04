using InFlightApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InFlightApp.Services.Interfaces
{
    public interface INotificationService
    {
        void SendNotification(string notification,string receiver);
        IEnumerable<Notification> GetAllNotifications();
        Notification GetMostRecentNotification();
    }
}
