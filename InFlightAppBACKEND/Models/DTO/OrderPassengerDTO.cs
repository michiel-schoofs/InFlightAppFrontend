using System.Collections.Generic;
using InFlightAppBACKEND.Models.Domain;
using System.Linq;

namespace InFlightAppBACKEND.Models.DTO{
    public class OrderPassengerDTO{
       public PersonDTO Passenger { get; set; }
       public IEnumerable<OrderDTO> Orders { get; set; }

        public OrderPassengerDTO(Passenger pas){
            Passenger = new PersonDTO(pas);
            Orders = pas.Orders.Select(o => new OrderDTO(o)).ToList();
        }

        public OrderPassengerDTO(){}
    }
}
