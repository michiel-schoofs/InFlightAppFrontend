using InFlightApp.Configuration;
using InFlightApp.Model;
using InFlightApp.View_Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
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
            var resourceBundle = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

            StringBuilder content = new StringBuilder().Append(selectedSerie.Year + ", " + selectedSerie.Runtime + "\n\n");
            content.Append(selectedSerie.Genre + "\n\n");
            content.Append(selectedSerie.Plot + "\n\n");
            content.Append(selectedSerie.Actors);

            contentDialog.Title = selectedSerie.Title;
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
