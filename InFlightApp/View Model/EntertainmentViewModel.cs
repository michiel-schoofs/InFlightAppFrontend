using InFlightApp.Configuration;
using InFlightApp.Model;
using InFlightApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InFlightApp.View_Model
{
    public class EntertainmentViewModel
    {
        private readonly IEntertainmentService _entertainmentService;

        public ObservableCollection<Movie> Movies { get; set; }
        public ObservableCollection<Serie> Series { get; set; }

        public EntertainmentViewModel()
        {
            try
            {
                _entertainmentService = ServiceLocator.Current.GetService<IEntertainmentService>(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
