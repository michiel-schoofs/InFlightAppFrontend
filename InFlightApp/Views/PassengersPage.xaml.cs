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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace InFlightApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PassengersPage : Page
    {
        private PassengersViewModel _model;

        public PassengersPage()
        {
            InitializeComponent();
            try
            {
                _model = ServiceLocator.Current.GetService<PassengersViewModel>(true);
                DataContext = _model;
                _model.LoadData();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            LayoutSeatingDesign();
        }

        private void LayoutSeatingDesign()
        {
            List<char> columnLetters = _model.GetSeatColumns();
            int numberOfRows = _model.GetSeatRows();

            for (int i = 0; i < numberOfRows + 1; i++)
            {
                RowDefinition rdRow = new RowDefinition();
                rdRow.Height = new GridLength(1, GridUnitType.Auto);
                SeatingsGrid.RowDefinitions.Add(rdRow);

                for (int j = 0; j < columnLetters.Count() + 1; j++)
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
                        tbHeader.Text = $"{columnLetters[j - 1]}";
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

                        StackPanel spPerson = new StackPanel();

                        TextBlock tbName = new TextBlock();
                        tbName.Text = "Tybo Vanderstraeten";
                        tbName.HorizontalAlignment = HorizontalAlignment.Center;
                        tbName.VerticalAlignment = VerticalAlignment.Center;
                        tbName.FontSize = 20;
                        tbName.Margin = new Thickness(15);
                        spPerson.Children.Add(tbName);

                        StackPanel spButtons = new StackPanel();
                        spButtons.Orientation = Orientation.Horizontal;

                        Button btnChangeSeat = new Button();
                        btnChangeSeat.Content = "Change seat";
                        btnChangeSeat.HorizontalAlignment = HorizontalAlignment.Center;
                        btnChangeSeat.VerticalAlignment = VerticalAlignment.Center;
                        btnChangeSeat.FontSize = 20;
                        btnChangeSeat.Margin = new Thickness(15);
                        spButtons.Children.Add(btnChangeSeat);

                        Button btnViewOrders = new Button();
                        btnViewOrders.Content = "View orders";
                        btnViewOrders.HorizontalAlignment = HorizontalAlignment.Center;
                        btnViewOrders.VerticalAlignment = VerticalAlignment.Center;
                        btnViewOrders.FontSize = 20;
                        btnViewOrders.Margin = new Thickness(15);
                        spButtons.Children.Add(btnViewOrders);

                        spPerson.Children.Add(spButtons);
                        spPerson.Margin = new Thickness(15);

                        SeatingsGrid.Children.Add(spPerson);
                        Grid.SetRow(spPerson, i);
                        Grid.SetColumn(spPerson, j);
                    }
                }
            }
        }
    }
}
