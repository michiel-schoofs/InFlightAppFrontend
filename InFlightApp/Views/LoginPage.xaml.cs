using InFlightApp.View_Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class LoginPage : Page
    {
        private Type _originPage;
        private LoginViewModel model;

        public LoginPage() {
            this.InitializeComponent();
            VisualStateManager.GoToState(usernameField, "Error", false);
            model = new LoginViewModel();
            this.DataContext = model;
            model.LoginFailedEvent += Model_LoginFailedEvent;
            model.LoginSuccess += Model_LoginSuccess;
        }

        private void Model_LoginSuccess(){
            //Transition needs to be smoother
            Frame.Navigate(typeof(MainSelectionpage));
        }

        private async Task Model_LoginFailedEvent(string str){
            SolidColorBrush redColorBrush = new SolidColorBrush(Colors.Red);
            usernameField.BorderBrush = redColorBrush;
            passwordField.BorderBrush = redColorBrush;

            ContentDialog ErrorDialog = new ContentDialog(){
                Title = "Login Failure",
                Content = str,
                CloseButtonText = "Ok"
            };

            await ErrorDialog.ShowAsync();
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
                this.Frame.Navigate(_originPage,null,new SuppressNavigationTransitionInfo());
        }
    }
}
