using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;

namespace EWPF.Attached
{
    /// <summary>
    /// A static class defining various extensions to the <see cref="GridView"/> control 
    /// in the form of attached properties.
    /// </summary>
    public static class GridViewExtensions
    {
        #region Events

        #endregion

        #region Fields



        #endregion

        #region Constructors

        #endregion

        #region Methods

        /// <summary>
        /// Sets a value indicating whether the grid's columns should be stretched - 
        /// Evenly sized across the entire <see cref="GridView"/>.
        /// </summary>
        public static void SetStretchedColumns(DependencyObject i_Element, bool i_Value)
        {
            if (i_Element == null)
                throw new ArgumentNullException("i_Element", @"Element can't be null");
            i_Element.SetValue(StretchedColumnsProperty, i_Value);
        }

        /// <summary>
        /// Gets a value indicating whether the grid's columns should be stretched - 
        /// Evenly sized across the entire <see cref="GridView"/>.
        /// </summary>
        public static bool GetStretchedColumns(DependencyObject i_Element)
        {
            if (i_Element == null)
                throw new ArgumentNullException("i_Element", @"Element can't be null");
            return (bool)i_Element.GetValue(StretchedColumnsProperty);
        }

        /// <summary>
        /// Handle a change to the <see cref="StretchedColumnsProperty"/> 
        /// by stretching all grid's columns evenly when requested to, 
        /// and reverting to normal when not.
        /// </summary>
        /// <param name="i_DependencyObject">Source <see cref="ListView"/>.</param>
        /// <param name="i_E">Arguments containing the new property value.</param>
        private static void IsStretchedColumnsChanged(DependencyObject i_DependencyObject,
            DependencyPropertyChangedEventArgs i_E)
        {
            Application.Current.Dispatcher.Invoke(async () =>
            {
                await Task.Yield();

                var sourceListView = i_DependencyObject as ListView;
                if (sourceListView == null)
                {
                    throw new ArgumentException(@"Expected source object to be a GridView",
                        "i_DependencyObject");
                }

                var newValue = i_E.NewValue;
                if (newValue == null)
                    throw new ArgumentNullException("i_E", @"New value can't be null");
                bool isStretchedColumns = (bool)newValue;

                var gridView = sourceListView.View as GridView;
                if (gridView == null)
                    throw new InvalidCastException(
                        @"Couldn't cast ListView's view to a GridView object");

                var columnWidthBinding = CreateGridColumnWidthMultiBinding(sourceListView);
                foreach (var gridViewColumn in gridView.Columns)
                {
                    if (isStretchedColumns)
                    {
                        BindingOperations.SetBinding(gridViewColumn, GridViewColumn.WidthProperty,
                            columnWidthBinding);
                    }
                    else
                    {
                        BindingOperations.ClearBinding(gridViewColumn, GridViewColumn.WidthProperty);
                    }
                }
            }, DispatcherPriority.ApplicationIdle);
        }

        /// <summary>
        /// Creates a multi-binding using the given <see cref="ListView"/> 
        /// by creating a binding to its <see cref="GridView"/>'s <see cref="GridView.Columns"/> count property 
        /// and its <see cref="FrameworkElement.ActualWidth"/> property.
        /// </summary>
        /// <param name="i_SourceGridView">ListView to use as the binding source.</param>
        /// <returns>Created multi-binding.</returns>
        private static MultiBinding CreateGridColumnWidthMultiBinding(ListView i_SourceGridView)
        {
            var widthMultiBinding = new MultiBinding
            {
                Converter = new GridViewColumnDataToColumnWidthConverter()
            };
            widthMultiBinding.Bindings.Add(CreateGridColumnsCountBinding((GridView)i_SourceGridView.View));
            widthMultiBinding.Bindings.Add(CreateListViewActualWidthBinding(i_SourceGridView));

            return widthMultiBinding;
        }

        /// <summary>
        /// Creates a binding using the given <see cref="GridView"/> to its
        /// <see cref="GridView.Columns"/> count property.
        /// </summary>
        /// <param name="i_SourceGridView">Object to use as the binding source.</param>
        /// <returns>Created binding.</returns>
        private static Binding CreateGridColumnsCountBinding(GridView i_SourceGridView)
        {
            return new Binding("Columns.Count") { Source = i_SourceGridView };
        }

        /// <summary>
        /// Creates a binding using the given <see cref="ListView"/> to its  
        /// <see cref="FrameworkElement.ActualWidth"/> property.
        /// </summary>
        /// <param name="i_SourceListView">Object to use as the binding source.</param>
        /// <returns>Created binding.</returns>
        private static Binding CreateListViewActualWidthBinding(ListView i_SourceListView)
        {
            return new Binding("ActualWidth") { Source = i_SourceListView };
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the grid's columns should be stretched - 
        /// Evenly sized across the entire <see cref="GridView"/>.
        /// </summary>
        public static readonly DependencyProperty StretchedColumnsProperty =
            DependencyProperty.RegisterAttached(
                "StretchedColumns", typeof(bool), typeof(GridViewExtensions),
                new FrameworkPropertyMetadata(default(bool),
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsArrange, IsStretchedColumnsChanged));

        #endregion
    }

    /// <inheritdoc />
    /// <summary>
    /// A converter class used to convert a <see cref="T:System.Windows.Controls.GridView" />'s data, 
    /// represented as the number of columns it has and its' 
    /// <see cref="P:System.Windows.FrameworkElement.ActualWidth" />, 
    /// to the <see cref="P:System.Windows.FrameworkElement.Width" /> of a single column.
    /// </summary>
    public class GridViewColumnDataToColumnWidthConverter : IMultiValueConverter
    {
        #region Fields

        private const double cm_MINIMAL_WIDTH_MARGIN = 10;

        #endregion

        #region Implementation of IMultiValueConverter

        public object Convert(object[] values, Type targetType, object parameter,
            CultureInfo culture)
        {
            if (values == null)
                throw new ArgumentNullException("values", @"Values can't be null");
            if (values.Length != 2)
            {
                throw new ArgumentException(
                    string.Format("Expected exactly 2 values, got {0} instead", values.Length),
                    "values");
            }
            if (!(values[0] is int))
            {
                throw new ArgumentException(
                    string.Format("Expected first value to be an int, instead it's a {0}",
                        values[0].GetType()), "values");
            }
            if (!(values[1] is double))
            {
                throw new ArgumentException(
                    string.Format("Expected first value to be a double, instead it's a {0}",
                        values[0].GetType()), "values");
            }

            int numberOfGridViewColumns = (int)values[0];
            double gridViewTotalActualWidth = (double)values[1] - cm_MINIMAL_WIDTH_MARGIN;
            return gridViewTotalActualWidth < 0 ? 0 : gridViewTotalActualWidth / numberOfGridViewColumns;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        { throw new NotImplementedException(); }

        #endregion
    }
}