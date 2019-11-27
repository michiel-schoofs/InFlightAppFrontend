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

namespace InFlightApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SeriesPage : Page
    {
        private readonly EntertainmentViewModel _model;

        public SeriesPage()
        {
            InitializeComponent();
            try
            {
                _model = ServiceLocator.Current.GetService<EntertainmentViewModel>(true);
                _model.LoadSeries();
                DataContext = _model;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void GridViewSeries_SelectionChanged(Object sender, RoutedEventArgs e)
        {
            var selectedSerie = (Serie)GridViewSeries.SelectedItem;
            ContentDialog contentDialog = new ContentDialog();

            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Vertical;

            TextBlock tbYearDirectorRuntime = new TextBlock() { Text = selectedSerie.Year + ", " + selectedSerie.Director + ", " + selectedSerie.Runtime, Padding = new Thickness(5) };
            TextBlock tbGenre = new TextBlock() { Text = selectedSerie.Genre, Padding = new Thickness(5) };
            TextBlock tbDescription = new TextBlock() { Text = selectedSerie.Plot, Padding = new Thickness(5) };
            TextBlock tbActors = new TextBlock() { Text = selectedSerie.Actors, Padding = new Thickness(5) };

            stackPanel.Children.Add(tbYearDirectorRuntime);
            stackPanel.Children.Add(tbDescription);
            stackPanel.Children.Add(tbGenre);
            stackPanel.Children.Add(tbActors);

            contentDialog.Title = selectedSerie.Title;
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
