using InFlightApp.Configuration;
using InFlightApp.Model;
using InFlightApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace InFlightApp.View_Model
{
    public class NotificationsViewModel
    {
        private readonly INotificationService _notificationService;
        public ObservableCollection<Notification> Notifications { get; set; }
        public Notification MostRecentNotification { get; set; }


        public NotificationsViewModel()
        {
            try
            {
                _notificationService = ServiceLocator.Current.GetService<INotificationService>(true);
                LoadNotifications();
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

        public void LoadMostRecentNotification()
        {
            var notification = _notificationService.GetMostRecentNotification();
            if (MostRecentNotification == null || MostRecentNotification.Timestamp != notification.Timestamp)
            {
                MostRecentNotification = notification;
                // Doesn't work because not activated from a page => doesn't know where to draw
                // CreateContentDialog(MostRecentNotification.Content);
            }
        }

        public void SendNotification(string notification)
        {
            _notificationService.SendNotification(notification);
        }

        public void CreateContentDialog(string notification)
        {
            Task.Run(async () =>
            {
                ContentDialog contentDialog = new ContentDialog();
                contentDialog.Title = "Notification from crew";
                contentDialog.Content = notification;
                contentDialog.CloseButtonText = "Close";
                contentDialog.DefaultButton = ContentDialogButton.Close;
                await contentDialog.ShowAsync();
            });
        }

    }
}
