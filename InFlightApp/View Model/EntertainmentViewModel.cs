using InFlightApp.Configuration;
using InFlightApp.Model;
using InFlightApp.Services.Interfaces;
using System;
using System.Collections.ObjectModel;
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
                LoadMovies();
                LoadSeries();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void LoadMovies()
        {
                Movies = new ObservableCollection<Movie>(_entertainmentService.GetMovies());
        }

        public void LoadSeries()
        {
            Series = new ObservableCollection<Serie>(_entertainmentService.GetSeries());
        }
    }
}
