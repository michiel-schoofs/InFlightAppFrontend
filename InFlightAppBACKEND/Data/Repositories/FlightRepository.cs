using InFlightAppBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InFlightAppBACKEND.Data.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private DbSet<Flight> _flights;
        private DbSet<CrewMember> _crewMembers;
        private DbSet<Passenger> _passengers;
        private DBContext _dbContext;

        public FlightRepository(DBContext dbContext)
        {
            _flights = dbContext.Flights;
            _crewMembers = dbContext.CrewMembers;
            _passengers = dbContext.Passengers;
            _dbContext = dbContext;
        }



        public IEnumerable<Flight> GetAll()
        {
            return _flights.Include(f => f.Origin).Include(f => f.Destination).ToList();
        }

        public IEnumerable<Passenger> GetPassengersFromFlight(int id)
        {
            return _flights.Include(f => f.Origin).Include(f => f.Destination).SingleOrDefault(f => f.FlightId == id).Seats.Select(s => s.Passenger).ToList();
        }

        public Flight GetById(int id)
        {
            return _flights.Include(f => f.Origin).Include(f => f.Destination).SingleOrDefault(f => f.FlightId == id);
        }

        public Seat GetSeatById(int seatNr)
        {
            return _flights.Include(f => f.Seats).ThenInclude(s=>s.Passenger)
                .First().Seats.FirstOrDefault(s => s.SeatId == seatNr);
        }

        public void Add(Flight flight)
        {
            _flights.Add(flight);
        }

        public IEnumerable<Seat> GetAllSeats()
        {
            return _flights.Include(f => f.Seats).First().Seats;
        }

        public void Remove(Flight flight)
        {
            _flights.Remove(flight);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public User GetByUsername(string username)
        {
            User user = _crewMembers.Include(c=>c.ProfilePicture)
                .FirstOrDefault(c => c.Username.ToLower().Equals(username.ToLower()));

            if (user == null)
            {
                var passengers = _passengers.Include(p => p.ProfilePicture).Include(p => p.Seat).Include(p => p.TravelGroup).ToList();
                user = passengers.FirstOrDefault(p => p.Username.ToLower().Equals(username.ToLower()));
            }

            return user;
        }
    }
}
