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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace InFlightApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PassengerPopupPage : Page{
        private Persoon pers;
        private PassengersViewModel pvm;

        public delegate void CancelButtonPressedDel();
        public event CancelButtonPressedDel CancelButtonPressed;

        public PassengerPopupPage(Persoon pas){
            pers = pas;
            this.DataContext = pas;

            pvm = ServiceLocator.Current.GetService<PassengersViewModel>(true);

            DispatchImage();
            this.InitializeComponent();

            confButton.DataContext = pvm;
        }

        private async void DispatchImage() {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                string path = pvm.GetImageForPassenger(pers).Result;
                pers.ImageFile = path;
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            CancelButtonPressed.Invoke();
        }
    }
}
