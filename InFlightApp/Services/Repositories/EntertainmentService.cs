using InFlightApp.Model;
using InFlightApp.Services.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace InFlightApp.Services.Repositories
{
    public class EntertainmentService : IEntertainmentService
    {
        private HttpClient client;

        public EntertainmentService()
        {
            client = ApiConnection.Client;
        }

        public IEnumerable<Movie> GetMovies()
        {
            ICollection<Movie> movies = new List<Movie>();

            // Want more movies? Add the url and add to the url[]
            string url1 = $"http://www.omdbapi.com/?apikey=bc156e83&s=harry+potter&type=movie";
            string url2 = $"http://www.omdbapi.com/?apikey=bc156e83&s=lord+of+the+rings&type=movie";
            string url3 = $"http://www.omdbapi.com/?apikey=bc156e83&s=star+wars&type=movie";
            string[] urls = { url1, url2, url3 };

            foreach (var url in urls)
            {
                string s = client.GetStringAsync(url).Result;
                JArray ar = JObject.Parse(s).Value<JArray>("Search");
                foreach (var e in ar)
                {
                    Movie currMovie = e.ToObject<Movie>();
                    movies.Add(currMovie);
                }
            }
            return movies;
        }

        public Movie GetMovie(string imdbID)
        {
            string url = $"http://omdbapi.com/?apikey=bc156e83&i={imdbID}";
            string s = client.GetStringAsync(url).Result;
            return JObject.Parse(s).ToObject<Movie>();
        }

        public IEnumerable<Serie> GetSeries()
        {
            ICollection<Serie> series = new List<Serie>();

            // Want more series? Add the url and add to the url[]
            string url1 = $"http://omdbapi.com/?apikey=bc156e83&i=tt0108778";
            string url2 = $"http://omdbapi.com/?apikey=bc156e83&i=tt0898266";
            string url3 = $"http://omdbapi.com/?apikey=bc156e83&i=tt1632701";
            string url4 = $"http://omdbapi.com/?apikey=bc156e83&i=tt2861424";
            string url5 = $"http://omdbapi.com/?apikey=bc156e83&i=tt0460649";
            string url6 = $"http://omdbapi.com/?apikey=bc156e83&i=tt0121955";
            string url7 = $"http://omdbapi.com/?apikey=bc156e83&i=tt2467372";
            string url8 = $"http://omdbapi.com/?apikey=bc156e83&i=tt2442560";
            string url9 = $"http://omdbapi.com/?apikey=bc156e83&i=tt0903747";
            string url10 = $"http://omdbapi.com/?apikey=bc156e83&i=tt0417299";

            string[] urls = { url1, url2, url3, url4, url5, url6, url7, url8, url9, url10 };

            foreach (var url in urls)
            {
                string s = client.GetStringAsync(url).Result;
                JObject obj = JObject.Parse(s);
                series.Add(obj.ToObject<Serie>());
            }
            return series;
        }


        public IEnumerable<Music> GetMusic()
        {
            ICollection<Music> music = new List<Music>();

            string url = $"https://api.deezer.com/chart";
            string s = client.GetStringAsync(url).Result;
            JObject rootObj = JObject.Parse(s).Value<JObject>("tracks");
            JArray ar = rootObj.Value<JArray>("data");
            foreach (var e in ar)
            {
                music.Add(e.ToObject<Music>());
            }
            return music;
        }
    }
}
