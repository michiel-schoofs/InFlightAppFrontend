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
    public sealed partial class PassengersPage : Page
    {
        public PassengersPage()
        {
            this.InitializeComponent();
            LayoutDesign();
        }

        private void LayoutDesign()
        {
            string[] rows = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            int seatsPerRow = 5;
            for (int i = 0; i < rows.Length; i++)
            {
                RowDefinition rdRow = new RowDefinition();
                rdRow.Height = new GridLength(1, GridUnitType.Auto);
                SeatingsGrid.RowDefinitions.Add(rdRow);

                for (int j = 0; j < seatsPerRow; j++)
                {
                    ColumnDefinition cdColumn = new ColumnDefinition();
                    cdColumn.Width = new GridLength(1, GridUnitType.Auto);
                    SeatingsGrid.ColumnDefinitions.Add(cdColumn);

                    TextBlock tbRow = new TextBlock();
                    tbRow.Text = $"{rows[i]} - {j + 1}";
                    tbRow.HorizontalAlignment = HorizontalAlignment.Center;
                    tbRow.VerticalAlignment = VerticalAlignment.Center;
                    tbRow.FontSize = 30;
                    tbRow.Margin = new Thickness(15);

                    SeatingsGrid.Children.Add(tbRow);
                    Grid.SetRow(tbRow, i);
                    Grid.SetColumn(tbRow, j);
                }
            }
        }
    }
}
