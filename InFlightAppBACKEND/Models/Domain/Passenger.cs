using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InFlightAppBACKEND.Models.Domain
{
    public class Passenger : User
    {

        public string Username { get => $"{FirstName.Replace(' ', '_')}.{Seat.SeatId}"; }
        public string Password { get => $"{LastName.Replace(' ', '_')}.{Seat.SeatId}"; }

        public Seat Seat { get; set; }
        public TravelGroup TravelGroup { get; set; }
        public ICollection<Order> Orders { get; set; }

        protected Passenger() : base()
        {
            Orders = new List<Order>();
        }

        public Passenger(string firstName, string lastName)
        : base(firstName, lastName)
        {
            Orders = new List<Order>();
        }

        public override void AddOrder(Order order)
        {
            Orders.Add(order);
        }

        public override void RemoveOrder(Order order)
        {
            Orders.Remove(order);
        }

        public void sendMessage(string content)
        {
            if (TravelGroup != null)
            {
                TravelGroup.sendMessage(this, content);
            }
        }
    }
}