using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace EWPF.Attached
{
    /// <summary>
    /// A static class defining attached properties to handle automatic sorting in  a <see cref="GridView"/>.
    /// </summary>
    public static class GridViewSort
    {
        #region Nested Classes

        /// <inheritdoc />
        /// <summary>
        /// A class used to draw a sort glyph as an adorner.
        /// </summary>
        private class SortGlyphAdorner : Adorner
        {
            private readonly GridViewColumnHeader m_ColumnHeader;
            private readonly ListSortDirection m_Direction;
            private readonly ImageSource m_SortGlyph;

            public SortGlyphAdorner(GridViewColumnHeader i_ColumnHeader, ListSortDirection i_Direction,
                ImageSource i_SortGlyph)
                : base(i_ColumnHeader)
            {
                m_ColumnHeader = i_ColumnHeader;
                m_Direction = i_Direction;
                m_SortGlyph = i_SortGlyph;
            }

            private Geometry GetDefaultGlyph()
            {
                double x1 = m_ColumnHeader.ActualWidth - 13;
                double x2 = x1 + 10;
                double x3 = x1 + 5;
                double y1 = m_ColumnHeader.ActualHeight / 2 - 3;
                double y2 = y1 + 5;

                if (m_Direction == ListSortDirection.Ascending)
                {
                    double tmp = y1;
                    y1 = y2;
                    y2 = tmp;
                }

                var pathSegmentCollection = new PathSegmentCollection
                {
                    new LineSegment(new Point(x2, y1), true),
                    new LineSegment(new Point(x3, y2), true)
                };

                var pathFigure = new PathFigure(
                    new Point(x1, y1),
                    pathSegmentCollection,
                    true);

                var pathFigureCollection = new PathFigureCollection { pathFigure };

                var pathGeometry = new PathGeometry(pathFigureCollection);
                return pathGeometry;
            }

            protected override void OnRender(DrawingContext i_DrawingContext)
            {
                base.OnRender(i_DrawingContext);

                if (m_SortGlyph != null)
                {
                    double x = m_ColumnHeader.ActualWidth - 13;
                    double y = m_ColumnHeader.ActualHeight / 2 - 5;
                    var rect = new Rect(x, y, 10, 10);
                    i_DrawingContext.DrawImage(m_SortGlyph, rect);
                }
                else
                {
                    i_DrawingContext.DrawGeometry(Brushes.LightGray, new Pen(Brushes.Gray, 1.0),
                        GetDefaultGlyph());
                }
            }
        }

        #endregion

        #region Events

        #endregion

        #region Fields    

        #endregion

        #region Constructors

        #endregion

        #region Methods        

        #region Event Handlers

        #region Property Change Handlers

        /// <summary>
        /// Handles a change to the <see cref="CommandProperty"/> by adding a handler to the 
        /// <see cref="ButtonBase.ClickEvent"/> as necessary.
        /// </summary>
        /// <param name="i_DependencyObject">Source <see cref="ListView"/>.</param>
        /// <param name="i_E">Arguments indicating whether a 
        /// <see cref="ButtonBase.ClickEvent"/> should be handled or not.</param>
        private static void HandleCommandChanged(DependencyObject i_DependencyObject,
            DependencyPropertyChangedEventArgs i_E)
        {
            if (i_DependencyObject == null)
                throw new ArgumentNullException("i_DependencyObject",
                    @"Dependency object can't be null");
            var listView = i_DependencyObject as ItemsControl;
            if (listView == null)
                throw new InvalidCastException("Couldn't cast dependency object to an ItemsControl object");

            if (GetAutoSort(listView)) // Don't change click handler if auto sort is set
                return;

            if (i_E.OldValue != null && i_E.NewValue == null)
            {
                listView.RemoveHandler(ButtonBase.ClickEvent,
                    new RoutedEventHandler(ColumnHeaderClick));
            }
            if (i_E.OldValue == null && i_E.NewValue != null)
            {
                listView.AddHandler(ButtonBase.ClickEvent,
                    new RoutedEventHandler(ColumnHeaderClick));
            }
        }

        /// <summary>
        /// Handles a change to the <see cref="AutoSortProperty"/> by adding a handler to the
        /// <see cref="ButtonBase.ClickEvent"/> as necessary.
        /// </summary>
        /// <param name="i_DependencyObject">Source <see cref="ListView"/>.</param>
        /// <param name="i_E">Arguments indicating whether a 
        /// <see cref="ButtonBase.ClickEvent"/> should be handled or not.</param>
        private static void HandleAutoSortChanged(DependencyObject i_DependencyObject,
            DependencyPropertyChangedEventArgs i_E)
        {
            if (i_DependencyObject == null)
                throw new ArgumentNullException("i_DependencyObject",
                    @"Dependency object can't be null");
            var listView = i_DependencyObject as ListView;
            if (listView == null)
                throw new InvalidCastException("Couldn't cast dependency object to a ListView object"); ;

            if (GetCommand(listView) != null) // Don't change click handler if a command is set
                return;

            bool oldValue = (bool)i_E.OldValue;
            bool newValue = (bool)i_E.NewValue;
            if (oldValue && !newValue)
            {
                listView.RemoveHandler(ButtonBase.ClickEvent,
                    new RoutedEventHandler(ColumnHeaderClick));
            }
            if (!oldValue && newValue)
            {
                listView.AddHandler(ButtonBase.ClickEvent,
                    new RoutedEventHandler(ColumnHeaderClick));
            }
        }

        #endregion

        /// <summary>
        /// Handles the <see cref="ButtonBase.ClickEvent"/> of a <see cref="GridViewColumnHeader"/> 
        /// by sorting its' associated column's items.
        /// </summary>
        /// <param name="i_Sender">Irrelevant.</param>
        /// <param name="i_E">Arguments containing the original event's source, which is the column header.</param>
        private static void ColumnHeaderClick(object i_Sender, RoutedEventArgs i_E)
        {
            if (i_E == null)
                throw new ArgumentNullException("i_E", @"Event arguments can't be null");

            var clickedHeader = i_E.OriginalSource as GridViewColumnHeader;
            if (clickedHeader == null)
                throw new InvalidCastException("Couldn't cast event source to a GridViewColumnHeader object");
            if (clickedHeader.Column == null)
                throw new NullReferenceException("Header's associated column can't be null");

            string propertyName = GetPropertyName(clickedHeader.Column);
            if (string.IsNullOrEmpty(propertyName))
                return;

            var listView = GetAncestor<ListView>(clickedHeader);
            if (listView == null)
                return;

            SortColumn(clickedHeader, listView, propertyName);
        }

        private static T GetAncestor<T>(DependencyObject i_Reference) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(i_Reference);
            while (!(parent is T))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return (T)parent;
        }

        /// <summary>
        /// Sorts the given column's items by executing a sorting method. <br />
        /// The method can be a custom command or a built-in automatic sort.
        /// </summary>
        /// <param name="i_ColumnHeader">Column's header.</param>
        /// <param name="i_ParentListView">Column's parent <see cref="ListView"/>.</param>
        /// <param name="i_ListViewPropertyName">Parent <see cref="ListView"/>'s property name.</param>
        private static void SortColumn(GridViewColumnHeader i_ColumnHeader,
            ListView i_ParentListView, string i_ListViewPropertyName)
        {
            var command = GetCommand(i_ParentListView);
            if (command != null)
            {
                if (command.CanExecute(i_ListViewPropertyName))
                    command.Execute(i_ListViewPropertyName);
            }
            else if (GetAutoSort(i_ParentListView))
            {
                ApplySort(i_ParentListView.Items, i_ListViewPropertyName, i_ParentListView,
                    i_ColumnHeader);
            }
        }

        private static void ApplySort(ICollectionView i_View, string i_PropertyName, ListView i_ListView,
            GridViewColumnHeader i_SortedColumnHeader)
        {
            var direction = ListSortDirection.Ascending;
            if (i_View.SortDescriptions.Count > 0)
            {
                var currentSort = i_View.SortDescriptions[0];
                if (currentSort.PropertyName == i_PropertyName)
                {
                    direction = currentSort.Direction == ListSortDirection.Ascending
                                    ? ListSortDirection.Descending
                                    : ListSortDirection.Ascending;
                }
                i_View.SortDescriptions.Clear();

                var currentSortedColumnHeader = GetSortedColumnHeader(i_ListView);
                if (currentSortedColumnHeader != null)
                    RemoveSortGlyph(currentSortedColumnHeader);
            }

            if (string.IsNullOrEmpty(i_PropertyName))
                return;

            i_View.SortDescriptions.Add(new SortDescription(i_PropertyName, direction));
            if (GetShowSortGlyph(i_ListView))
                AddSortGlyph(
                    i_SortedColumnHeader,
                    direction,
                    direction == ListSortDirection.Ascending
                        ? GetAscendingSortGlyph(i_ListView)
                        : GetDescendingSortGlyph(i_ListView));
            SetSortedColumnHeader(i_ListView, i_SortedColumnHeader);
        }

        private static void AddSortGlyph(GridViewColumnHeader i_ColumnHeader,
            ListSortDirection i_Direction, ImageSource i_SortGlyph)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(i_ColumnHeader);
            adornerLayer.Add(
                new SortGlyphAdorner(
                    i_ColumnHeader,
                    i_Direction,
                    i_SortGlyph
                ));
        }

        private static void RemoveSortGlyph(GridViewColumnHeader i_ColumnHeader)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(i_ColumnHeader);
            var adorners = adornerLayer.GetAdorners(i_ColumnHeader);
            if (adorners == null)
                return;

            foreach (var adorner in adorners)
            {
                if (adorner is SortGlyphAdorner)
                    adornerLayer.Remove(adorner);
            }
        }

        #endregion

        #endregion

        #region Properties        

        #region Command

        /// <summary>
        /// Gets or sets a custom command used to sort the items of a single column.
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached(
                "Command",
                typeof(ICommand),
                typeof(GridViewSort),
                new UIPropertyMetadata(null, HandleCommandChanged));

        /// <summary>
        /// Gets a custom command used to sort the items of a single column.
        /// </summary>
        /// <param name="i_Obj">Source <see cref="GridView"/>.</param>
        /// <returns>Command to sort columns.</returns>
        public static ICommand GetCommand(DependencyObject i_Obj)
        {
            return (ICommand)i_Obj.GetValue(CommandProperty);
        }

        /// <summary>
        /// Sets a custom command used to sort the items of a single column.
        /// </summary>
        /// <param name="i_Obj">Source <see cref="GridView"/>.</param>
        /// <param name="i_Value">Command to sort columns.</param>
        public static void SetCommand(DependencyObject i_Obj, ICommand i_Value)
        {
            i_Obj.SetValue(CommandProperty, i_Value);
        }

        #endregion

        #region Auto Sort

        /// <summary>
        /// Gets or sets a boolean value indicating whether 
        /// the columns should be automatically sorted or not. <br />
        /// If the value is 'false', a custom sorting command is expected to be provided, 
        /// otherwise the columns will have no sorting abilities.
        /// </summary>
        public static readonly DependencyProperty AutoSortProperty =
            DependencyProperty.RegisterAttached(
                "AutoSort",
                typeof(bool),
                typeof(GridViewSort),
                new UIPropertyMetadata(default(bool), HandleAutoSortChanged));

        /// <summary>
        /// Gets a boolean value indicating whether the columns should be automatically sorted or not.
        /// </summary>
        /// <param name="i_Obj">Source <see cref="GridView"/>.</param>
        /// <returns>True if columns should be sorted automatically, false otherwise.</returns>
        public static bool GetAutoSort(DependencyObject i_Obj)
        {
            return (bool)i_Obj.GetValue(AutoSortProperty);
        }

        /// <summary>
        /// Sets a boolean value indicating whether the columns should be automatically sorted or not. <br />
        /// If the value is 'false', a custom sorting command is expected to be provided, 
        /// otherwise the columns will have no sorting abilities.
        /// </summary>
        /// <param name="i_Obj"><see cref="GridView"/>. to set its' <see cref="AutoSortProperty"/>.</param>
        /// <param name="i_Value">True if columns should be sorted automatically, false otherwise.</param>
        public static void SetAutoSort(DependencyObject i_Obj, bool i_Value)
        {
            i_Obj.SetValue(AutoSortProperty, i_Value);
        }

        #endregion

        #region Property Name

        /// <summary>
        /// Gets or sets a property name of some <see cref="DependencyObject"/>. <br />
        /// This property is completely generic and is used only to support custom commands.
        /// </summary>
        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.RegisterAttached(
                "PropertyName",
                typeof(string),
                typeof(GridViewSort),
                new UIPropertyMetadata(null)
            );

        /// <summary>
        /// Gets the property name of the given object.
        /// </summary>
        /// <param name="i_Obj">Source object to get its property name from.</param>
        /// <returns>Object's property name.</returns>
        public static string GetPropertyName(DependencyObject i_Obj)
        {
            return (string)i_Obj.GetValue(PropertyNameProperty);
        }

        /// <summary>
        /// Sets the property name of the given object.
        /// </summary>
        /// <param name="i_Obj">Source object to set its property name.</param>
        /// <param name="i_Value">Property name to set.</param>
        public static void SetPropertyName(DependencyObject i_Obj, string i_Value)
        {
            i_Obj.SetValue(PropertyNameProperty, i_Value);
        }

        #endregion

        #region Show Sort Glyph

        /// <summary>
        /// Gets or sets a value indicating whether a sort glyph should be displayed or not.
        /// </summary>
        public static readonly DependencyProperty ShowSortGlyphProperty =
            DependencyProperty.RegisterAttached("ShowSortGlyph", typeof(bool), typeof(GridViewSort),
                new UIPropertyMetadata(true));

        /// <summary>
        /// Gets a value indicating whether a sort glyph should be displayed or not. 
        /// </summary>
        /// <param name="i_Obj">Source <see cref="GridView"/>.</param>
        /// <returns>True if glyph should be displayed, false otherwise.</returns>
        public static bool GetShowSortGlyph(DependencyObject i_Obj)
        {
            return (bool)i_Obj.GetValue(ShowSortGlyphProperty);
        }

        /// <summary>
        /// Sets a value indicating whether a sort glyph should be displayed or not. 
        /// </summary>
        /// <param name="i_Obj">Source <see cref="GridView"/>.</param>
        /// <param name="i_Value">True to display glyph, false otherwise.</param>
        public static void SetShowSortGlyph(DependencyObject i_Obj, bool i_Value)
        {
            i_Obj.SetValue(ShowSortGlyphProperty, i_Value);
        }

        #endregion

        #region Ascending Sort Glyph

        /// <summary>
        /// Gets or sets the glyph displayed when <see cref="GridView"/>'s sort is ascending.
        /// </summary>
        public static readonly DependencyProperty AscendingSortGlyphProperty =
            DependencyProperty.RegisterAttached("AscendingSortGlyph", typeof(ImageSource),
                typeof(GridViewSort), new UIPropertyMetadata(null));

        /// <summary>
        /// Gets the glyph displayed when <see cref="GridView"/>'s sort is ascending.
        /// </summary>
        /// <param name="i_Obj">Source <see cref="GridView"/>.</param>
        /// <returns>Ascending glyph as an image.</returns>
        public static ImageSource GetAscendingSortGlyph(DependencyObject i_Obj)
        {
            return (ImageSource)i_Obj.GetValue(AscendingSortGlyphProperty);
        }

        /// <summary>
        /// Sets the glyph displayed when <see cref="GridView"/>'s sort is ascending.
        /// </summary>
        /// <param name="i_Obj">Source <see cref="GridView"/>.</param>
        /// <returns>Ascending glyph as an image.</returns>
        public static void SetAscendingSortGlyph(DependencyObject i_Obj, ImageSource i_Value)
        {
            i_Obj.SetValue(AscendingSortGlyphProperty, i_Value);
        }

        #endregion

        #region Descending Sort Glyph

        /// <summary>
        /// Gets or sets the glyph displayed when <see cref="GridView"/>'s sort is descending.
        /// </summary>
        public static readonly DependencyProperty DescendingSortGlyphProperty =
            DependencyProperty.RegisterAttached("DescendingSortGlyph", typeof(ImageSource),
                typeof(GridViewSort), new UIPropertyMetadata(null));

        /// <summary>
        /// Gets the glyph displayed when <see cref="GridView"/>'s sort is descending.
        /// </summary>
        /// <param name="i_Obj">Source <see cref="GridView"/>.</param>
        /// <returns>Descending glyph as an image.</returns>
        public static ImageSource GetDescendingSortGlyph(DependencyObject i_Obj)
        {
            return (ImageSource)i_Obj.GetValue(DescendingSortGlyphProperty);
        }

        /// <summary>
        /// Sets the glyph displayed when <see cref="GridView"/>'s sort is descending.
        /// </summary>
        /// <param name="i_Obj">Source <see cref="GridView"/>.</param>
        /// <param name="i_Value">Descending glyph as an image.</param>
        public static void SetDescendingSortGlyph(DependencyObject i_Obj, ImageSource i_Value)
        {
            i_Obj.SetValue(DescendingSortGlyphProperty, i_Value);
        }

        #endregion

        #region Sorted Column Header

        /// <summary>
        /// Gets or sets the header of a sorted <see cref="GridViewColumn"/>.
        /// </summary>
        private static readonly DependencyProperty sm_SortedColumnHeaderProperty =
            DependencyProperty.RegisterAttached(
                "sm_SortedColumnHeader",
                typeof(GridViewColumnHeader),
                typeof(GridViewSort), new UIPropertyMetadata(null));

        /// <summary>
        /// Gets the header of a sorted <see cref="GridViewColumn"/>.
        /// </summary>
        /// <param name="i_Obj">Source <see cref="ListView"/>.</param>
        /// <returns>Sorted column's header.</returns>
        private static GridViewColumnHeader GetSortedColumnHeader(DependencyObject i_Obj)
        {
            return (GridViewColumnHeader)i_Obj.GetValue(sm_SortedColumnHeaderProperty);
        }

        /// <summary>
        /// Sets the header of a sorted <see cref="GridViewColumn"/>.
        /// </summary>
        /// <param name="i_Obj">Source <see cref="ListView"/>.</param>
        /// <param name="i_Value">Sorted column's header.</param>
        private static void SetSortedColumnHeader(DependencyObject i_Obj, GridViewColumnHeader i_Value)
        {
            i_Obj.SetValue(sm_SortedColumnHeaderProperty, i_Value);
        }

        #endregion

        #endregion
    }
}