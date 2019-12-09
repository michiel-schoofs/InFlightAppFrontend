using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace InFlightApp.Converters {
    class DoubleConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            try {
                double item = (double)value;
                string format = (string)parameter;
                return String.Format(format, item.ToString("F1"));
            } catch {
                return null;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
