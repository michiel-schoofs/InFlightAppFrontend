using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace InFlightApp.Converters {
    public class StringFormatConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            try {
                string item = value.ToString();
                string format = (string)parameter;
                return String.Format(format, item);
            } catch (Exception e) {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
