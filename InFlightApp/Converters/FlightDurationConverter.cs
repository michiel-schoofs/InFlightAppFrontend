using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace InFlightApp.Converters {
    class FlightDurationConverter : IValueConverter{
        public object Convert(object value, Type targetType, object parameter, string language) {
            try {
                TimeSpan ts = (TimeSpan)value;
                return ts.ToString(@"hh\:mm");
            } catch (Exception e) {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
