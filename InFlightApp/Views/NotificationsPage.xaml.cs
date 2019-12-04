using InFlightApp.Configuration;
using InFlightApp.Model;
using InFlightApp.View_Model;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;


namespace InFlightApp.Views
{

    public sealed partial class NotificationsPage : Page
    {

        private readonly NotificationsViewModel _model;
        public NotificationsPage()
        {
            InitializeComponent();
            try
            {
                _model = ServiceLocator.Current.GetService<NotificationsViewModel>(true);
                _model.LoadNotifications();
                _model.LoadPassengers();
                DataContext = _model;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void BtnNotification_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var notification = txtNotification.Text;
            if (notification != null && !string.Empty.Equals(notification))
            {
                if (ReceiverBox.SelectedItem != null)
                {
                    var receiver = (ReceiverBox.SelectedItem as Passenger).Seat.SeatCode;
                    if (receiver.Equals("All")) { receiver = null; }
                    _model.SendNotification(notification, receiver);
                }
                else
                {
                    _model.SendNotification(notification, null);
                }
                ReceiverBox.SelectedIndex = -1;
                txtNotification.Text = String.Empty;

            }
        }

    }
}
