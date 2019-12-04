using InFlightApp.Configuration;
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
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace InFlightApp.Views
{
    public sealed partial class UserButton : UserControl{
        public UserButton(){
            try {
               LoginViewModel lvm = ServiceLocator.Current.GetService<LoginViewModel>(true);
                DataContext = lvm.GetLoggedInUser();
               InitializeComponent();
            } catch (Exception e){
                //Replace with logging later on
                Console.WriteLine(e);
            }

        }
    }
}
