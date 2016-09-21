using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EWPF.Converters
{
    /// <summary>
    /// A converter used to convert a flow direction to a horizontal alignment value.
    /// </summary>
    public class FlowDirToHAlignmentConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var flowDirection = (FlowDirection)value;
            return flowDirection == FlowDirection.LeftToRight ? HorizontalAlignment.Right : HorizontalAlignment.Left;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}