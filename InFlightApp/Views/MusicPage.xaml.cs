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

namespace InFlightApp.Views
{
    public sealed partial class MusicPage : Page
    {
        private readonly EntertainmentViewModel _model;
        private bool IsPlaying { get; set; }

        public MusicPage()
        {
            InitializeComponent();
            try
            {
                _model = ServiceLocator.Current.GetService<EntertainmentViewModel>(true);
                _model.LoadMusic();
                DataContext = _model;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Song_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var stackPanel = sender as StackPanel;
            var symbolIcon = new SymbolIcon()
            {
                Symbol = Symbol.Volume,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10, 0, 0, 0)
            };
            if (!IsPlaying)
            {
                if (stackPanel.Children.Contains(symbolIcon))
                {
                    stackPanel.Children.Remove(symbolIcon);
                }
                MusicPlayer.Position = TimeSpan.FromSeconds(0);
                MusicPlayer.Play();
                IsPlaying = true;
                stackPanel.Children.Add(symbolIcon);
            }
            else
            {
                MusicPlayer.Position = TimeSpan.FromSeconds(0);
                MusicPlayer.Stop();
                IsPlaying = false;
                if (stackPanel.Children.Contains(symbolIcon))
                {
                    stackPanel.Children.Remove(symbolIcon);
                }
            }
        }
    }
}
