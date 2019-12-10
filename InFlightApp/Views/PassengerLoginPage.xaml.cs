using InFlightApp.Configuration;
using InFlightApp.Model;
using InFlightApp.View_Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace InFlightApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserloginGrid : Page
    {
        private Type _originPage;
        private PassengersViewModel pvm;

        public UserloginGrid(){
            pvm = ServiceLocator.Current.GetService<PassengersViewModel>(true);
            this.DataContext = pvm;
            pvm.SelectionChanged += Pvm_SelectionChanged;
            this.InitializeComponent();
        }

        private void Pvm_SelectionChanged(Seat s){

            //this.Frame.Navigate(typeof(ChatPage),s.SeatId);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e){
            _originPage = (Type)e.Parameter;
            base.OnNavigatedTo(e);
            var anim = ConnectedAnimationService.GetForCurrentView().GetAnimation("ForwardConnectedAnimation");
            if (anim != null)
            {
                anim.TryStart(Grid);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e){
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("BackwardConnectedAnimation", Grid);
            if (_originPage != null)
                this.Frame.Navigate(_originPage, null, new SuppressNavigationTransitionInfo());
        }

        private void Grid_SelectionChanged(object sender, SelectionChangedEventArgs e){
            int index = Grid.SelectedIndex;
            var cont = Grid.ContainerFromIndex(index);
            ShowFlyoutAt((FrameworkElement)cont);
        }

        private void Grid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Grid send = (Grid)sender;
            Grid.SelectedItem = send.DataContext;
            //ShowFlyoutAt(send);
        }


        private async void ShowFlyoutAt(FrameworkElement element) {
            Flyout fl = new Flyout();

            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                bool b = pvm.SeatHasUser().Result;
                if (!b){
                    var resourceBundle = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
                    string text = resourceBundle.GetString("seatHasNoUser");
                    TextBlock tb = MakeTextBlock(text);
                    fl.Content = tb;
                }else {
                   Persoon pas = pvm.GetPassengerOnSeat();
                   fl.Content = new PassengerPopupPage(pas);
                }
                fl.ShowAt(element);
            });

        }

        private TextBlock MakeTextBlock(string text) {
            TextBlock tb = new TextBlock();
            tb.Text = text;
            tb = AddStyling(tb);
            return tb;
        }


        private TextBlock AddStyling(TextBlock tb) {
            tb.Padding = new Thickness(10);
            tb.FontSize = 15;
            tb.FontWeight = Windows.UI.Text.FontWeights.Bold;
            return tb;
        }
    }
}
