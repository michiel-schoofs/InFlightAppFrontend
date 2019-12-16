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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            LayoutSeatingDesign();
        }

        private void LayoutSeatingDesign()
        {
            SeatingsGrid.Children.Clear();
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
                        /*
                         *This block is to make up the seats and if present the passenger for that seat
                         */

                        ColumnDefinition cdColumn = new ColumnDefinition();
                        cdColumn.Width = new GridLength(1, GridUnitType.Auto);
                        SeatingsGrid.ColumnDefinitions.Add(cdColumn);

                        StackPanel spPerson = new StackPanel();
                        spPerson.Margin = new Thickness(15);

                        string gridCode = i.ToString() + columnLetters[j - 1];
                        Seat seat = _model.Seats.SingleOrDefault(s => s.SeatCode.Equals(gridCode));
                        Passenger passenger = _model.Passengers.SingleOrDefault(p => p.Seat.SeatId == seat.SeatId);

                        #region Name
                        TextBlock tbName = new TextBlock();
                        tbName.Text = passenger != null ? passenger.FirstName + " " + passenger.LastName : "Empty seat";
                        tbName.HorizontalAlignment = HorizontalAlignment.Center;
                        tbName.VerticalAlignment = VerticalAlignment.Center;
                        tbName.FontSize = 20;
                        tbName.Margin = new Thickness(15);
                        spPerson.Children.Add(tbName);
                        #endregion

                        #region Buttons + clickHandlers
                        StackPanel spButtons = new StackPanel();
                        spButtons.Orientation = Orientation.Horizontal;

                        Button btnChangeSeat = new Button();
                        btnChangeSeat.Content = "Change seat";
                        btnChangeSeat.HorizontalAlignment = HorizontalAlignment.Center;
                        btnChangeSeat.VerticalAlignment = VerticalAlignment.Center;
                        btnChangeSeat.FontSize = 20;
                        btnChangeSeat.Margin = new Thickness(15);
                        spButtons.Children.Add(btnChangeSeat);              

                        if (passenger == null)
                        {
                            btnChangeSeat.Visibility = Visibility.Collapsed;
                        }

                        // Click events for buttons
                        btnChangeSeat.Click += ButtonChangeSeat_Click;

                        #region Button Change Seat
                        void ButtonChangeSeat_Click(object sender, RoutedEventArgs e)
                        {
                            btnChangeSeat.IsEnabled = false;

                            StackPanel spChangeSeat = new StackPanel();
                            spPerson.Children.Add(spChangeSeat);
                            spChangeSeat.Orientation = Orientation.Horizontal;

                            ComboBox cbSeats = new ComboBox();
                            foreach (var s in _model.Seats.OrderBy(s => s.SeatCode))
                            {
                                cbSeats.Items.Add(s.SeatCode);
                            }
                            cbSeats.HorizontalAlignment = HorizontalAlignment.Center;
                            cbSeats.VerticalAlignment = VerticalAlignment.Center;
                            cbSeats.Margin = new Thickness(15);

                            Button btnChange = new Button();
                            btnChange.Content = "Change";
                            btnChange.HorizontalAlignment = HorizontalAlignment.Center;
                            btnChange.VerticalAlignment = VerticalAlignment.Center;
                            btnChange.Margin = new Thickness(15);
                            btnChange.Click += ButtonChange_Click;
                            void ButtonChange_Click(object sender2, RoutedEventArgs e2)
                            {
                                if (cbSeats.SelectedItem != null)
                                {
                                    var seatToChangeTo = _model.Seats.SingleOrDefault(s => s.SeatCode.Equals(cbSeats.SelectedItem.ToString())).SeatId;
                                    _model.ChangeSeat(passenger.Id, seatToChangeTo);
                                    LayoutSeatingDesign();
                                }
                                else
                                {
                                    spPerson.Children.Remove(spChangeSeat);
                                    btnChangeSeat.IsEnabled = true;

                                }
                            }

                            Button btnCancel = new Button();
                            btnCancel.Content = "Cancel";
                            btnCancel.HorizontalAlignment = HorizontalAlignment.Center;
                            btnCancel.VerticalAlignment = VerticalAlignment.Center;
                            btnCancel.Margin = new Thickness(15);
                            btnCancel.Click += ButtonCancel_Click;
                            void ButtonCancel_Click(object sender3, RoutedEventArgs e3)
                            {
                                spPerson.Children.Remove(spChangeSeat);
                                btnChangeSeat.IsEnabled = true;
                            }

                            spChangeSeat.Children.Add(cbSeats);
                            spChangeSeat.Children.Add(btnChange);
                            spChangeSeat.Children.Add(btnCancel);
                        }
                        #endregion

                        #region Textblock Name 
                        tbName.Tapped += TextBlockName_Tapped;
                        void TextBlockName_Tapped(Object sender, RoutedEventArgs e)
                        {
                            if (spPerson.Children.Contains(spButtons))
                            {
                                spPerson.Children.Remove(spButtons);
                            }
                            else
                            {
                                spPerson.Children.Add(spButtons);
                            }
                        }
                        #endregion
                        #endregion

                        // Add person stackpanel to grid
                        SeatingsGrid.Children.Add(spPerson);
                        Grid.SetRow(spPerson, i);
                        Grid.SetColumn(spPerson, j);
                    }
                }
            }
        }
    }
}
