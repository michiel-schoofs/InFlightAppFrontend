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
    public class FlightService : IFlightService
    {
        private HttpClient client;

        public FlightService()
        {
            client = ApiConnection.Client;
        }

        public Weather GetCurrentWeather(string city, string country)
        {
            string url = $"https://samples.openweathermap.org/data/2.5/weather?q={city},{country}&appid=b6907d289e10d714a6e88b30761fae22";
            string s = client.GetStringAsync(url).Result;
            JObject obj = JObject.Parse(s);

            return new Weather()
            {
                Cloudiness = obj.Value<double>("clouds.all"),
                Humidity = obj.Value<double>("main.humidity"),
                Pressure = obj.Value<double>("main.pressure"),
                Temp = obj.Value<double>("main.pressure"),
                TempMax = obj.Value<double>("main.temp_max"),
                TempMin = obj.Value<double>("main.temp_min"),
                //Description = obj.Value<string>("weather")[0].
                WindSpeed = obj.Value<double>("wind.speed")
            };
        }

        public Flight GetFlight()
        {
            string url = $"{ApiConnection.URL}/Flight/info";
            string s = client.GetStringAsync(url).Result;
            return JObject.Parse(s).ToObject<Flight>();
        }

        public IEnumerable<Seat> GetSeats()
        {
            string url = $"{ApiConnection.URL}/Flight/seats";
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
            }).ToList();
        }

    }
}
