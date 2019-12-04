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

    public sealed partial class InfoPage : Page
    {
        private readonly InfoViewModel _model;
        public InfoPage()
        {
            InitializeComponent();
            try
            {
                _model = ServiceLocator.Current.GetService<InfoViewModel>(true);
                _model.LoadFlightInfo();
                _model.LoadWeatherInfo();
                DataContext = _model;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
