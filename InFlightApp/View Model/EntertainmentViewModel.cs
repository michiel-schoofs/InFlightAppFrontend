using InFlightApp.Configuration;
using InFlightApp.Model;
using InFlightApp.Services.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace InFlightApp.View_Model
{
    public class EntertainmentViewModel
    {
        private readonly IEntertainmentService _entertainmentService;

        public ObservableCollection<Movie> Movies { get; set; }
        public ObservableCollection<Serie> Series { get; set; }
        public ObservableCollection<Music> Music { get; set; }

        public EntertainmentViewModel()
        {
            try
            {
                _entertainmentService = ServiceLocator.Current.GetService<IEntertainmentService>(true);
                Movies = new ObservableCollection<Movie>();
                Series = new ObservableCollection<Serie>();
                Music = new ObservableCollection<Music>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void LoadMovies()
        {
            if (Movies.Count == 0)
            {
                Movies = new ObservableCollection<Movie>(_entertainmentService.GetMovies());
            }
        }

        public void LoadDetailsMovie(string imdbID)
        {
            Movie movie = Movies.SingleOrDefault(m => m.imdbID.Equals(imdbID));
            if (!movie.DetailsLoaded)
            {
                Movie movieDetail = _entertainmentService.GetMovie(imdbID);
                movie.Released = movieDetail.Released;
                movie.Runtime = movieDetail.Runtime;
                movie.Genre = movieDetail.Genre;
                movie.Director = movieDetail.Director;
                movie.Actors = movieDetail.Actors;
                movie.Plot = movieDetail.Plot;
                movie.Language = movieDetail.Language;
                movie.Country = movieDetail.Country;
                movie.DetailsLoaded = true;
            }
        }

        public void LoadSeries()
        {
            if (Series.Count == 0)
            {
                Series = new ObservableCollection<Serie>(_entertainmentService.GetSeries());
            }
        }

        public void LoadMusic()
        {
            if (Music.Count == 0)
            {
                Music = new ObservableCollection<Music>(_entertainmentService.GetMusic());
            }
        }

        public void SetPlaying(Music song)
        {
            foreach (var track in Music)
            {
                track.IsPlaying = false;
            }
            song.IsPlaying = true;
        }

        public void SetNotPlaying()
        {
            foreach (var track in Music)
            {
                track.IsPlaying = false;
            }
        }
    }
}
