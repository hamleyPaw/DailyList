using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace HamleyPaw.DailyList.Converters
{
    public class LogicInverterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is bool)
            {
                return !((bool)value);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw (new NotImplementedException());
        }
    }
}
