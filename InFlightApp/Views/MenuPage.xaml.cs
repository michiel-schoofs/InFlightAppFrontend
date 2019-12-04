using InFlightApp.Configuration;
using InFlightApp.View_Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace InFlightApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MenuPage : Page
    {
        private readonly NotificationsViewModel _model;

        public MenuPage()
        {
            this.InitializeComponent();
            try
            {
                _model = ServiceLocator.Current.GetService<NotificationsViewModel>(true);
                LoginViewModel lvm = ServiceLocator.Current.GetService<LoginViewModel>(true);
                logoutBtn.DataContext = lvm;
                UserStackPanel.DataContext = lvm;
                lvm.LoggedOut += Lvm_LoggedOut;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            PollNotifications();
        }

        private void Lvm_LoggedOut(){
            Frame.Navigate(typeof(MainPage));
        }

        private void NavigationViewControl_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                NavigationViewFrame.Navigate(typeof(SettingsPage));
            }
            else
            {
                NavigationViewItem itemContent = args.SelectedItem as NavigationViewItem;
                if (itemContent != null)
                {
                    switch (itemContent.Tag)
                    {
                        case "Nav_Entertainment":
                            NavigationViewFrame.Navigate(typeof(EntertainmentMainPage));
                            break;
                        case "Nav_Passengers":
                            NavigationViewFrame.Navigate(typeof(PassengersPage));
                            break;
                        case "Nav_Products":
                            NavigationViewFrame.Navigate(typeof(ProductPage));
                            break;
                        case "Nav_Notifications":
                            NavigationViewFrame.Navigate(typeof(NotificationsPage));
                            break;
                        case "Nav_Info":
                            NavigationViewFrame.Navigate(typeof(InfoPage));
                            break;
                        default: break;
                    }
                }
            }
        }

        private void PollNotifications()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(10000);
                    var result = _model.LoadMostRecentNotification();
                    if (result != null)
                    {
                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                        {
                            await CreateContentDialog(result.Content);
                        });
                    }
                }
            });
        }

        private async Task CreateContentDialog(string notification)
        {
            ContentDialog contentDialog = new ContentDialog();
            contentDialog.Title = "Notification from crew";
            contentDialog.Content = notification;
            contentDialog.CloseButtonText = "Close";
            contentDialog.DefaultButton = ContentDialogButton.Close;
            try
            {
                await contentDialog.ShowAsync();
            }
            catch (Exception ex) { _model.MostRecentNotification = null; }
        }

        private void userIcon_PointerPressed(object sender, PointerRoutedEventArgs e){
            userIcon.ContextFlyout.ShowAt(userIcon);
        }
    }
}
