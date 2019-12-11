using InFlightApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace InFlightApp.Converters {
    public class ProductTypeConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            try {
                
                ProductType item = Enum.Parse<ProductType>((string)value);
                var resourceBundle = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

                return resourceBundle.GetString(item.ToString());
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
