using System;
using InFlightAppBACKEND.Models.Domain;

namespace InFlightAppBACKEND.Models.DTO
{
    public class FlightInfoDTO
    {
        #region Properties
        public string FlightNr { get; set; }
        public string Plane { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public Origin Origin { get; set; }
        public Destination Destination { get; set; }
        public string Agency { get; set; }
        public int Altitude { get; set; }
        public int Speed { get; set; }
        #endregion

        #region Constructor
        public FlightInfoDTO(Flight fl)
        {
            FlightNr = fl.FlightNr;
            Plane = fl.Plane;
            DepartureTime = fl.DepartureTime;
            ArrivalTime = fl.ArrivalTime;
            Origin = fl.Origin;
            Destination = fl.Destination;
            Agency = fl.Agency;
            Speed = fl.Speed;
            Altitude = fl.Altitude;
        }
        #endregion
    }
}
