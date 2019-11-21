using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InFlightApp.Model
{
    public class Seat
    {
        public int SeatId { get; set; }
        public SeatType Type { get; set; }
        public string SeatCode { get; set; }
    }
}
