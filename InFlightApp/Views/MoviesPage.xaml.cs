using InFlightApp.Configuration;
using InFlightApp.Model;
using InFlightApp.View_Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace InFlightApp.Views
{
    public sealed partial class MoviesPage : Page
    {
        private readonly EntertainmentViewModel _model;

        public MoviesPage()
        {
            InitializeComponent();
            try
            {
                _model = ServiceLocator.Current.GetService<EntertainmentViewModel>(true);
                _model.LoadMovies();
                DataContext = _model;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void GridViewMovies_SelectionChanged(Object sender, RoutedEventArgs e)
        {
            var selectedMovie = (Movie)GridViewMovies.SelectedItem;
            ContentDialog contentDialog = new ContentDialog();

            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Vertical;

            TextBlock tbYearDirectorRuntime = new TextBlock() { Text = selectedMovie.Year + ", " + selectedMovie.Director + ", " + selectedMovie.Runtime, Padding = new Thickness(5) };
            TextBlock tbGenre = new TextBlock() { Text = selectedMovie.Genre, Padding = new Thickness(5) };
            TextBlock tbDescription = new TextBlock() { Text = selectedMovie.Plot, Padding = new Thickness(5) };
            TextBlock tbActors = new TextBlock() { Text = selectedMovie.Actors, Padding = new Thickness(5) };

            stackPanel.Children.Add(tbYearDirectorRuntime);
            stackPanel.Children.Add(tbDescription);
            stackPanel.Children.Add(tbGenre);
            stackPanel.Children.Add(tbActors);

            contentDialog.Title = selectedMovie.Title;
            contentDialog.Content = stackPanel;
            contentDialog.PrimaryButtonText = "Watch";
            contentDialog.PrimaryButtonClick += ContentDialog_WatchButtonClick;
            void ContentDialog_WatchButtonClick(ContentDialog sender2, ContentDialogButtonClickEventArgs e2)
            {
                Frame.Navigate(typeof(MoviePlayerPage));
            }
            contentDialog.CloseButtonText = "Close";

            contentDialog.DefaultButton = ContentDialogButton.Primary;

            try
            {
                contentDialog.ShowAsync();
            }
            catch (Exception ex) { }
        }
    }
}
