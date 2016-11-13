using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EWPF.Converters
{
    /// <summary>
    /// A multi converter used to convert various visibillty values to the positive button's
    /// column position value inside the buttons grid of a message box.
    /// </summary>
    public class MBoxPButtonPositionConverter : IMultiValueConverter
    {
        #region Methods

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2 || !(values[0] is Visibility) || !(values[1] is Visibility))
                return null;
            var negativeButtonVisibility = (Visibility)values[0];
            var neutralButtonVisibility = (Visibility)values[1];
            if (negativeButtonVisibility != Visibility.Visible) // Negtive is invisble - Show only positive
                return 2;
            return neutralButtonVisibility != Visibility.Visible ? 1 : 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}