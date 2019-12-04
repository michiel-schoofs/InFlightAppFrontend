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

namespace InFlightApp.Views
{
    public sealed partial class MusicPage : Page
    {
        private readonly EntertainmentViewModel _model;

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
            var song = Songlist.SelectedItem as Music;

            if (!song.IsPlaying)
            {
                _model.SetPlaying(song);
                MusicPlayer.Position = TimeSpan.FromSeconds(0);
                MusicPlayer.Play();
            }
            else
            {
                _model.SetNotPlaying();
                MusicPlayer.Position = TimeSpan.FromSeconds(0);
                MusicPlayer.Stop();
            }
        }
    }
}
