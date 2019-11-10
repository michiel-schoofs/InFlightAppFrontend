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
                Seats = new ObservableCollection<Seat>();
                Passengers = new ObservableCollection<Passenger>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void LoadData()
        {
            Seats = (ObservableCollection<Seat>)_flightRepo.GetSeats().GetAwaiter().GetResult();
            Passengers = (ObservableCollection<Passenger>)_userInterface.GetPassengers().GetAwaiter().GetResult();

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
