using System;
using System.Windows;

namespace EWPF.Attached
{
    public static class FocusExtension
    {
        #region Events



        #endregion

        #region Fields



        #endregion

        #region Constructors



        #endregion

        #region Methods

        private static void IsFocusedChanged(DependencyObject i_DependencyObject, DependencyPropertyChangedEventArgs i_E)
        {
            var frameworkElement = (FrameworkElement)i_DependencyObject;

            if (i_E.OldValue == null) // Case the element hasn't been assigned any focus extension yet
            {
                frameworkElement.GotFocus += FrameworkElement_GotFocus;
                frameworkElement.LostFocus += FrameworkElement_LostFocus;
            }

            if (!frameworkElement.IsVisible) // Case element isn't visible
            {
                frameworkElement.IsVisibleChanged += FrameworkElement_IsVisibleChanged;
            }

            if ((bool)i_E.NewValue) // Case focus should be gained
            {
                frameworkElement.Focus();
            }
        }

        private static void FrameworkElement_IsVisibleChanged(object i_Sender, DependencyPropertyChangedEventArgs i_E)
        {
            var frameworkElement = (FrameworkElement)i_Sender;

            // Return if element isn't visible or if it doesn't has focus
            if (!frameworkElement.IsVisible || !(bool)((FrameworkElement)i_Sender).GetValue(IsFocusedProperty))
                return;

            // Unregister from event
            frameworkElement.IsVisibleChanged -= FrameworkElement_IsVisibleChanged;

            // Gain focus
            frameworkElement.Focus();
        }

        private static void FrameworkElement_GotFocus(object i_Sender, RoutedEventArgs i_E)
        {
            ((FrameworkElement)i_Sender).SetValue(IsFocusedProperty, true);
        }

        private static void FrameworkElement_LostFocus(object i_Sender, RoutedEventArgs i_E)
        {
            ((FrameworkElement)i_Sender).SetValue(IsFocusedProperty, false);
        }

        #endregion

        #region Properties

        public static readonly DependencyProperty IsFocusedProperty =
        DependencyProperty.RegisterAttached("IsFocused", typeof(bool?), typeof(FocusExtension),
                                            new FrameworkPropertyMetadata(IsFocusedChanged));

        public static bool? GetIsFocused(DependencyObject i_Element)
        {
            if (i_Element == null)
            {
                throw new ArgumentNullException("i_Element");
            }

            return (bool?)i_Element.GetValue(IsFocusedProperty);
        }

        public static void SetIsFocused(DependencyObject i_Element, bool? i_Value)
        {
            if (i_Element == null)
            {
                throw new ArgumentNullException("i_Element");
            }

            i_Element.SetValue(IsFocusedProperty, i_Value);
        }

        #endregion
    }
}