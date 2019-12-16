using InFlightApp.Configuration;
using InFlightApp.Model;
using InFlightApp.View_Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
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
        private readonly HandleOrdersViewModel _hovm;
        private Flyout _flyout;
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
                _hovm = ServiceLocator.Current.GetService<HandleOrdersViewModel>(true);

                logoutBtn.DataContext = _userModel;
                UserStackPanel.DataContext = _userModel;
                _userModel.LoggedOut += Lvm_LoggedOut;

                PassengerType? pt = _userModel.GetPassengerType();
                if (pt != null && pt == PassengerType.Passenger) {
                    HideUIElements();
                    AddUIElements();
                }

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

        private void HideUIElements() {
            NavPassenger.Visibility = Visibility.Collapsed;
            NavOrders.Visibility = Visibility.Collapsed;
            NavNotif.Visibility = Visibility.Collapsed;
        }

        private void AddUIElements() {
            this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                if (_userModel.PassengerInFlightgroup()) 
                    chatIcon.Visibility = Visibility.Visible;

                cartIcon.Visibility = Visibility.Visible;
                AddCartInteractivity();
                HandleOrdersViewModel.CartChanged += _hovm_CartChanged;
            });
        }

        private Task _hovm_CartChanged(){
            AddCartInteractivity();
            
            if(_flyout != null && _flyout.IsOpen)
                makeFlyout();

            return null;
        }

        private void AddCartInteractivity() {
            if (_hovm.GetAmountOfProductsInCar() > 0)
                hasOrdersCircle.Visibility = Visibility.Visible;
            else
                hasOrdersCircle.Visibility = Visibility.Collapsed;
        }

        private async void Lvm_LoggedOut(){
            if (!ensureonetime) {
                if (Frame == null)
                    await CoreApplication.RequestRestartAsync("oopsie");
                else
                    Frame.Navigate(typeof(MainPage));
            }
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
                        case "Nav_Orders":
                            NavigationViewFrame.Navigate(typeof(HandlingOrdersPage));
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
                    await Task.Delay(10000);
                    ddvar result = _model.LoadMostRecentNotification();
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

            var resourceBundle = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            var title = resourceBundle.GetString("NotificationTitleDialogTitle");
            var close = resourceBundle.GetString("Close");

            ContentDialog contentDialog = new ContentDialog();
            contentDialog.Title = title;
            contentDialog.Content = notification;
            contentDialog.CloseButtonText = close;
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

        private void chatIcon_PointerPressed(object sender, PointerRoutedEventArgs e){
            this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                Seat s = _userModel.GetSeatOfLoggedIn();
                ChatPage chat = new ChatPage(s.SeatId);

                Flyout fl = new Flyout();
                StyleFlyout(fl);

                fl.Content = chat;
                fl.ShowAt(chatIcon);
            });
        }

        private void StyleFlyout(Flyout fl) {
            var style = new Style(typeof(FlyoutPresenter));

            style.Setters.Add(new Setter(FlyoutPresenter.BackgroundProperty, new AcrylicBrush() { Opacity = 0 }));
            style.Setters.Add(new Setter(FlyoutPresenter.MarginProperty,new Thickness(40,0,40,0)));
            style.Setters.Add(new Setter(FlyoutPresenter.BorderThicknessProperty, 0));
            style.Setters.Add(new Setter(FlyoutPresenter.MinHeightProperty, 600));
            style.Setters.Add(new Setter(FlyoutPresenter.MinWidthProperty, 600));

            fl.FlyoutPresenterStyle = style;
        }

        private void StyleFlyoutCart(Flyout fl) {
            var style = new Style(typeof(FlyoutPresenter));
            style.Setters.Add(new Setter(FlyoutPresenter.MarginProperty, new Thickness(0,15,0,0)));
            fl.FlyoutPresenterStyle = style;
        }

        private void cartIcon_PointerPressed(object sender, PointerRoutedEventArgs e){
            makeFlyout();
        }

        private void makeFlyout() {
            this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                Flyout fl = new Flyout();
                StyleFlyoutCart(fl);

                if(this._flyout !=null)
                    this._flyout.Hide();

                if (_hovm.GetAmountOfProductsInCar() <= 0)
                {
                    TextBlock tb = new TextBlock();

                    var resourceBundle = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
                    tb.Text = resourceBundle.GetString("emptyCart");

                    fl.Content = tb;
                } else {
                    fl.Content = new ShoppingCart();
                }

                this._flyout = fl;
                fl.Closing += Fl_Closing;

                fl.ShowAt(cartIcon);
            });
        }

        private void Fl_Closing(FlyoutBase sender, FlyoutBaseClosingEventArgs args) {
            this._flyout = null;
        }

    }
}
