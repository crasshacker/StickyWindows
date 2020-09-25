using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using StickyWindows;

namespace WpfTest {
    public class StringToModifierKeyConverter : IValueConverter{
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return ((ComboBoxItem)value)?.Content switch
            {
                "None"    => StickyWindow.ModifierKey.None,
                "Control" => StickyWindow.ModifierKey.Control,
                "Shift"   => StickyWindow.ModifierKey.Shift,
                _         => StickyWindow.ModifierKey.None
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return value switch
            {
                StickyWindow.ModifierKey.None    => "None",
                StickyWindow.ModifierKey.Control => "Control",
                StickyWindow.ModifierKey.Shift   => "Shift",
                _                                => "None"
            };
        }
    }
}
