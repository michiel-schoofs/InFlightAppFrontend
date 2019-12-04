using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InFlightApp.Model
{
    public class Flight
    {
        public string FlightNr { get; set; }
        public string Plane { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public TimeSpan Eta {
            get { return ArrivalTime - DepartureTime; }
        }
        public Location Origin { get; set; }
        public Location Destination { get; set; }
        public string Agency { get; set; }
        public int Altitude { get; set; }
        public int Speed { get; set; }
    }

    public class Location
    {
        public string IATA { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
