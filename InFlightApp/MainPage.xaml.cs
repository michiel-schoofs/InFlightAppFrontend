
using InFlightApp.Configuration;
using InFlightApp.View_Model;
using InFlightApp.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace InFlightApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private LoginViewModel lvm;
        private static bool ensureOneTime;

        public MainPage()
        {
            Startup.ConfigureAsync();
            this.InitializeComponent();

            try
            {
                if (lvm == null){
                    ensureOneTime = false;
                    lvm = ServiceLocator.Current.GetService<LoginViewModel>(true);
                    PassengersViewModel pvm = ServiceLocator.Current.GetService<PassengersViewModel>(true);

                    pvm.LoginSuccess += NavigateToMP;
                    lvm.LoginSuccess += NavigateToMP;
                }
            }catch (Exception e){
                //Replace with logging later on
                Console.WriteLine(e);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MenuPage));
        }

        private void NavigateToMP() {
            if(!ensureOneTime)
                this.Frame.Navigate(typeof(MenuPage));

            ensureOneTime = true;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainPageViewModel vm = new MainPageViewModel();
            bool cred = vm.CheckForUserCredentials();

            if (cred)
                Window.Current.Content = new MenuPage();
            else
                this.ShowFrame.Navigate(typeof(StartAppPage));
            base.OnNavigatedTo(e);
        }
    }
}
