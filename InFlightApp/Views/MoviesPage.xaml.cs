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
    public sealed partial class MoviesPage : Page
    {
        private readonly EntertainmentViewModel _model;

        public MoviesPage()
        {
            InitializeComponent();
            try
            {
                _model = ServiceLocator.Current.GetService<EntertainmentViewModel>(true);
                DataContext = _model;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void GridViewMovies_ItemClick(Object sender, RoutedEventArgs e)
        {

        }
    }
}
