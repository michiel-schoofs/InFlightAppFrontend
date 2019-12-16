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

        public Weather GetCurrentWeather(string city, string country)
        {
            string url = $"https://samples.openweathermap.org/data/2.5/weather?q={city},{country}&appid=6326ae48f0ab3d3df2909c877d21a633";
            string s = ApiConnection.Client.GetStringAsync(url).Result;
            JObject obj = JObject.Parse(s);

            string desc = obj.SelectToken("weather")[0].SelectToken("description").ToString();
            if (!String.IsNullOrEmpty(desc)) { 
                desc = desc.First().ToString().ToUpper() + desc.Substring(1);
            }
            return new Weather() {
                Cloudiness = double.Parse(obj.SelectToken("clouds").SelectToken("all").ToString()),
                Humidity = double.Parse(obj.SelectToken("main").SelectToken("humidity").ToString()),
                Pressure = double.Parse(obj.SelectToken("main").SelectToken("pressure").ToString()),
                Temp = double.Parse(obj.SelectToken("main").SelectToken("temp").ToString()) - 273.15,
                TempMax = double.Parse(obj.SelectToken("main").SelectToken("temp_max").ToString()) - 273.15,
                TempMin = double.Parse(obj.SelectToken("main").SelectToken("temp_min").ToString()) - 273.15,
                Description = desc,
                WindSpeed = double.Parse(obj.SelectToken("wind").SelectToken("speed").ToString()),
                Icon = obj.SelectToken("weather")[0].SelectToken("icon").ToString(),
            };
        }

        public Flight GetFlight()
        {
            string url = $"{ApiConnection.URL}/Flight/info";
            string s = ApiConnection.Client.GetStringAsync(url).Result;
            return JObject.Parse(s).ToObject<Flight>();
        }

        public Persoon GetPassengerOnSeat(int seatID){
            string url = $"{ApiConnection.URL}/Flight/seats/{seatID}/user";
            string s = ApiConnection.Client.GetStringAsync(url).Result;

            JObject jObject = JObject.Parse(s);
            string voornaam = jObject.Value<string>("firstName");
            string achternaam = jObject.Value<string>("lastName");
            int id = jObject.Value<int>("id");

            return new Persoon(){
                FirstName = voornaam,
                LastName = achternaam,
                Id = id
            };
        }

        public IEnumerable<Seat> GetSeats()
        {
            string url = $"{ApiConnection.URL}/Flight/seats";
            string s = ApiConnection.Client.GetStringAsync(url).Result;
            JArray ar = JArray.Parse(s);

            return ar.Select(seat =>
            {
                return new Seat()
                {
                    SeatId = seat.Value<int>("seatID"),
                    Type = (SeatType)Enum.Parse(typeof(SeatType), seat.Value<string>("type")),
                    SeatCode = seat.Value<string>("seatCode")
                };
            }).OrderBy(se=>se.SeatCode).ToList();
        }

        public bool SeatHasUser(int id){
            string url = $"{ApiConnection.URL}/Flight/seats/{id}/user/exist";
            return bool.Parse(ApiConnection.Client.GetStringAsync(url).Result);
        }
    }
}
