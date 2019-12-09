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
        private readonly LoginViewModel _userModel;
        private CancellationTokenSource source;
        private Task task;
        private static bool ensureonetime = false;

        public MenuPage()
        {
            this.InitializeComponent();
            try
            {
                _model = ServiceLocator.Current.GetService<NotificationsViewModel>(true);
                _userModel = ServiceLocator.Current.GetService<LoginViewModel>(true);
                logoutBtn.DataContext = _userModel;
                UserStackPanel.DataContext = _userModel;
                _userModel.LoggedOut += Lvm_LoggedOut;

                source = new CancellationTokenSource();
                ensureonetime = false;
                this.Dispatcher.RunAsync(Dispatcher.CurrentPriority, () => {
                    _userModel.GetUserImage();
                });

                NavigateToInfoPage();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            PollNotifications();
        }

        private void Lvm_LoggedOut(){
            if(!ensureonetime)
                Frame.Navigate(typeof(MainPage));
            ensureonetime = true;
        }

        private void NavigationViewControl_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args) {
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
                        default: break;
                    }
                }
        }

        private void PollNotifications()
        {
            var loggedInPassenger = _userModel.GetLoggedIn();
            if (loggedInPassenger != null)
            {
                CancellationToken ct = source.Token;
                task =  Task.Run(async () =>{
                while (true && !ct.IsCancellationRequested)
                {
                    await Task.Delay(5000);
                    var result = _model.LoadMostRecentNotification();
                    if (result != null)
                    {
                        {
                            if (result.Receiver == null || result.Receiver.Equals(loggedInPassenger.Seat.SeatCode))
                            {
                                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                                {
                                    await CreateContentDialog(result.Content);
                                });
                            }
                        }
                    }
                }
            });
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            source.Cancel();
            this.Dispatcher.StopProcessEvents();
            base.OnNavigatingFrom(e);
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

        private void NavigateToInfoPage() {
            NavigationViewControl.SelectedItem = null;
            NavigationViewFrame.Navigate(typeof(InfoPage));
        }

        private void FontIcon_PointerPressed(object sender, PointerRoutedEventArgs e){
            NavigateToInfoPage();
        }
    }
}
