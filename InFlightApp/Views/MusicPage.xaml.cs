using InFlightApp.Configuration;
using InFlightApp.Model;
using InFlightApp.View_Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
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
            SongSlider.Value = MusicPlayer.Position.TotalSeconds;
            if (!song.IsPlaying)
            {
                _model.SetPlaying(song);
                MusicPlayer.Position = TimeSpan.FromSeconds(0);
                SongSlider.Maximum = MusicPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                SongSlider.Value = 0;
                MusicPlayer.Play();
                SongSlider.Tapped += Slider_Dragged;
                void Slider_Dragged(object sender2, RoutedEventArgs e2)
                {
                    MusicPlayer.Position = TimeSpan.FromSeconds((sender2 as Slider).Value);
                    ExecuteSlider(SongSlider);
                }
                ExecuteSlider(SongSlider);
            }
            else
            {
                _model.SetNotPlaying();
                MusicPlayer.Position = TimeSpan.FromSeconds(0);
                SongSlider.Value = 0;
                MusicPlayer.Stop();
            }
        }

        private void ExecuteSlider(Slider slider)
        {
            var max = slider.Maximum;
            var pos = slider.Value;
            Task.Run(async () =>
             {
                 while (pos < max)
                 {
                     await Task.Delay(1000);
                     await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                     {
                         SongSlider.Value += 1;
                     });
                     pos += 1;
                 }
             });
        }

    }
}
