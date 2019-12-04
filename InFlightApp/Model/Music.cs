using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InFlightApp.Model
{
    public class Music : INotifyPropertyChanged
    {
        public string Title { get; set; }
        public int Duration { get; set; }
        public int Position { get; set; }
        public Artist Artist { get; set; }
        public Album Album { get; set; }

        private bool _isPlaying;
        public bool IsPlaying {
            get {
                return _isPlaying;
            }
            set {
                if (value != _isPlaying)
                {
                    this._isPlaying = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string GetDurationString()
        {
            return TimeSpan.FromSeconds(Duration).ToString(@":mm\:ss");
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
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
