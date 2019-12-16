using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InFlightAppBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace InFlightAppBACKEND.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private DbSet<Order> _orders;
        private DBContext _dbContext;

        public OrderRepository(DBContext dbContext)
        {
            _orders = dbContext.Orders;
            _dbContext = dbContext;
        }

        public IEnumerable<Order> GetAll()
        {
            return _orders
                .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.Product)
                .ToList();
        }

        public IEnumerable<Order> GetAllByPassenger(Passenger pas){
           var orders= _orders.Include(o => o.Passenger).Include(o => o.OrderLines).ThenInclude(ol=>ol.Product);
           return orders.Where(o => o.Passenger.UserId == pas.UserId).ToList();
        }

        public IEnumerable<Order> GetAllUnprocessed()
        {
            return _orders
                .Where(o => !o.IsDone)
                .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.Product)
                .Include(o => o.Passenger)
                .ThenInclude(p => p.Seat)
                .ToList();
        }

        public Order GetById(int id)
        {
            return _orders
                .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.Product)
                .SingleOrDefault(o => o.OrderId == id);
        }

        public void Add(Order order)
        {
            _orders.Add(order);
        }

        public void Remove(Order order)
        {
            _orders.Remove(order);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
