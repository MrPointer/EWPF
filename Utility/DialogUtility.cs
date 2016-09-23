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
        /// Displays a Windows-native OpenFileDialog with the given filter extensions.
        /// <para/>
        /// It also can check for existance of a path supplied by the user, depending on the passed arguments.
        /// </summary>
        /// <param name="i_Extensions">List of extensions the dialog should look for.</param>
        /// <param name="i_DefaultExtension">Default extension displayed by the dialog.</param>
        /// <param name="i_CheckPathExistance">Indicates weather the dialog should display a warning 
        /// if the user specified a non-existing path.</param>
        /// <param name="i_OwnerWindow">Reference to the owner window which will display the dialog on top of it.</param>
        /// <returns>FileInfo object representing the selected file or null if operation has been canceled.</returns>
        public static FileInfo ShowOpenFileDialog(string i_Extensions, string i_DefaultExtension, bool i_CheckPathExistance,
            Window i_OwnerWindow)
        {
            var fileDialog = new OpenFileDialog
            {
                Filter = i_Extensions,
                DefaultExt = i_DefaultExtension,
                CheckPathExists = i_CheckPathExistance
            };
            var dialogResult = fileDialog.ShowDialog(i_OwnerWindow);
            return !dialogResult.Value ? null : new FileInfo(fileDialog.FileName);
        }

        #endregion

        #region Properties



        #endregion
    }
}