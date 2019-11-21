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
        private readonly IUserInterface _userInterface;

        public ObservableCollection<Seat> Seats { get; set; }
        public ObservableCollection<Passenger> Passengers { get; set; }

        public PassengersViewModel()
        {
            try
            {
                _flightRepo = ServiceLocator.Current.GetService<IFlightRepository>(true);
                _userInterface = ServiceLocator.Current.GetService<IUserInterface>(true);
                Seats = new ObservableCollection<Seat>(_flightRepo.GetSeats());
                Passengers = new ObservableCollection<Passenger>(_userInterface.GetPassengers());
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
                   }).Distinct().OrderBy(c => c).ToList();
        }

        public int GetSeatRows()
        {
            return Seats.Count() / GetSeatColumns().Count();
        }

        public void ChangeSeat(int userId, int seatId)
        {
            _userInterface.ChangeSeat(userId, seatId);
        }

    }
}
