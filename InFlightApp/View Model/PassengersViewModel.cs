using InFlightApp.Configuration;
using InFlightApp.Model;
using InFlightApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InFlightApp.View_Model
{
    public class PassengersViewModel
    {
        private readonly IFlightRepository _flightRepo;

        public ObservableCollection<Seat> Seats { get; set; }

        public PassengersViewModel()
        {
            try
            {
                _flightRepo = ServiceLocator.Current.GetService<IFlightRepository>(true);
                Seats = new ObservableCollection<Seat>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public List<char> GetSeatColumns()
        {
            return Seats.Aggregate(new List<char>(), (result, seat) =>
            {
                result.Add(seat.SeatCode[seat.SeatCode.Length - 1]);
                return result;
            });
        }

        public int GetSeatRows()
        {
            return Seats.Count() / GetSeatColumns().Count();
        }
    }
}
