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
            LayoutSeatingDesign();
        }

        private void LayoutSeatingDesign()
        {
            string[] columns = { "A", "B", "C", "D", "E", "F", "G" };
            int seatsPerColumn = 40;


            for (int i = 0; i < seatsPerColumn + 1; i++)
            {
                RowDefinition rdRow = new RowDefinition();
                rdRow.Height = new GridLength(1, GridUnitType.Auto);
                SeatingsGrid.RowDefinitions.Add(rdRow);

                for (int j = 0; j < columns.Length + 1; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        ColumnDefinition cdZeroZero = new ColumnDefinition();
                        cdZeroZero.Width = new GridLength(1, GridUnitType.Auto);
                        SeatingsGrid.ColumnDefinitions.Add(cdZeroZero);
                    }
                    else if (i == 0 && j > 0)
                    {
                        ColumnDefinition cdColumnHeader = new ColumnDefinition();
                        cdColumnHeader.Width = new GridLength(1, GridUnitType.Auto);
                        SeatingsGrid.ColumnDefinitions.Add(cdColumnHeader);

                        TextBlock tbHeader = new TextBlock();
                        tbHeader.Text = $"{columns[j - 1]}";
                        tbHeader.HorizontalAlignment = HorizontalAlignment.Center;
                        tbHeader.VerticalAlignment = VerticalAlignment.Center;
                        tbHeader.FontSize = 30;
                        tbHeader.Margin = new Thickness(15);

                        SeatingsGrid.Children.Add(tbHeader);
                        Grid.SetRow(tbHeader, i);
                        Grid.SetColumn(tbHeader, j);

                    }
                    else if (i > 0 && j == 0)
                    {

                        RowDefinition rdHeader = new RowDefinition();
                        rdHeader.Height = new GridLength(1, GridUnitType.Auto);
                        SeatingsGrid.RowDefinitions.Add(rdHeader);

                        TextBlock tbHeader = new TextBlock();
                        tbHeader.Text = $"{i}";
                        tbHeader.HorizontalAlignment = HorizontalAlignment.Center;
                        tbHeader.VerticalAlignment = VerticalAlignment.Center;
                        tbHeader.FontSize = 30;
                        tbHeader.Margin = new Thickness(15);

                        SeatingsGrid.Children.Add(tbHeader);
                        Grid.SetRow(tbHeader, i);
                        Grid.SetColumn(tbHeader, j);

                    }
                    else
                    {
                        ColumnDefinition cdColumn = new ColumnDefinition();
                        cdColumn.Width = new GridLength(1, GridUnitType.Auto);
                        SeatingsGrid.ColumnDefinitions.Add(cdColumn);

                        TextBlock tbName = new TextBlock();
                        tbName.Text = "Tybo Vanderstraeten";
                        tbName.HorizontalAlignment = HorizontalAlignment.Center;
                        tbName.VerticalAlignment = VerticalAlignment.Center;
                        tbName.FontSize = 30;
                        tbName.Margin = new Thickness(15);

                        SeatingsGrid.Children.Add(tbName);
                        Grid.SetRow(tbName, i);
                        Grid.SetColumn(tbName, j);
                    }
                }
            }
        }
    }
}
