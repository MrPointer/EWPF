using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using EWPF.MVVM.Services;
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
        /// Displays the requested dialog on top of the current <see cref="Window"/>. <br />
        /// This method searches through all of the <see cref="Type"/>s in the executing <see cref="Assembly"/>, 
        /// looking for a type that has the <see cref="DialogAttribute"/> registered to it, 
        /// and if it does, it compares the given <paramref name="i_DialogName"/> to its registered name. <br />
        /// The search could be optimized to search just in a specific namespace by passing the namespace name 
        /// as an argument.
        /// </summary>
        /// <param name="i_DialogName">Name of the dialog to search, registered in its <see cref="DialogAttribute"/>.</param>
        /// <param name="i_Namespace">Name of the specific namespace to search in. 
        /// Default is null, meaning the whole <see cref="Assembly"/> will be searched.</param>
        /// <returns>Dialog result of the opened dialog.</returns>
        /// <exception cref="ArgumentException">If the string arguments are empty or contain only whitespaces, 
        /// or if the requested dialog isn't found.</exception>
        /// <exception cref="InvalidCastException">If the requested dialog is found but is not 
        /// of a <see cref="Window"/> type.</exception>
        public static bool? ShowDialog(string i_DialogName, string i_Namespace = null)
        {
            // First, check arguments validity
            if (i_DialogName == null)
                throw new ArgumentNullException(nameof(i_DialogName), @"Dialog name can't be null");
            if (string.IsNullOrWhiteSpace(i_DialogName))
            {
                throw new ArgumentException(@"Dialog name can't be empty or contain only whitespaces",
                    nameof(i_DialogName));
            }
            if (i_Namespace != null && string.IsNullOrWhiteSpace(i_Namespace))
            {
                throw new ArgumentException(@"Namespace can't be empty or contain only whitespaces",
                    nameof(i_Namespace));
            }

            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            Type requestedDialogType = null;
            foreach (var assembly in allAssemblies)
            {
                requestedDialogType = assembly.GetTypes()
                    .Where(i_Type => i_Namespace == null || i_Type.IsClass && i_Type.Namespace == i_Namespace)
                    .FirstOrDefault(i_Type =>
                    {
                        var customAttributes = i_Type.GetCustomAttributes(typeof(DialogAttribute), false);
                        if (customAttributes.Length == 0)
                            return false;
                        var currentDialogAttribute = customAttributes[0] as DialogAttribute;
                        return currentDialogAttribute?.Name == i_DialogName;
                    });
                if (requestedDialogType != null) // Has been found in the current assembly
                    break;
            }

            if (requestedDialogType == null)
            {
                throw new ArgumentException(@"Given dialog name doesn't exist in this assembly, check spelling",
                    nameof(i_DialogName));
            }
            var dialogInstance = Activator.CreateInstance(requestedDialogType) as Window;
            if (dialogInstance == null)
                throw new InvalidCastException("Couldn't cast the created dialog instance to a Window object");
            return dialogInstance.ShowDialog();
        }

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