using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InFlightApp.Model
{
    public class Weather
    {
        public double Cloudiness { get; set; }
        public double Humidity { get; set; }
        public double Pressure { get; set; }
        public double Temp { get; set; }
        public double TempMax { get; set; }
        public double TempMin { get; set; }
        public string Description { get; set; }
        public double WindSpeed { get; set; }
    }
}
