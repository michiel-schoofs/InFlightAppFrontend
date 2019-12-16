using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InFlightAppBACKEND.Models.Domain
{
    public class Seat
    {
        public int SeatId { get; set; }
        public int FlightId { get; set; }
        public int? PassengerId { get; set; }
        public SeatType Type { get; set; }
        public Passenger Passenger { get; set; }
        public string SeatCode { get; set; }

        protected Seat(){}

        public Seat(SeatType type, string seatCode) : this(type, null, seatCode) { }

        public Seat(SeatType type, Passenger passenger,string seatCode){
            Type = type;
            Passenger = passenger;
            SeatCode = seatCode;
        }
    }
}
