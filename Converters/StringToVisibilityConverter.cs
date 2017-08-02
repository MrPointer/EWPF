using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EWPF.Converters
{
    /// <summary>
    /// Converter used to convert strings to visibility state of a WPF element.
    /// </summary>
    public class StringToVisibilityConverter : IValueConverter
    {
        #region Methods

        /// <summary>Converts a string to a visibility state of a WPF element.</summary>
        /// <returns><see cref="Visibility.Collapsed"/> if string is null or empty, 
        /// <see cref="Visibility.Visible"/> otherwise.</returns>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string stringValue = (string)value;
            return string.IsNullOrEmpty(stringValue) ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <summary>Converts a value. </summary>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}