using System;
using System.Windows;
using EWPF.MVVM.ViewModel;
using EWPF.Styles;

namespace EWPF.Utility
{
    /// <summary>
    /// A static utility class providing utility methods to display various message boxes, themed or native.
    /// </summary>
    public static class MessageBoxUtility
    {
        #region Events

        #endregion

        #region Fields



        #endregion

        #region Constructors

        #endregion

        #region Methods

        /// <summary>
        /// Shows a EWPF-themed message box based on the given parameters.
        /// <para/>
        /// It calls one of the utility methods provided by this class to show a specific dialog based on the button parameter.
        /// </summary>
        /// <param name="i_Caption">Message box's caption/title.</param>
        /// <param name="i_Content">Message box's textual content.</param>
        /// <param name="i_Button">Message box's button/s.</param>
        /// <param name="i_Image">Message box's image.</param>
        /// <param name="i_Options">Message box's extra options.</param>
        /// <param name="i_OwnerWindow"></param>
        /// <returns>Message box's result.</returns>
        public static MessageBoxResult ShowMessageBox(string i_Caption, string i_Content, 
            MessageBoxButton i_Button, MessageBoxImage i_Image, MessageBoxOptions i_Options, Window i_OwnerWindow = null)
        {
            switch (i_Button)
            {
                case MessageBoxButton.OK:
                    return ShowOkDialog(i_Caption, i_Content, i_Image, i_Options, i_OwnerWindow);

                case MessageBoxButton.OKCancel:
                    break;

                case MessageBoxButton.YesNoCancel:
                    break;

                case MessageBoxButton.YesNo:
                    break;

                default: // Should never happen
                    throw new ArgumentOutOfRangeException("i_Button", i_Button, null);
            }
            return MessageBoxResult.OK; // ToDo: Delete when all cases of the switch will return a value
        }

        /// <summary>
        /// Shows a message box dialog with a single 'OK' button.
        /// Use it to show informative message boxes.
        /// </summary>
        /// <param name="i_Caption">Message box's caption/title.</param>
        /// <param name="i_Content">Message box's textual content.</param>
        /// <param name="i_Image">Message box's image.</param>
        /// <param name="i_Options">Message box's extra options.</param>
        /// <param name="i_OwnerWindow"></param>
        /// <returns>Message box's result.</returns>
        public static MessageBoxResult ShowOkDialog(string i_Caption, string i_Content,
            MessageBoxImage i_Image, MessageBoxOptions i_Options, Window i_OwnerWindow = null)
        {
            var emsgBox = new EMessageBox();
            var emsgBoxVM = emsgBox.DataContext as EMsgBoxViewModel;
            if (emsgBoxVM == null)
                throw new InvalidCastException("Couldn't cast data context to EMsgBox view model.");
            emsgBoxVM.Title = i_Caption;
            emsgBoxVM.Content = i_Content;
            emsgBoxVM.MBoxFlowDirection = i_Options == MessageBoxOptions.RtlReading
                ? FlowDirection.RightToLeft
                : FlowDirection.LeftToRight;
            // ToDo: Extract text from language module
            emsgBoxVM.PositiveText = "OK";
            emsgBox.Owner = i_OwnerWindow;
            emsgBox.ShowDialog();
            return MessageBoxResult.OK;
        }

        #endregion

        #region Properties



        #endregion
    }
}