using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace InFlightApp.Model
{
    public class Product:INotifyPropertyChanged{
        public string ImagePath {
            get {
                if (ImageFile == null)
                    return "/Assets/img/products/default.jpg";

                return ImageFile;
            }
        }

        public string Color { 
            get {
                return (Amount > 0 ? "Green" : "Red");
            } 
        }

        public string AvailableText {
            get {
                return (Amount > 0 ? "Available" : "Not Available");
            }
        }

        private string imageFile;

        public string ImageFile {
            get {
                if (imageFile == null)
                    return "/Assets/img/products/default.jpg";
                return imageFile;
            }
            set {
                imageFile = value;
                RaisePropertyChanged("ImageFile");
            }
        }


        public int ProductID { get; set; }
        public decimal UnitPrice { get; set; }
        public int Amount{ get; set; }
        public string Name{ get; set; }
        public string Description { get; set; }
        public ProductType Type { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
