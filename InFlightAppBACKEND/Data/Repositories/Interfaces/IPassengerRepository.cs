using InFlightAppBACKEND.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InFlightAppBACKEND.Data.Repositories
{
    public interface IPassengerRepository
    {
        IEnumerable<Passenger> GetAll();
        IEnumerable<Order> GetOrdersFromPassenger(int id);
        Passenger GetById(int id);
        Passenger GetBySeatNumber(int seat);
        int GetSeatNumberFromPassenger(int id);
        void Add(Passenger passenger);
        void Remove(Passenger passenger);
        void SaveChanges();
    }
}
