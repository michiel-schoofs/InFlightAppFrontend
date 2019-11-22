//using InFlightApp.Configuration;
//using InFlightApp.Model;
//using InFlightApp.View_Model;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Runtime.InteropServices.WindowsRuntime;
//using Windows.Foundation;
//using Windows.Foundation.Collections;
//using Windows.UI.Core;
//using Windows.UI.Xaml;
//using Windows.UI.Xaml.Controls;
//using Windows.UI.Xaml.Controls.Primitives;
//using Windows.UI.Xaml.Data;
//using Windows.UI.Xaml.Input;
//using Windows.UI.Xaml.Media;
//using Windows.UI.Xaml.Navigation;

//// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

//namespace InFlightApp.Views
//{
//    /// <summary>
//    /// An empty page that can be used on its own or navigated to within a Frame.
//    /// </summary>
//    public sealed partial class GridViewProduct : UserControl
//    {
//        private ProductViewModel pvm;

//        public GridViewProduct()
//        {
//            pvm = ServiceLocator.Current.GetService<ProductViewModel>(true);
//            this.DataContext = pvm;
//            this.InitializeComponent();
//            this.gridTemp.ItemsSource = pvm.FilteredProducts;
//        }

//    }
//}
