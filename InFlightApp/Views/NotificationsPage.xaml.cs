using InFlightApp.Configuration;
using InFlightApp.View_Model;
using Microsoft.AspNet.SignalR.Client;
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
            this.InitializeComponent();
            RunProxy();

            try
            {
                _model = ServiceLocator.Current.GetService<NotificationsViewModel>(true);
                DataContext = _model;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void RunProxy()
        {
            Task.Run(() => ConnectToNotificationHub());
        }

        public async Task ConnectToNotificationHub()
        {
            using (var hubConnection = new HubConnection("https://localhost:44355/"))
            {
                IHubProxy notificationHubProxy = hubConnection.CreateHubProxy("NotificationHub");
                notificationHubProxy.On<string>("ReceiveNotification", notification => CreateContentDialog(notification));
                await hubConnection.Start();
            }
        }

        private void CreateContentDialog(string notification)
        {
            ContentDialog contentDialog = new ContentDialog();
            contentDialog.Title = "Notification from crew";
            contentDialog.Content = notification;
            contentDialog.CloseButtonText = "Close";
            contentDialog.DefaultButton = ContentDialogButton.Close;
            //contentDialog.ShowAsync();
        }

        private void BtnNotification_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var notification = txtNotification.Text;
            if (notification != null && !string.Empty.Equals(notification))
            {
                _model.SendNotification(notification);
            }
        }
    }
}
