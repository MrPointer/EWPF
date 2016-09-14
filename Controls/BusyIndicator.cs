using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace EWPF.Controls
{
    /// <summary>
    /// A custom control used to display a busy indication as a circular progress bar.
    /// </summary>
    public class BusyIndicator : Control
    {
        #region Fields

        private const string cm_PARENT_CANVAS_NAME = "PART_Canvas";
        private Canvas m_ParentCanvas;

        private Storyboard m_AnimationStoryboard;
        private BeginStoryboard m_AnimationBeginStoryboard;
        private StopStoryboard m_AnimationStopStoryboard;

        private double m_AngleBetweenPoints;

        private MultiBinding m_PointXCoordinateBinding;
        private MultiBinding m_PointYCoordinateBinding;
        private MultiBinding m_EllipseSizeBinding;
        private Binding m_StrokeBinding;
        private Binding m_OpacityBinding;

        private static bool m_IsInitialized;

        #endregion

        #region Dependency Properties

        #region Points

        /// <summary>
        /// Gets or sets the number of points to draw on the busy indicator's circle.
        /// </summary>
        public static readonly DependencyProperty PointsProperty = DependencyProperty.Register("Points", typeof(int), typeof(BusyIndicator),
                new FrameworkPropertyMetadata(default(int), OnPointsChanged));

        /// <summary>
        /// Number of points to draw on the busy indicator's circle.
        /// </summary>
        public int Points
        {
            get { return (int)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }

        #endregion

        #region Stroke

        /// <summary>
        /// Gets or sets the brush to use when filling the busy indicator circle's points.
        /// </summary>
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
            "Stroke", typeof(Brush), typeof(BusyIndicator), new PropertyMetadata(default(Brush)));

        /// <summary>
        /// Brush to use when filling the points.
        /// </summary>
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        #endregion

        #region Is Accelerated

        /// <summary>
        /// Gets or sets weather the busy indicator should apply an acceleration and deceleration animation.
        /// </summary>
        public static readonly DependencyProperty IsAcceleratedProperty = DependencyProperty.Register(
            "IsAccelerated", typeof(bool), typeof(BusyIndicator), new PropertyMetadata(default(bool)));

        /// <summary>
        /// Gets or sets a boolean value indicating weather the busy indicator should be accelerated and decelerated in it's animation.
        /// </summary>
        public bool IsAccelerated
        {
            get { return (bool)GetValue(IsAcceleratedProperty); }
            set { SetValue(IsAcceleratedProperty, value); }
        }

        #endregion

        #region Is Animated

        /// <summary>
        /// Gets or sets weather the progress bar's animation is active or not.
        /// </summary>
        public static readonly DependencyProperty IsAnimatedProperty = DependencyProperty.Register(
            "IsAnimated", typeof(bool), typeof(BusyIndicator), new FrameworkPropertyMetadata(default(bool), OnIsAnimatedChanged));

        /// <summary>
        /// Gets or sets a boolean value indicating weather the progress bar's animation is active or not.
        /// </summary>
        public bool IsAnimated
        {
            get { return (bool)GetValue(IsAnimatedProperty); }
            set { SetValue(IsAnimatedProperty, value); }
        }

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs and registers the custom control among with it's dependency properties.
        /// </summary>
        static BusyIndicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BusyIndicator), new FrameworkPropertyMetadata(typeof(BusyIndicator)));
        }

        #endregion

        #region Methods

        #region Event Handlers

        /// <summary>
        /// Handles an event raised when the enclosing canvas's visibility has changed - 
        /// It stops the busy indicator's animation if the canvas becomes invisible and the animation is still running.
        /// </summary>
        /// <param name="sender">Enclosing Canvas.</param>
        /// <param name="e">Irrelevant due to a bug in WPF's system.</param>
        private void ParentCanvasIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            bool visibilityState = m_ParentCanvas.IsVisible;
            if (visibilityState) return;
            if (m_AnimationStoryboard == null) return;
            m_AnimationStoryboard.Stop();
            m_AnimationStoryboard = null;
        }

        #endregion

        #region Callback Methods

        /// <summary>
        /// Handles an event raised when the number of points to draw has changed.
        /// </summary>
        /// <param name="i_Sender">This class.</param>
        /// <param name="i_EventArgs">Event args for this method containing the old and new values of the number of points.</param>
        private static void OnPointsChanged(DependencyObject i_Sender, DependencyPropertyChangedEventArgs i_EventArgs)
        {
            var instance = i_Sender as BusyIndicator;
            if (instance == null)
                return;
            instance.m_AngleBetweenPoints = 360.0 / (int)i_EventArgs.NewValue;
            if (!m_IsInitialized) return; // Do not proceed if control is not yet initialized
            instance.InvalidateCanvas();
        }

        /// <summary>
        /// Handles an event raised when IsAnimation property has changed, 
        /// meaning when the animating state of the busy indicator has changed.
        /// </summary>
        /// <param name="i_Sender">This class.</param>
        /// <param name="i_EventArgs">Event args for this method containing the new value of the IsAnimated property.</param>
        private static void OnIsAnimatedChanged(DependencyObject i_Sender, DependencyPropertyChangedEventArgs i_EventArgs)
        {
            var instance = i_Sender as BusyIndicator;
            if (instance == null)
                return;
            bool isAnimated = (bool)i_EventArgs.NewValue;
            if (isAnimated) // Animation should start
                instance.StartAnimation();
            else
                instance.StopAnimation();
        }

        /// <summary>
        /// Called upon control invalidation, usually before the enclosing control is loaded.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            m_ParentCanvas = GetTemplateChild(cm_PARENT_CANVAS_NAME) as Canvas;
            if (m_ParentCanvas == null) return;
            m_ParentCanvas.IsVisibleChanged += ParentCanvasIsVisibleChanged;

            CreateAnimationStoryBoard();
            CreatePointBindings(); // Create all necessary bindings
            InvalidateCanvas();

            if (IsAnimated)
                StartAnimation();

            m_IsInitialized = true;
        }

        #endregion

        #region Control-Related Methods

        /// <summary>
        /// Invalidates the enclosing canvas by clearing its' children and adding new ones instead.
        /// </summary>
        private void InvalidateCanvas()
        {
            m_ParentCanvas.Children.Clear();
            for (int i = 0; i < Points; i++)
            {
                var newPoint = CreatePoint(i);
                m_ParentCanvas.Children.Add(newPoint);
            }
        }

        /// <summary>
        /// Creates a new ellipse representing a point on the busy indicator's circle based on it's ordinal value among other points.
        /// </summary>
        /// <param name="i_PointIndex">Point's index in the series of points constructing buys indicator's circle.</param>
        /// <returns>Ellipse object representing a point on the busy indicator's circle.</returns>
        private Ellipse CreatePoint(int i_PointIndex)
        {
            var ellipse = new Ellipse { Stretch = Stretch.Fill, Tag = i_PointIndex };
            ellipse.SetBinding(WidthProperty, m_EllipseSizeBinding);
            ellipse.SetBinding(HeightProperty, m_EllipseSizeBinding);
            ellipse.SetBinding(Canvas.LeftProperty, m_PointXCoordinateBinding);
            ellipse.SetBinding(Canvas.TopProperty, m_PointYCoordinateBinding);
            ellipse.SetBinding(Shape.FillProperty, m_StrokeBinding);
            ellipse.SetBinding(OpacityProperty, m_OpacityBinding);
            return ellipse;
        }

        /// <summary>
        /// Creates and stores binding objects to bind a point in the busy indicator's circle to a location on the enclosing canvas.
        /// </summary>
        private void CreatePointBindings()
        {
            #region Size

            m_EllipseSizeBinding = new MultiBinding { Converter = new CanvasSizeToRadiusConverter() };
            m_EllipseSizeBinding.Bindings.Add(new Binding("Width") { Source = m_ParentCanvas });
            m_EllipseSizeBinding.Bindings.Add(new Binding("Height") { Source = m_ParentCanvas });

            #endregion

            #region X Coordinate

            m_PointXCoordinateBinding = new MultiBinding
                {
                    Converter = new PointToCanvasLeftConverter(),
                    ConverterParameter = m_AngleBetweenPoints
                };

            m_PointXCoordinateBinding.Bindings.Add(new Binding("Width") { Source = m_ParentCanvas });
            m_PointXCoordinateBinding.Bindings.Add(new Binding("Height") { Source = m_ParentCanvas });
            m_PointXCoordinateBinding.Bindings.Add(new Binding("Tag") { RelativeSource = RelativeSource.Self });
            m_PointXCoordinateBinding.Bindings.Add(new Binding("Width") { RelativeSource = RelativeSource.Self });

            #endregion

            #region Y Coordinate

            m_PointYCoordinateBinding = new MultiBinding
                {
                    Converter = new PointToCanvasTopConverter(),
                    ConverterParameter = m_AngleBetweenPoints
                };

            m_PointYCoordinateBinding.Bindings.Add(new Binding("Width") { Source = m_ParentCanvas });
            m_PointYCoordinateBinding.Bindings.Add(new Binding("Height") { Source = m_ParentCanvas });
            m_PointYCoordinateBinding.Bindings.Add(new Binding("Tag") { RelativeSource = RelativeSource.Self });
            m_PointYCoordinateBinding.Bindings.Add(new Binding("Height") { RelativeSource = RelativeSource.Self });

            #endregion

            #region Stroke

            m_StrokeBinding = new Binding("Stroke")
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor) { AncestorType = typeof(BusyIndicator) }
                };

            #endregion

            #region Opacity

            m_OpacityBinding = new Binding
            {
                Path = new PropertyPath("Tag"),
                RelativeSource = RelativeSource.Self,
                Converter = new PointIndexToOpacityConverter(),
                ConverterParameter = Points
            };

            #endregion
        }

        #endregion

        #region Animation-Related

        /// <summary>
        /// Creates the busy indicator's storyboard, which controls the animation process.
        /// </summary>
        private void CreateAnimationStoryBoard()
        {
            m_AnimationStoryboard = new Storyboard();
            var renderTransformAnimation = new DoubleAnimation
            {
                AccelerationRatio = IsAccelerated ? 0.4 : 0,
                DecelerationRatio = IsAccelerated ? 0.6 : 0,
                Duration = new Duration(TimeSpan.FromSeconds(1.0).Add(TimeSpan.FromMilliseconds(200.0))),
                From = 0.0,
                To = 360.0,
                RepeatBehavior = RepeatBehavior.Forever
            };
            m_AnimationStoryboard.Children.Add(renderTransformAnimation);
            Storyboard.SetTarget(renderTransformAnimation, m_ParentCanvas);
            Storyboard.SetTargetProperty(renderTransformAnimation, new PropertyPath("RenderTransform.Angle"));
        }

        /// <summary>
        /// Starts the animation bound to the busy indicator, changing its' visibility to Visibility.Visible if needed.
        /// </summary>
        public void StartAnimation()
        {
            if (m_AnimationStoryboard == null)
                return;
            if (Visibility != Visibility.Visible) // Control isn't visible
                Visibility = Visibility.Visible;
            m_AnimationStoryboard.Begin();
        }

        /// <summary>
        /// Stops the animation bound to the busy indicator and sets its' visibility state to <see cref="Visibility.Hidden"/>.
        /// </summary>
        public void StopAnimation()
        {
            if (m_AnimationStoryboard == null)
                return;
            m_AnimationStoryboard.Stop();
            Visibility = Visibility.Hidden;
        }

        #endregion

        #endregion
    }

    #region Converters

    #region Point To Canvas Left

    /// <summary>
    /// Multi converter used to convert a busy indicator's point to Canvas.Left value, meaning the point's X coordinate.
    /// </summary>
    public class PointToCanvasLeftConverter : IMultiValueConverter
    {
        /// <summary>
        /// Converts source values to a value for the binding target. The data binding engine calls this method 
        /// when it propagates the values from source bindings to the binding target.</summary>
        /// <returns>A converted value.If the method returns null, the valid null value is used.
        /// A return value of <see cref="T:System.Windows.DependencyProperty" />.
        /// <see cref="F:System.Windows.DependencyProperty.UnsetValue" /> indicates that the converter did not produce a value, 
        /// and that the binding will use the <see cref="P:System.Windows.Data.BindingBase.FallbackValue" /> 
        /// if it is available, or else will use the default value.A return value of <see cref="T:System.Windows.Data.Binding" />.
        /// <see cref="F:System.Windows.Data.Binding.DoNothing" /> indicates that the binding does not transfer the value or use the 
        /// <see cref="P:System.Windows.Data.BindingBase.FallbackValue" /> or the default value.</returns>
        /// <param name="values">The array of values that the source bindings in the 
        /// <see cref="T:System.Windows.Data.MultiBinding" /> produces. 
        /// The value <see cref="F:System.Windows.DependencyProperty.UnsetValue" /> indicates that the source binding has no value
        ///  to provide for conversion.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 4 || !(values[0] is double) || !(values[1] is double)
                || !(values[2] is int) || !(values[3] is double) || !(parameter is double)) // Error in parameters
                return null;
            double canvasWidth = (double)values[0];
            double canvasHeight = (double)values[1];
            int ordinalNumber = (int)values[2];
            double ellipseWidth = (double)values[3];
            double angle = (double)parameter;
            double radius = Math.Min(canvasWidth, canvasHeight) / 2; // Determine optimal size and then divide by 2
            double xCord = radius + radius * Math.Cos(Math.PI / 180 * angle * ordinalNumber - Math.PI / 2) - ellipseWidth / 2;
            return xCord;
        }

        /// <summary>
        /// Converts a binding target value to the source binding values.</summary>
        /// <returns>An array of values that have been converted from the target value back to the source values.</returns>
        /// <param name="value">The value that the binding target produces.</param>
        /// <param name="targetTypes">The array of types to convert to. The array length indicates the number and types of values that are suggested for the method to return.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    #region Point To Canvas Top

    /// <summary>
    /// Multi converter used to convert a busy indicator's point to Canvas.Top value, meaning the point's Y coordinate.
    /// </summary>
    public class PointToCanvasTopConverter : IMultiValueConverter
    {
        /// <summary>
        /// Converts source values to a value for the binding target. The data binding engine calls this method when it propagates the values from source bindings to the binding target.</summary>
        /// <returns>A converted value.If the method returns null, the valid null value is used.A return value of <see cref="T:System.Windows.DependencyProperty" />.<see cref="F:System.Windows.DependencyProperty.UnsetValue" /> indicates that the converter did not produce a value, and that the binding will use the <see cref="P:System.Windows.Data.BindingBase.FallbackValue" /> if it is available, or else will use the default value.A return value of <see cref="T:System.Windows.Data.Binding" />.<see cref="F:System.Windows.Data.Binding.DoNothing" /> indicates that the binding does not transfer the value or use the <see cref="P:System.Windows.Data.BindingBase.FallbackValue" /> or the default value.</returns>
        /// <param name="values">The array of values that the source bindings in the <see cref="T:System.Windows.Data.MultiBinding" /> produces. The value <see cref="F:System.Windows.DependencyProperty.UnsetValue" /> indicates that the source binding has no value to provide for conversion.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 4 || !(values[0] is double) || !(values[1] is double)
                || !(values[2] is int) || !(values[3] is double) || !(parameter is double)) // Error in parameters
                return null;
            double canvasWidth = (double)values[0];
            double canvasHeight = (double)values[1];
            int ordinalNumber = (int)values[2];
            double ellipseHeight = (double)values[3];
            double angle = (double)parameter;
            double radius = Math.Min(canvasWidth, canvasHeight) / 2; // Determine optimal size and then divide by 2
            double yCord = radius + radius * Math.Sin(Math.PI / 180 * angle * ordinalNumber - Math.PI / 2) - ellipseHeight / 2;
            return yCord;
        }

        /// <summary>
        /// Converts a binding target value to the source binding values.</summary>
        /// <returns>An array of values that have been converted from the target value back to the source values.</returns>
        /// <param name="value">The value that the binding target produces.</param>
        /// <param name="targetTypes">The array of types to convert to. The array length indicates the number and types of values that are suggested for the method to return.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    #region Canavs Size To Radius

    /// <summary>
    /// Multi converter used to convert canvas' size to radius by
    /// </summary>
    public class CanvasSizeToRadiusConverter : IMultiValueConverter
    {
        /// <summary>
        /// Converts source values to a value for the binding target. The data binding engine calls this method when it propagates the values from source bindings to the binding target.</summary>
        /// <returns>A converted value.If the method returns null, the valid null value is used.A return value of <see cref="T:System.Windows.DependencyProperty" />.<see cref="F:System.Windows.DependencyProperty.UnsetValue" /> indicates that the converter did not produce a value, and that the binding will use the <see cref="P:System.Windows.Data.BindingBase.FallbackValue" /> if it is available, or else will use the default value.A return value of <see cref="T:System.Windows.Data.Binding" />.<see cref="F:System.Windows.Data.Binding.DoNothing" /> indicates that the binding does not transfer the value or use the <see cref="P:System.Windows.Data.BindingBase.FallbackValue" /> or the default value.</returns>
        /// <param name="values">The array of values that the source bindings in the <see cref="T:System.Windows.Data.MultiBinding" /> produces. The value <see cref="F:System.Windows.DependencyProperty.UnsetValue" /> indicates that the source binding has no value to provide for conversion.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2 || !(values[0] is double) || !(values[1] is double)) // Error in parameters
                return null;
            double canvasWidth = (double)values[0];
            double canvasHeight = (double)values[1];
            return Math.Min(canvasWidth, canvasHeight) * 10 / 100; // Determine optimal size and take a 10% of it
        }

        /// <summary>
        /// Converts a binding target value to the source binding values.</summary>
        /// <returns>An array of values that have been converted from the target value back to the source values.</returns>
        /// <param name="value">The value that the binding target produces.</param>
        /// <param name="targetTypes">The array of types to convert to. The array length indicates the number and types of values that are suggested for the method to return.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    #region Point Index To Opacity

    /// <summary>
    /// Converter used convert a point's index to its' matching opacity in the circle.
    /// </summary>
    public class PointIndexToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int) || !(parameter is int))
                return 1.0; // default value
            int index = (int)value;
            int totalPoints = (int)parameter;
            double opacityValue = (double)index / totalPoints;
            return opacityValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    #region Bool To Acceleration Ratio

    /// <summary>
    /// Converter used to convert a boolean value, indicating if acceleration should be applied on the busy indicator),
    /// to it's acceleration ratio.
    /// </summary>
    public class BoolToAccelerationRatioConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isAccelerated = (bool)value;
            return isAccelerated ? 0.4 : 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    #region Bool To Deceleration Ratio

    /// <summary>
    /// Converter used to convert a boolean value, indicating if acceleration should be applied on the busy indicator),
    /// to it's acceleration ratio.
    /// </summary>
    public class BoolToDecelerationRatioConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isDecelerated = (bool)value;
            return isDecelerated ? 0.6 : 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    #endregion
}
