using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace EWPF.Utility
{
    /// <summary>
    /// A static utility class providing utility methods to show various types of dialogs exposed by the IDialogService interface.
    /// </summary>
    public static class DialogUtility
    {
        #region Events

        #endregion

        #region Fields



        #endregion

        #region Constructors

        #endregion

        #region Methods

        /// <summary>
        /// Displays a Windows-native <see cref="OpenFileDialog"/> with the given filter extensions.
        /// <para/>
        /// It also can check for existence of a path supplied by the user, depending on the passed arguments.
        /// </summary>
        /// <param name="i_Extensions">List of extensions the dialog should look for.</param>
        /// <param name="i_DefaultExtension">Default extension displayed by the dialog.</param>
        /// <param name="i_InitialLocation">Indicates weather the dialog will be displayed at the center of it's owner window or not.</param>
        /// <param name="i_OwnerWindow">Reference to the owner window which will display the dialog on top of it.</param>
        /// <param name="i_IsPathChecked">Indicates weather the resolved path should be checked for validity, displaying warning if invalid.</param>
        /// <returns>FileInfo object representing the selected file or null if operation has been canceled.</returns>
        public static FileInfo ShowOpenFileDialog(string i_Extensions, string i_DefaultExtension,
            string i_InitialLocation, Window i_OwnerWindow, bool i_IsPathChecked = false)
        {
            var fileDialog = new OpenFileDialog
            {
                Filter = i_Extensions,
                DefaultExt = i_DefaultExtension,
                CheckPathExists = i_IsPathChecked
            };
            if (!string.IsNullOrEmpty(i_InitialLocation))
                fileDialog.InitialDirectory = i_InitialLocation;
            var dialogResult = fileDialog.ShowDialog(i_OwnerWindow);
            return !dialogResult.Value ? null : new FileInfo(fileDialog.FileName);
        }

        /// <summary>
        /// Displays a Windows-native <see cref="SaveFileDialog"/> with the given filter extensions.
        /// <para/>
        /// It also can check for existence of a path supplied by the user, depending on the passed arguments.
        /// </summary>
        /// <param name="i_ExtensionsFilter">List of extensions the dialog should filter.</param>
        /// <param name="i_DefaultExtension">Default extension displayed by the dialog.</param>
        /// <param name="i_InitialLocation">Indicates weather the dialog will be displayed at the center of it's owner window or not.</param>
        /// <param name="i_OwnerWindow">Reference to the owner window which will display the dialog on top of it.</param>
        /// <param name="i_IsPathChecked">Indicates weather the resolved path should be checked for validity, displaying warning if invalid.</param>
        /// <returns>FileInfo object representing the selected file or null if operation has been canceled.</returns>
        public static FileInfo ShowSaveFileDialog(string i_ExtensionsFilter, string i_DefaultExtension,
            string i_InitialLocation, Window i_OwnerWindow, bool i_IsPathChecked = false)
        {
            var saveDialog = new SaveFileDialog
            {
                Filter = i_ExtensionsFilter,
                DefaultExt = i_DefaultExtension,
                CheckPathExists = i_IsPathChecked
            };
            if (!string.IsNullOrEmpty(i_InitialLocation))
                saveDialog.InitialDirectory = i_InitialLocation;
            var dialogResult = saveDialog.ShowDialog(i_OwnerWindow);
            return !dialogResult.Value ? null : new FileInfo(saveDialog.FileName);
        }

        #endregion

        #region Properties



        #endregion
    }
}