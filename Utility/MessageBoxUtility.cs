using System;
using System.Collections.Generic;
using System.Windows;
using EWPF.MVVM.ViewModel;
using EWPF.Styles;
using EWPFLang;
using EWPFLang.ELang;

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
        private static IDictionary<string, string> sm_CustomIcons;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialize static class to use native icons by default.
        /// </summary>
        static MessageBoxUtility()
        {
            UseNativeIcons = true;
        }

        #endregion

        #region Methods

        #region Icon-Related Methods

        /// <summary>
        /// Informs the message boxes to use custom icons based on the parameters.
        /// </summary>
        /// <param name="i_ErrorIcon">Path to an error icon.</param>
        /// <param name="i_WarningIcon">Path to a warning icon.</param>
        /// <param name="i_QuestionIcon">Path to a question icon.</param>
        /// <param name="i_InformationIcon">Path to an information icon.</param>
        public static void SetCustomIcons(string i_ErrorIcon, string i_WarningIcon, string i_QuestionIcon,
            string i_InformationIcon)
        {
            sm_CustomIcons = new Dictionary<string, string>(4)
            {
                {IconStrings.ERROR, i_ErrorIcon},
                {IconStrings.WARNING, i_WarningIcon},
                {IconStrings.QUESTION, i_QuestionIcon},
                {IconStrings.INFORMATION, i_InformationIcon}
            };
            UseNativeIcons = false;
        }

        /// <summary>
        /// Resolves the correct icon path to use based on the given message box image.
        /// It handles both native and custom icons.
        /// </summary>
        /// <param name="i_Image">Requested image to display as the icon.</param>
        /// <returns></returns>
        private static string ResolveIcon(MessageBoxImage i_Image)
        {
            if (UseNativeIcons) // Native icons should be used
            {
                switch (i_Image)
                {
                    case MessageBoxImage.None:
                        return string.Empty;

                    case MessageBoxImage.Hand:
                        return IconStrings.ERROR;

                    case MessageBoxImage.Question:
                        return IconStrings.QUESTION;

                    case MessageBoxImage.Exclamation:
                        return IconStrings.WARNING;

                    case MessageBoxImage.Asterisk:
                        return IconStrings.INFORMATION;

                    default:
                        throw new ArgumentOutOfRangeException("i_Image", i_Image, null);
                }
            }
            // Custom icons are used
            bool isValueRetrieved;
            string iconPath;
            switch (i_Image)
            {
                case MessageBoxImage.None:
                    return string.Empty;

                case MessageBoxImage.Hand:
                    isValueRetrieved = sm_CustomIcons.TryGetValue(IconStrings.ERROR, out iconPath);
                    if (isValueRetrieved)
                        return iconPath;
                    throw new KeyNotFoundException();

                case MessageBoxImage.Question:
                    isValueRetrieved = sm_CustomIcons.TryGetValue(IconStrings.QUESTION, out iconPath);
                    if (isValueRetrieved)
                        return iconPath;
                    throw new KeyNotFoundException();

                case MessageBoxImage.Exclamation:
                    isValueRetrieved = sm_CustomIcons.TryGetValue(IconStrings.WARNING, out iconPath);
                    if (isValueRetrieved)
                        return iconPath;
                    throw new KeyNotFoundException();

                case MessageBoxImage.Asterisk:
                    isValueRetrieved = sm_CustomIcons.TryGetValue(IconStrings.INFORMATION, out iconPath);
                    if (isValueRetrieved)
                        return iconPath;
                    throw new KeyNotFoundException();

                default:
                    throw new ArgumentOutOfRangeException("i_Image", i_Image, null);
            }
        }

        #endregion

        #region Show Methods

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
            emsgBoxVM.SetIcon(ResolveIcon(i_Image));

            var language = LanguageRepository.GetLanguage(sm_UsedLanguage);
            emsgBoxVM.PositiveText = language.GetWord(DictionaryCode.OK);

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
            emsgBoxVM.SetIcon(ResolveIcon(i_Image));

            var language = LanguageRepository.GetLanguage(sm_UsedLanguage);
            emsgBoxVM.PositiveText = language.GetWord(DictionaryCode.OK);
            emsgBoxVM.NegativeText = language.GetWord(DictionaryCode.Cancel);

            emsgBox.Owner = i_OwnerWindow;
            emsgBox.ShowDialog();
            var mboxResult = emsgBox.Result;
            switch (mboxResult)
            {
                case EDialogResult.Positive:
                    return MessageBoxResult.OK;
                case EDialogResult.Negative:
                    return MessageBoxResult.Cancel;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
            emsgBoxVM.SetIcon(ResolveIcon(i_Image));

            var language = LanguageRepository.GetLanguage(sm_UsedLanguage);
            emsgBoxVM.PositiveText = language.GetWord(DictionaryCode.Yes);
            emsgBoxVM.NegativeText = language.GetWord(DictionaryCode.No);

            emsgBox.Owner = i_OwnerWindow;
            emsgBox.ShowDialog();
            var mboxResult = emsgBox.Result;
            switch (mboxResult)
            {
                case EDialogResult.Positive:
                    return MessageBoxResult.Yes;
                case EDialogResult.Negative:
                    return MessageBoxResult.No;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
            emsgBoxVM.SetIcon(ResolveIcon(i_Image));

            var language = LanguageRepository.GetLanguage(sm_UsedLanguage);
            emsgBoxVM.PositiveText = language.GetWord(DictionaryCode.Yes);
            emsgBoxVM.NegativeText = language.GetWord(DictionaryCode.No);
            emsgBoxVM.NeutralText = language.GetWord(DictionaryCode.Cancel);

            emsgBox.Owner = i_OwnerWindow;
            emsgBox.ShowDialog();
            var mboxResult = emsgBox.Result;
            switch (mboxResult) // Convert custom result to an actual result
            {
                case EDialogResult.Positive:
                    return MessageBoxResult.Yes;
                case EDialogResult.Negative:
                    return MessageBoxResult.No;
                case EDialogResult.Neutral:
                    return MessageBoxResult.Cancel;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a boolean indicating weather Windows-native icons should be displayed in the message boxes.
        /// </summary>
        public static bool UseNativeIcons { get; set; }

        #endregion
    }
}