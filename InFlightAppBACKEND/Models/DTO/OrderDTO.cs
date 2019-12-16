using System;
using System.Collections.Generic;
using System.Linq;
using InFlightAppBACKEND.Models.Domain;

namespace InFlightAppBACKEND.Models.DTO
{
    public class OrderDTO{
        public int OrderID { get; set; }
        public bool IsDone { get; set; }
        public DateTime OrderDate { get; set; }
        public PassengerDTO Passenger { get; set; }
        public IEnumerable<OrderLineDTO> OrderLines { get; set; }

        public OrderDTO(Order order){
            OrderID = order.OrderId;
            IsDone = order.IsDone;
            OrderDate = order.OrderDate;
            OrderLines = order.OrderLines.Select(ol => new OrderLineDTO(ol)).ToList();
            if (order.Passenger != null) {
                Passenger = new PassengerDTO(order.Passenger);
            }
        }

        public OrderDTO(){}
    }
}
