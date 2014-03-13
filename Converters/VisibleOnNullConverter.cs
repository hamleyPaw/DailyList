using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace HamleyPaw.DailyList.Converters {
    public class VisibleOnNullConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value != null) {
                return Visibility.Collapsed;
            } else {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw (new NotImplementedException());
        }
    }
}
