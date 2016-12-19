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

        /// <inheritdoc />
        public object Convert(object[] i_Values, Type i_TargetType, object i_Parameter, CultureInfo i_Culture)
        {
            if ((i_Values[0] != null && !(i_Values[0] is string)) || (i_Values[1] != null && !(i_Values[1] is string)))
                throw new ArgumentException("Given values are not of the expected type.");
            string negativeString = (string)i_Values[0];
            string neutralString = (string)i_Values[1];
            if (string.IsNullOrEmpty(negativeString) || string.IsNullOrEmpty(neutralString))
                return 2; // Always the 2nd (last) column
            return 0;
        }

        /// <inheritdoc />
        public object[] ConvertBack(object i_Value, Type[] i_TargetTypes, object i_Parameter, CultureInfo i_Culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}