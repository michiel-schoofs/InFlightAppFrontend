using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace InFlightApp.Converters{
    public class MoneyConverter:IValueConverter{
        public object Convert(object value, Type targetType, object parameter, string language) {
            //Needed to get the current currency symbol.
            var ri = new RegionInfo(System.Threading.Thread.CurrentThread.CurrentUICulture.LCID);
            var currencySymb = ri.CurrencySymbol;

            decimal val = (decimal)value;
            return String.Format("{0:f} {1}", val, currencySymb);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
