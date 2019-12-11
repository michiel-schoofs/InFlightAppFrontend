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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace InFlightApp.Views {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HandlingOrdersPage : Page {
        private HandleOrdersViewModel _model;

        public HandlingOrdersPage() {
            this.InitializeComponent();
            _model = ServiceLocator.Current.GetService<HandleOrdersViewModel>(true);
            
            _model.PopUpDeny.Subscribe(order => {
                MakePopup(_model.DenyOrder, order, "Are you sure yu want to remove this order?");
            });

            _model.PopUpApprove.Subscribe(order => {
                MakePopup(_model.ApproveOrder, order, "Are you sure the passenger paid for this order?");
            });

            DataContext = _model;
        }

        private async void MakePopup(RelayCommand rc, Order o, String message) {
            ContentDialog dialog = new ContentDialog() {
                Title = "Warning",
                Content = message,
                PrimaryButtonText = "Yes",
                CloseButtonText = "No"
            };

            dialog.PrimaryButtonCommand = rc;
            dialog.PrimaryButtonCommandParameter = o;

            await dialog.ShowAsync();
        }
    }
}
