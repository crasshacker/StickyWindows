using System;
using System.Globalization;
using System.Windows.Data;
using StickyWindows;

namespace WpfTest {
    public class IntegerToWindowTypeConverter : IValueConverter{
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return (StickyWindowType) value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return value;
        }
    }
}
