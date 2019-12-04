﻿using InFlightApp.Configuration;
using InFlightApp.View_Model;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace InFlightApp.Views {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChatPage : Page {
        private TravelGroupViewModel _model;

        public ChatPage() {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            int seatnr = (int)e.Parameter;
            base.OnNavigatedTo(e);
            try
            {
                //_model = ServiceLocator.Current.GetService<TravelGroupViewModel>(true);
                _model = new TravelGroupViewModel(seatnr);
                DataContext = _model;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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
