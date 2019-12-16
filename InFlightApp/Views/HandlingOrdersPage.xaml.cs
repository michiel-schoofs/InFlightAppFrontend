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

            try {
                _model = ServiceLocator.Current.GetService<HandleOrdersViewModel>(true);

                _model.UpdateOrders();
                _model.PopUpDeny.Subscribe(order => {
                    MakePopup(_model.DenyOrder, order, "RemoveOrder");
                });

                _model.PopUpApprove.Subscribe(order => {
                    MakePopup(_model.ApproveOrder, order, "AcceptOrder");
                });

                DataContext = _model;
            } catch (Exception e) {
                MakeWarning(e.Message);
            }

        }

        private async void MakePopup(RelayCommand rc, Order o, String key) {
            var resourceBundle = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            var yes = resourceBundle.GetString("Yes");
            var no = resourceBundle.GetString("No");
            var message = resourceBundle.GetString(key);
            var warning = resourceBundle.GetString("Warning");

            ContentDialog dialog = new ContentDialog() {
                Title = warning,
                Content = message,
                PrimaryButtonText = yes,
                CloseButtonText = no
            };

            dialog.PrimaryButtonCommand = rc;
            dialog.PrimaryButtonCommandParameter = o;

            //Dirty NoUIEntryPoints Fix
            try {
                await dialog.ShowAsync();
            } catch {}
        }

        private async void MakeWarning(String message) {
            var resourceBundle = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            var warning = resourceBundle.GetString("Warning");
            ContentDialog dialog = new ContentDialog() {
                Title = warning,
                Content = message,
                PrimaryButtonText = "Ok",
            };

            try {
                await dialog.ShowAsync();
            } catch { }
        }
    }
}
