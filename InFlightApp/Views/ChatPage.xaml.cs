using InFlightApp.Configuration;
using InFlightApp.View_Model;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace InFlightApp.Views {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChatPage : Page {
        private readonly TravelGroupViewModel _model;

        public ChatPage() {
            try
            {
                _model = ServiceLocator.Current.GetService<TravelGroupViewModel>(true);
                DataContext = _model;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            this.InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if ( InputBox.Text != "")
            {
                _model.SendMessage(InputBox.Text);
                InputBox.Text = "";
            }
        }
    }
}
