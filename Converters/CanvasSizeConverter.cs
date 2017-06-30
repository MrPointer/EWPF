using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace EWPF.Converters
{
    /// <summary>
    /// A converter class used to convert a size of a <see cref="Canvas"/> to its 
    /// <see cref="FrameworkElement.Width"/> and <see cref="FrameworkElement.Height"/>.
    /// </summary>
    public class CanvasSizeConverter : IMultiValueConverter
    {
        #region Methods

        /// <inheritdoc />
        public object Convert(object[] values, Type targetType, object parameter,
            CultureInfo culture)
        {
            if (values == null)
                throw new ArgumentNullException("values", @"Values collection can't be null");
            if (values.Length != 2)
            {
                throw new ArgumentOutOfRangeException("values",
                    @"Values collection must contain exactly 2 items");
            }
            if (!(values[0] is double) || !(values[1] is double))
            {
                throw new ArgumentException(@"Values collection must contain only double types",
                    "values");
            }

            double sizeValue = (double)values[0];
            if (sizeValue > 0)
                return sizeValue;
            double actualWidthValue = (double)values[1];
            return actualWidthValue;
        }

        /// <inheritdoc />
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}