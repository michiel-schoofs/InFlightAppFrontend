using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InFlightApp.Model
{
    public class Music
    {
        public string Title { get; set; }
        public int Duration { get; set; }
        public int Position { get; set; }
        public Artist Artist { get; set; }
        public Album Album { get; set; }

        public string GetDurationString()
        {
            return TimeSpan.FromSeconds(Duration).ToString(@":mm\:ss");
        }
    }

    public class Artist
    {
        public string Name { get; set; }

    }

    public class Album
    {
        public string Title { get; set; }
        public string Cover { get; set; }
    }
}
