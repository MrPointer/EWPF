using System;
using System.Windows;
using EWPF.MVVM.ViewModel;
using EWPF.Styles;
using EWPFLang;

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

        private static LanguageCode sm_UsedLanguage;

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
        /// <param name="i_DisplayLanguage">Language to display the message box in.</param>
        /// <param name="i_OwnerWindow"></param>
        /// <returns>Message box's result.</returns>
        public static MessageBoxResult ShowMessageBox(string i_Caption, string i_Content,
            MessageBoxButton i_Button, MessageBoxImage i_Image, MessageBoxOptions i_Options,
            LanguageCode i_DisplayLanguage, Window i_OwnerWindow = null)
        {
            sm_UsedLanguage = i_DisplayLanguage;
            bool isRtlLanguage = i_DisplayLanguage == LanguageCode.Hebrew;
            switch (i_Button)
            {
                case MessageBoxButton.OK:
                    return ShowOkDialog(i_Caption, i_Content, i_Image, isRtlLanguage ?
                        i_Options | MessageBoxOptions.RtlReading : i_Options, i_OwnerWindow);

                case MessageBoxButton.OKCancel:
                    return ShowOkCancelDialog(i_Caption, i_Content, i_Image, isRtlLanguage ?
                        i_Options | MessageBoxOptions.RtlReading : i_Options, i_OwnerWindow);

                case MessageBoxButton.YesNoCancel:
                    return ShowYesNoCancelDialog(i_Caption, i_Content, i_Image, isRtlLanguage ?
                        i_Options | MessageBoxOptions.RtlReading : i_Options, i_OwnerWindow);

                case MessageBoxButton.YesNo:
                    return ShowYesNoDialog(i_Caption, i_Content, i_Image, isRtlLanguage ?
                        i_Options | MessageBoxOptions.RtlReading : i_Options, i_OwnerWindow);

                default: // Should never happen
                    throw new ArgumentOutOfRangeException("i_Button", i_Button, null);
            }
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

        /// <summary>
        /// Shows a message box dialog with two buttons: "Ok" and "Cancel".
        /// Use it to ask user's permission to proceed a task for example.
        /// </summary>
        /// <param name="i_Caption">Message box's caption/title.</param>
        /// <param name="i_Content">Message box's textual content.</param>
        /// <param name="i_Image">Message box's image.</param>
        /// <param name="i_Options">Message box's extra options.</param>
        /// <param name="i_OwnerWindow"></param>
        /// <returns>Message box's result.</returns>
        public static MessageBoxResult ShowOkCancelDialog(string i_Caption, string i_Content,
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
            emsgBoxVM.NegativeText = "Cancel";
            emsgBox.Owner = i_OwnerWindow;
            var mboxResult = emsgBox.ShowDialog();
            return mboxResult.HasValue && mboxResult.Value ? MessageBoxResult.Yes : MessageBoxResult.Cancel;
        }

        /// <summary>
        /// Shows a message box dialog with two buttons: "Yes" and "No".
        /// Use it to ask user a determinite question.
        /// </summary>
        /// <param name="i_Caption">Message box's caption/title.</param>
        /// <param name="i_Content">Message box's textual content.</param>
        /// <param name="i_Image">Message box's image.</param>
        /// <param name="i_Options">Message box's extra options.</param>
        /// <param name="i_OwnerWindow"></param>
        /// <returns>Message box's result.</returns>
        public static MessageBoxResult ShowYesNoDialog(string i_Caption, string i_Content,
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
            emsgBoxVM.PositiveText = "Yes";
            emsgBoxVM.NegativeText = "No";
            emsgBox.Owner = i_OwnerWindow;
            var mboxResult = emsgBox.ShowDialog();
            return mboxResult.HasValue && mboxResult.Value ? MessageBoxResult.Yes : MessageBoxResult.No;
        }

        /// <summary>
        /// Shows a message box dialog with three buttons: "Yes", "No" and "Cancel".
        /// Use it to ask user an indeterminite question.
        /// </summary>
        /// <param name="i_Caption">Message box's caption/title.</param>
        /// <param name="i_Content">Message box's textual content.</param>
        /// <param name="i_Image">Message box's image.</param>
        /// <param name="i_Options">Message box's extra options.</param>
        /// <param name="i_OwnerWindow"></param>
        /// <returns>Message box's result.</returns>
        public static MessageBoxResult ShowYesNoCancelDialog(string i_Caption, string i_Content,
            MessageBoxImage i_Image, MessageBoxOptions i_Options, Window i_OwnerWindow = null)
        {
            var emsgBox = new EMessageBox();
            var emsgBoxVM = emsgBox.DataContext as EMsgBoxViewModel;
            if (emsgBoxVM == null)
                throw new InvalidCastException("Couldn't cast data context to EMsgBox view model.");
            emsgBoxVM.Title = i_Caption;
            emsgBoxVM.Content = i_Content;
            emsgBoxVM.MBoxFlowDirection = (i_Options & MessageBoxOptions.RtlReading) != 0
                ? FlowDirection.RightToLeft
                : FlowDirection.LeftToRight;
            // ToDo: Extract text from language module
            var language = LanguageRepository.GetLanguage(sm_UsedLanguage);
            emsgBoxVM.PositiveText = language.GetWord(DictionaryCode.Yes);
            emsgBoxVM.NegativeText = language.GetWord(DictionaryCode.No);
            emsgBoxVM.NeutralText = language.GetWord(DictionaryCode.Cancel);
            emsgBox.Owner = i_OwnerWindow;
            var mboxResult = emsgBox.ShowDialog();
            if (!mboxResult.HasValue)
                return MessageBoxResult.Cancel;
            return mboxResult.Value ? MessageBoxResult.Yes : MessageBoxResult.No;
        }

        #endregion

        #region Properties



        #endregion
    }
}