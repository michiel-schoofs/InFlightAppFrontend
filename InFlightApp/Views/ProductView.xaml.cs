using InFlightApp.Configuration;
using InFlightApp.Model;
using InFlightApp.View_Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace InFlightApp.Views
{
    public sealed partial class ProductView : UserControl
    {
        private HandleOrdersViewModel hovm;
        private readonly LoginViewModel lvm;

        public ProductView(){
            lvm = ServiceLocator.Current.GetService<LoginViewModel>(true);
            this.InitializeComponent();

            this.DataContextChanged += ProductView_DataContextChanged;
        }

        private void ProductView_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args){
            if (lvm.GetPassengerType() == PassengerType.Passenger  && args.NewValue != null)
                ChangeToOrder();
        }

        private void ChangeToOrder()
        {
            hovm = ServiceLocator.Current.GetService<HandleOrdersViewModel>(true);
            HandleOrdersViewModel.CartChanged += Hovm_CartChanged;

            refillButton.Visibility = Visibility.Collapsed;
            orderButton.Visibility = Visibility.Visible;
            addToCartbtn.DataContext = hovm;

            ChangeTextBox();
        }

        private Task Hovm_CartChanged(){
            ChangeTextBox();
            return null;
        }

        private void ChangeTextBox(){
            int amount = hovm.GetAmountInCart((Product)DataContext);
            var resourceBundle = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

            if (amount < 0)
                status.Text = resourceBundle.GetString("productNotInCart");
            else
                status.Text = resourceBundle.GetString("productInCart") + ' ' + amount.ToString();

        }
    }
}
