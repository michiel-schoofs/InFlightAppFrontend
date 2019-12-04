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
            string url = $"https://samples.openweathermap.org/data/2.5/weather?q={city},{country}&appid=6326ae48f0ab3d3df2909c877d21a633";
            string s = client.GetStringAsync(url).Result;
            JObject obj = JObject.Parse(s);


            return new Weather()
            {
                Cloudiness = double.Parse(obj.SelectToken("clouds").SelectToken("all").ToString()),
                Humidity = double.Parse(obj.SelectToken("main").SelectToken("humidity").ToString()),
                Pressure = double.Parse(obj.SelectToken("main").SelectToken("pressure").ToString()),
                Temp = double.Parse(obj.SelectToken("main").SelectToken("temp").ToString()) - 273.15,
                TempMax = double.Parse(obj.SelectToken("main").SelectToken("temp_max").ToString()) - 273.15,
                TempMin = double.Parse(obj.SelectToken("main").SelectToken("temp_min").ToString()) - 273.15,
                Description = obj.SelectToken("weather")[0].SelectToken("description").ToString(),
                WindSpeed = double.Parse(obj.SelectToken("wind").SelectToken("speed").ToString()),
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
