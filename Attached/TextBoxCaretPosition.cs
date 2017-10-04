using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace EWPF.Attached
{
    /// <summary>
    /// A static class defining attached properties to handle 
    /// positioning of carets at the end of <see cref="TextBox"/>es.
    /// </summary>
    public static class TextBoxCaretPosition
    {
        #region Events

        #endregion

        #region Fields



        #endregion

        #region Constructors

        #endregion

        #region Methods

        /// <summary>
        /// Handles a change to the <see cref="IsPositionedAtTheEndProperty"/> 
        /// by setting the caret position to the end of the calling <see cref="TextBox"/> 
        /// if required. <br />        
        /// </summary>
        /// <remarks>
        /// A <see cref="TextBox.CaretIndex"/> can change only when 
        /// the calling <see cref="TextBox"/> has focus, so a handler must be registered
        /// to the <see cref="UIElement.GotFocusEvent"/> if the <see cref="TextBox"/> 
        /// doesn't have focus yet.
        /// </remarks>
        /// <param name="i_DependencyObject">Calling <see cref="TextBox"/>.</param>
        /// <param name="i_E">Indicates whether the caret should be placed 
        /// at the end of the <see cref="TextBox"/> or not.</param>
        private static void HandleIsPositionedAtTheEndChanged(DependencyObject i_DependencyObject,
            DependencyPropertyChangedEventArgs i_E)
        {
            if (i_DependencyObject == null)
                throw new ArgumentNullException("i_DependencyObject",
                    @"Dependency object can't be null");
            if (!(i_DependencyObject is TextBox))
                return;

            Application.Current.Dispatcher.Invoke(async () =>
            {
                await Task.Yield();

                var sourceTextBox = (TextBox)i_DependencyObject;
                bool caretShouldBePlacedAtTheEnd = (bool)i_E.NewValue;

                if (caretShouldBePlacedAtTheEnd)
                    PositionCaretAtTheEndOTextBox(sourceTextBox);
                else
                    ReleaseCaretFromTheEndOfTextBox(sourceTextBox);
            }, DispatcherPriority.Input);
        }

        /// <summary>
        /// Positions caret at the end of the given <see cref="TextBox"/>. <br />
        /// If the <see cref="TextBox"/> has focus already it is done immediately, 
        /// otherwise it's done when it gains focus by registering to its' <see cref="UIElement.GotFocusEvent"/>.
        /// </summary>
        /// <param name="i_TargetTextBox">Target TextBox to position caret for.</param>
        private static void PositionCaretAtTheEndOTextBox(TextBox i_TargetTextBox)
        {
            if (i_TargetTextBox.IsFocused)
                i_TargetTextBox.CaretIndex = i_TargetTextBox.Text.Length;
            i_TargetTextBox.GotFocus += OnTextBoxGotFocus;
        }

        /// <summary>
        /// Releases the caret's position from the end of the given <see cref="TextBox"/> 
        /// by un-registering from its' <see cref="UIElement.GotFocusEvent"/>.
        /// </summary>
        /// <param name="i_TargetTextBox">Target TextBox to release caret from.</param>
        private static void ReleaseCaretFromTheEndOfTextBox(TextBox i_TargetTextBox)
        {
            i_TargetTextBox.GotFocus -= OnTextBoxGotFocus;
        }

        /// <summary>
        /// Handles the 'GotFocus' event of a <see cref="TextBox"/> by placing its' 
        /// <see cref="TextBox.CaretIndex"/> to the last possible index.
        /// </summary>
        /// <param name="i_Sender">Irrelevant.</param>
        /// <param name="i_E">Event args containing the event's source <see cref="TextBox"/>.</param>
        private static void OnTextBoxGotFocus(object i_Sender, RoutedEventArgs i_E)
        {
            var sourceTextBox = (TextBox)i_E.Source;
            sourceTextBox.CaretIndex = sourceTextBox.Text.Length;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a boolean value indicating if a <see cref="TextBox"/>'s caret 
        /// should be positioned at the end of it.
        /// </summary>
        public static readonly DependencyProperty IsPositionedAtTheEndProperty =
            DependencyProperty.RegisterAttached(
                "IsPositionedAtTheEnd",
                typeof(bool),
                typeof(TextBoxCaretPosition),
                new FrameworkPropertyMetadata(default(bool), HandleIsPositionedAtTheEndChanged));

        /// <summary>
        /// Sets a boolean value indicating if a <see cref="TextBox"/>'s caret 
        /// should be positioned at the end of it.
        /// </summary>
        /// <param name="i_Element">Source <see cref="TextBox"/>.</param>
        /// <param name="i_Value">True if caret should be positioned at the end, false otherwise.</param>
        public static void SetIsPositionedAtTheEnd(DependencyObject i_Element, bool i_Value)
        {
            if (i_Element == null)
                throw new ArgumentNullException("i_Element", @"Source element can't be null");
            i_Element.SetValue(IsPositionedAtTheEndProperty, i_Value);
        }

        /// <summary>
        /// Gets a boolean value indicating if a <see cref="TextBox"/>'s caret 
        /// should be positioned at the end of it.
        /// </summary>
        /// <param name="i_Element">Source <see cref="TextBox"/>.</param>
        /// <returns>True if caret should be positioned at the end, false otherwise.</returns>
        public static bool GetIsPositionedAtTheEnd(DependencyObject i_Element)
        {
            if (i_Element == null)
                throw new ArgumentNullException("i_Element", @"Source element can't be null");
            if (IsPositionedAtTheEndProperty == null)
                return false;
            return (bool)i_Element.GetValue(IsPositionedAtTheEndProperty);
        }

        #endregion
    }
}