using InFlightApp.Configuration;
using InFlightApp.Model;
using InFlightApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InFlightApp.View_Model
{
    public class InfoViewModel
    {
        private readonly IFlightService _flightService;

        public Flight Flight { get; set; }
        public Weather Weather { get; set; }

        public InfoViewModel()
        {
            try
            {
                _flightService = ServiceLocator.Current.GetService<IFlightService>(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void LoadFlightInfo()
        {

            Flight = _flightService.GetFlight();

        }

        public void LoadWeatherInfo()
        {
            Weather = _flightService.GetCurrentWeather(Flight.Destination.City, Flight.Destination.Country);
        }
    }
}
