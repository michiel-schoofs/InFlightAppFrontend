using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InFlightAppBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace InFlightAppBACKEND.Data.Repositories
{
    public class PassengerRepository : IPassengerRepository
    {
        private DbSet<Passenger> _passengers;
        private DBContext _dbContext;

        public PassengerRepository(DBContext dbContext)
        {
            _passengers = dbContext.Passengers;
            _dbContext = dbContext;
        }

        public IEnumerable<Passenger> GetAll(){
            return _passengers
                .Include(p => p.TravelGroup)
                .Include(p => p.Seat)
                .ToList();
        }

        public IEnumerable<Order> GetOrdersFromPassenger(int id)
        {
            return _passengers
                .SingleOrDefault(p => p.UserId == id)
                .Orders
                .ToList();
        }

        public Passenger GetById(int id){
            return _passengers
                .Include(p => p.TravelGroup)
                .Include(p => p.ProfilePicture)
                .Include(p=>p.Seat)
                .SingleOrDefault(p => p.UserId == id);
        }

        public int GetSeatNumberFromPassenger(int id)
        {
            return _passengers.SingleOrDefault(p => p.UserId == id).Seat.SeatId;
        }

        public void Add(Passenger passenger)
        {
            _passengers.Add(passenger);
        }

        public void Remove(Passenger passenger)
        {
            _passengers.Remove(passenger);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public Passenger GetBySeatNumber(int seat){
            return _passengers.Include(p => p.Seat)
                .FirstOrDefault(p => p.Seat.SeatId == seat);
        }
    }
}
