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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace InFlightApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MenuPage : Page
    {
        public MenuPage()
        {
            this.InitializeComponent();
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
                    }
                }
            }
        }
    }
}
