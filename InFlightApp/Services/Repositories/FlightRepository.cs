using InFlightApp.Model;
using InFlightApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InFlightApp.Services.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        public IEnumerable<Seat> GetSeats()
        {
            throw new NotImplementedException();
        }

        public string[] GetSeatTypes()
        {
            throw new NotImplementedException();
        }
    }
}
