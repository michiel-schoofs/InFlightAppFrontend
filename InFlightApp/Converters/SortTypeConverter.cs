using InFlightApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace InFlightApp.Converters{
    public class SortTypeConverter:IValueConverter{
        public object Convert(object value, Type targetType, object parameter, string language){
            return Enum.GetName(typeof(SortType), ((SortType)value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language){
            return Enum.Parse(typeof(SortType), (string)value);
        }
    }
}
