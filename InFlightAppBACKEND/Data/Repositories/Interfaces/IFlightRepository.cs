using InFlightAppBACKEND.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InFlightAppBACKEND.Data.Repositories
{
    public interface IFlightRepository
    {
        User GetByUsername(string username);
        IEnumerable<Flight> GetAll();
        IEnumerable<Passenger> GetPassengersFromFlight(int id);
        IEnumerable<Seat> GetAllSeats();
        Seat GetSeatById(int seatNr);
        Flight GetById(int id);
        void Add(Flight flight);
        void Remove(Flight flight);
        void SaveChanges();
    }
}
