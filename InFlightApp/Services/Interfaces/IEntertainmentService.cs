using InFlightApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InFlightApp.Services.Interfaces
{
    public interface IEntertainmentService
    {
        IEnumerable<Movie> GetMovies();
        Movie GetMovie(string imdbID);
        IEnumerable<Serie> GetSeries();
    }
}
