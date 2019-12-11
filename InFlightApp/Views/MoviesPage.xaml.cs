using InFlightApp.Configuration;
using InFlightApp.Model;
using InFlightApp.View_Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
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
            _model.LoadDetailsMovie(selectedMovie.imdbID);

            ContentDialog contentDialog = new ContentDialog();
            var resourceBundle = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

            StringBuilder content = new StringBuilder().Append(selectedMovie.Year + ", " + selectedMovie.Director + ", " + selectedMovie.Runtime + "\n\n");
            content.Append(selectedMovie.Genre + "\n\n");
            content.Append(selectedMovie.Plot + "\n\n");
            content.Append(selectedMovie.Actors);

            contentDialog.Title = selectedMovie.Title;
            contentDialog.Content = content;
            contentDialog.PrimaryButtonText = resourceBundle.GetString("Watch");
            contentDialog.PrimaryButtonClick += ContentDialog_WatchButtonClick;
            void ContentDialog_WatchButtonClick(ContentDialog sender2, ContentDialogButtonClickEventArgs e2)
            {
                Frame.Navigate(typeof(MoviePlayerPage));
            }
            contentDialog.CloseButtonText = resourceBundle.GetString("Close");

            contentDialog.DefaultButton = ContentDialogButton.Primary;

            try
            {
                contentDialog.ShowAsync();
            }
            catch (Exception ex) { }
        }
    }
}
