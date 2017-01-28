using System;
using System.Globalization;
using System.Windows.Data;

namespace EWPF.Converters
{
    /// <summary>
    /// a converter class used to convert a boolean value to its' opposite value.
    /// <para />
    /// For example: True will become false.
    /// </summary>
    public class InverseBoolConverter : IValueConverter
    {
        #region Methods

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value), @"Value to convert can't be null");
            bool boolValue = (bool)value;
            return !boolValue;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }

        #endregion
    }
}