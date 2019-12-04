using InFlightApp.Configuration;
using InFlightApp.Model;
using InFlightApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace InFlightApp.View_Model
{
    public class NotificationsViewModel
    {
        private readonly INotificationService _notificationService;
        public ObservableCollection<Notification> Notifications { get; set; }
        public Notification MostRecentNotification { get; set; }

        public ObservableCollection<Passenger> Passengers { get; set; }
        private readonly IUserService _userInterface;

        public NotificationsViewModel()
        {
            try
            {
                _notificationService = ServiceLocator.Current.GetService<INotificationService>(true);
                _userInterface = ServiceLocator.Current.GetService<IUserService>(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void LoadNotifications()
        {
            Notifications = new ObservableCollection<Notification>(_notificationService.GetAllNotifications().OrderByDescending(n => n.Timestamp));
        }

        public Notification LoadMostRecentNotification()
        {

            var notification = _notificationService.GetMostRecentNotification();
            if (MostRecentNotification == null || MostRecentNotification.Timestamp != notification.Timestamp)
            {
                MostRecentNotification = notification;
                return MostRecentNotification;
            }
            return null;
        }

        public void SendNotification(string notification, string receiver)
        {
            _notificationService.SendNotification(notification, receiver);
            Notifications.Insert(0, new Notification() { Content = notification, Timestamp = DateTime.Now, Receiver = receiver });
        }

        public void LoadPassengers()
        {
            Passengers = new ObservableCollection<Passenger>(_userInterface.GetPassengers().OrderBy(p=>p.Seat.SeatCode).ToList());
            Passengers.Insert(0, new Passenger() { FirstName = "All", LastName = "Passengers", Seat = new Seat() { SeatCode = "All" } });
        }


    }
}
