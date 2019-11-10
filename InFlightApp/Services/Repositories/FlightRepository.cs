using InFlightApp.Model;
using InFlightApp.Services.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InFlightApp.Services.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private HttpClient client;

        public FlightRepository()
        {
            client = ApiConnection.Client;
        }

        public IEnumerable<Seat> GetSeats()
        {
            string url = $"{ApiConnection.URL}/Flights/seats}";
            string s = client.GetStringAsync(url).Result;
            JArray ar = JArray.Parse(s);

            return ar.Select(seat =>
            {
                return new Seat()
                {
                    SeatId = seat.Value<int>("seatID"),
                    Type = (SeatType)Enum.Parse(typeof(SeatType), seat.Value<string>("type")),
                    SeatCode = seat.Value<string>("seatCode")
                };
            }).toList();
        }

    }
}
