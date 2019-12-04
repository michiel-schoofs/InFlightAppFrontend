using InFlightApp.Configuration;
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
            string receiver = null;
            if (notification != null && !string.Empty.Equals(notification))
            {
                _model.SendNotification(notification,receiver);
                txtNotification.Text = String.Empty;
            }
        }

    }
}
