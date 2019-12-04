using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class StartAppPage : Page
    {
        public StartAppPage() {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e){
            base.OnNavigatedTo(e);
            var anim = ConnectedAnimationService.GetForCurrentView().GetAnimation("BackwardConnectedAnimation");
            if (anim != null){
                anim.TryStart(crewButton);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e){
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ForwardConnectedAnimation",crewButton);
            this.Frame.Navigate(typeof(LoginPage),typeof(StartAppPage),new SuppressNavigationTransitionInfo());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e){
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ForwardConnectedAnimation", passengerButton);
            this.Frame.Navigate(typeof(UserloginGrid), typeof(StartAppPage), new SuppressNavigationTransitionInfo());
        }
    }
}
