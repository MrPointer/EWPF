using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EWPF.Converters
{
    /// <summary>
    /// A converter used to convert the visibility of the neutral button in a message box to the negative button's
    /// column position in the buttons grid.
    /// </summary>
    public class MBoxNButtonPositonConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var neutralButtonVisibility = (Visibility)value;
            return neutralButtonVisibility != Visibility.Visible ? 2 : 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}