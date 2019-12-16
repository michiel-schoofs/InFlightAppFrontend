using InFlightAppBACKEND.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InFlightAppBACKEND.Data.Repositories
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetAll();
        IEnumerable<Order> GetAllUnprocessed();
        IEnumerable<Order> GetAllByPassenger(Passenger pas);
        Order GetById(int id);
        void Add(Order order);
        void Remove(Order order);
        void SaveChanges();
    }
}
