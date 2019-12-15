using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace InFlightApp.Converters {
    public class DateTimeConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            try {
                DateTime ts = (DateTime)value;
                return ts.ToString(@"HH\:mm MM/dd/yyyy");
            } catch (Exception e) {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
