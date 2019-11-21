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

        public string Glyph{
            get {
                return (Amount > 0 ? "Accept" : "Clear");
            }
        }

        public string Color {
            get {
                return (Amount > 0 ? "Lime" : "Red");
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

        private int _amount;

        public int Amount{
            get { return _amount; }
            set {
                _amount = value;
                RaisePropertyChanged("Amount");
                RaisePropertyChanged("Color");
                RaisePropertyChanged("Glyph");
            }
        }


        public int ProductID { get; set; }
        public decimal UnitPrice { get; set; }
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
