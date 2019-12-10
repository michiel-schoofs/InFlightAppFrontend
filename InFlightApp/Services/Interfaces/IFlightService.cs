using InFlightApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InFlightApp.Services.Interfaces
{
    public interface IFlightService
    {
        IEnumerable<Seat> GetSeats();
        Flight GetFlight();
        Weather GetCurrentWeather(string city,string country);
        Persoon GetPassengerOnSeat(int seatID);
        bool SeatHasUser(int id);
    }
}
