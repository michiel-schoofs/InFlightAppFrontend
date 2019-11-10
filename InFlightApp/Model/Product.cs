using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace InFlightApp.Model
{
    public class Product{
        public string ImagePath {
            get {
                if (ImageFile == null)
                    return "/Assets/img/products/default.jpg";

                return ImageFile;
            }
        }

        public int ProductID { get; set; }
        public decimal UnitPrice { get; set; }
        public int Amount{ get; set; }
        public string Name{ get; set; }
        public string Description { get; set; }
        public ProductType Type { get; set; }
        public string ImageFile { get; set; }
    }
}
