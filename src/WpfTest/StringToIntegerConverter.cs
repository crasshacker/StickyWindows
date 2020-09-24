using System;
using System.Globalization;
using System.Windows.Data;
using StickyWindows;

namespace WpfTest {
    public class StringToIntegerConverter : IValueConverter{
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return Int32.TryParse((string)value, out int intValue) ? intValue : 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return value.ToString();
        }
    }
}
