using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EWPF.Converters
{
    /// <summary>
    /// A converter class used to convert a <see cref="Visibility"/> value 
    /// to a <see cref="bool"/> value.
    /// </summary>
    public class VisibilityToBoolConverter : IValueConverter
    {
        #region Methods

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new ArgumentNullException("value", @"Value to convert can't be null");
            var visibilityValue = (Visibility)value;
            switch (visibilityValue)
            {
                case Visibility.Visible:
                    return true;
                default:
                    return false;
            }
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}