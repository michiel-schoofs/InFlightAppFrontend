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
        private readonly IFlightService _flightRepo;
        private readonly IUserService _userInterface;

        public ObservableCollection<Seat> Seats { get; set; }
        public ObservableCollection<Passenger> Passengers { get; set; }

        public Seat SelectedSeat { get; set; }
        public RelayCommand SetPassenger { get; set; }

        public int SeatRows { get => GetSeatRows(); }
        public int SeatColumns { get => GetSeatColumns().Count*2; }

        public delegate void SeatSelectionChanged(Seat s);
        public event SeatSelectionChanged SelectionChanged;

        public RelayCommand LoginAsPassenger { get; set; }

        public delegate void LoginSuccessDelegate();
        public event LoginSuccessDelegate LoginSuccess;

        public PassengersViewModel()
        {
            try
            {
                _flightRepo = ServiceLocator.Current.GetService<IFlightService>(true);
                _userInterface = ServiceLocator.Current.GetService<IUserService>(true);
                LoadData();
                MakeCommands();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void MakeCommands() {
            SetPassenger = new RelayCommand((object o) => {
                SelectionChanged.Invoke(SelectedSeat);
            });

            LoginAsPassenger = new RelayCommand((object o) =>{
                if (_userInterface.AuthenticatePassenger(SelectedSeat.SeatId))
                    LoginSuccess.Invoke();
                
            });
        }

        public async Task<bool> SeatHasUser() {
            if (SelectedSeat == null)
                return false;

            return _flightRepo.SeatHasUser(SelectedSeat.SeatId);
        }

        public async Task<string> GetImageForPassenger(Persoon pers) {
            return _userInterface.GetImageForPerson(pers).Result;
        }

        public Persoon GetPassengerOnSeat() {
            if (SelectedSeat == null)
                return null;

            return _flightRepo.GetPassengerOnSeat(SelectedSeat.SeatId);
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
            LoadData();
        }

        private void LoadData()
        {
            Passengers = new ObservableCollection<Passenger>(_userInterface.GetPassengers());
            Seats = new ObservableCollection<Seat>(_flightRepo.GetSeats());

        }
    }
}
