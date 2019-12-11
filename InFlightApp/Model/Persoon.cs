using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.System;

namespace InFlightApp.Model{
    public class Persoon: INotifyPropertyChanged{
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Id { get; set; }
        public PassengerType? Type { get; set;  }

        private string imageFile;
        public string ImageFile {
            get {
                if (imageFile == null)
                    return "/Assets/img/users/defaultuser.png";
                return imageFile;
            }
            set {
                imageFile = value;
                RaisePropertyChanged("ImageFile");
            }
        }

        public override string ToString() {
            return $"{FirstName} {LastName}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName]string propertyName = ""){
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
