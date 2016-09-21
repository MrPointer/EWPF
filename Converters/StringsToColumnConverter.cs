using System;
using System.Globalization;
using System.Windows.Data;

namespace EWPF.Converters
{
    /// <summary>
    /// A converter used to convert a collection of strings to a grid column value.
    /// </summary>
    public class StringsToColumnConverter : IMultiValueConverter
    {
        #region Methods

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if ((values[0] != null && !(values[0] is string)) || (values[1] != null && !(values[1] is string)))
                throw new ArgumentException("Given values are not of the expected type.");
            string negativeString = (string)values[0];
            string neutralString = (string)values[1];
            if (string.IsNullOrEmpty(negativeString) || string.IsNullOrEmpty(neutralString))
                return 2; // Always the 2nd (last) column
            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}