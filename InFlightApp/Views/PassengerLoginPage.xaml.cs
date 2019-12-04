using InFlightApp.Model;
using InFlightApp.View_Model;
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
    public sealed partial class UserloginGrid : Page
    {
        private Type _originPage;

        public UserloginGrid()
        {
            PassengersViewModel pvm = new PassengersViewModel();
            this.DataContext = pvm;
            pvm.SelectionChanged += Pvm_SelectionChanged;
            this.InitializeComponent();
        }

        private void Pvm_SelectionChanged(Seat s){
            this.Frame.Navigate(typeof(ChatPage),s.SeatId);
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
    }
}
