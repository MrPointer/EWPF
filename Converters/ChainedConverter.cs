using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace EWPF.Converters
{
    /// <summary>
    /// A class used to convert one value to another using multiple converters, 
    /// chained one after another, ordered.
    /// </summary>
    [ContentProperty("ConverterCollection")]
    [ContentWrapper(typeof(ValueConverterCollection))]
    public class ChainedConverter : IValueConverter
    {
        #region Fields

        private readonly ValueConverterCollection m_ConverterCollection =
            new ValueConverterCollection();

        #endregion

        #region Methods

        ///<inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConverterCollection.Aggregate(value,
                (current, converter) => converter.Convert(current, targetType, parameter, culture));
        }

        ///<inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            return ConverterCollection
                .Reverse()
                .Aggregate(value,
                    (current, converter) => converter.Convert(current, targetType, parameter,
                        culture));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a collection of <see cref="IValueConverter"/>s that need to be aggregated 
        /// in order to return a final result.
        /// </summary>
        public ValueConverterCollection ConverterCollection
        {
            get { return m_ConverterCollection; }
        }

        #endregion
    }

    /// <summary>
    /// Represents a collection of <see cref="IValueConverter"/>s.
    /// </summary>
    public sealed class ValueConverterCollection : Collection<IValueConverter>
    {

    }
}