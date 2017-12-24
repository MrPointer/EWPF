using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using EWPF.Dialogs;
using EWPF.MVVM.Services;
using EWPF.MVVM.ViewModel;
using KISCore;
using KISCore.Execution;
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
        /// This method searches through all of the <see cref="Type"/>s in all of the <see cref="Assembly"/>s 
        /// under the current <see cref="AppDomain"/>,
        /// looking for a type that has the <see cref="DialogAttribute"/> registered to it. <br />
        /// If it does fine one, it then compares the given <paramref name="i_DialogName"/> 
        /// to the name registered within its <see cref="DialogAttribute"/>. <br />
        /// The search could be optimized to search just in a specific namespace by passing its name, 
        /// and even further optimized to search in a specific assembly by passing its name.
        /// </summary>
        /// <param name="i_DialogName">Name of the dialog to search, registered in its <see cref="DialogAttribute"/>.</param>
        /// <param name="i_Owner"></param>
        /// <param name="i_DataContext">Reference to an object that will be stored as 
        ///     the dialog's <see cref="FrameworkElement.DataContext"/>.</param>
        /// <param name="i_Namespace">Name of the specific namespace to search in. 
        ///     Default is null, meaning the whole <see cref="Assembly"/> will be searched.</param>
        /// <param name="i_AssemblyName">Name of the specific assembly to search in.</param>
        /// <returns>Dialog result of the opened dialog.</returns>
        /// <exception cref="ArgumentException">If the string arguments are empty or contain only whitespaces, 
        /// or if the requested dialog isn't found.</exception>
        /// <exception cref="InvalidCastException">If the requested dialog is found but is not 
        /// of a <see cref="Window"/> type.</exception>
        public static bool? ShowDialog(string i_DialogName, Window i_Owner = null,
            object i_DataContext = null, string i_Namespace = null,
            string i_AssemblyName = null)
        {
            // Reserve optimization flags for later use
            bool useNamespaceOptimization = false, useAssemblyOptimization = false;
            // First, check arguments validity
            if (i_DialogName == null)
                throw new ArgumentNullException("i_DialogName", @"Dialog name can't be null");
            if (string.IsNullOrWhiteSpace(i_DialogName))
            {
                throw new ArgumentException(
                    @"Dialog name can't be empty or contain only whitespaces",
                    "i_DialogName");
            }
            if (i_Namespace != null)
            {
                if (string.IsNullOrWhiteSpace(i_Namespace))
                    throw new ArgumentException(
                        @"Namespace can't be empty or contain only whitespaces",
                        "i_Namespace");
                useNamespaceOptimization = true;
            }
            if (i_AssemblyName != null)
            {
                if (string.IsNullOrWhiteSpace(i_AssemblyName))
                    throw new ArgumentException(
                        @"Assembly name can't be empty or contain only whitespaces",
                        "i_AssemblyName");
                useAssemblyOptimization = true;
            }

            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(i_Assembly =>
                    !useAssemblyOptimization || i_Assembly.FullName == i_AssemblyName);
            Type requestedDialogType = null;
            foreach (var assembly in allAssemblies)
            {
                requestedDialogType = assembly.GetTypes()
                    .Where(i_Type =>
                        !useNamespaceOptimization ||
                        i_Type.IsClass && i_Type.Namespace == i_Namespace)
                    .FirstOrDefault(i_Type =>
                    {
                        var customAttributes =
                            i_Type.GetCustomAttributes(typeof(DialogAttribute), false);
                        if (customAttributes.Length == 0)
                            return false;
                        var currentDialogAttribute = customAttributes[0] as DialogAttribute;
                        return currentDialogAttribute != null &&
                               currentDialogAttribute.Name == i_DialogName;
                    });
                if (requestedDialogType != null) // Has been found in the current assembly
                    break;
            }

            if (requestedDialogType == null)
            {
                throw new ArgumentException(
                    @"Given dialog name doesn't exist in the featured assemblies, "
                    + @"check spelling",
                    "i_DialogName");
            }

            // Create an instance of the dialog type, 
            // assuming it has to call AssignServices() in one of its ctors
            Window dialogInstance;
            if (i_DataContext != null)
                dialogInstance =
                    Activator.CreateInstance(requestedDialogType, i_DataContext) as Window;
            else
                dialogInstance = Activator.CreateInstance(requestedDialogType) as Window;
            if (dialogInstance == null)
                throw new InvalidCastException(
                    "Couldn't cast the created dialog instance to a Window object");

            if (dialogInstance.DataContext == null && i_DataContext != null)
                dialogInstance.DataContext = i_DataContext;
            if (i_Owner != null)
                dialogInstance.Owner = i_Owner;
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
        public static FileInfo ShowOpenFileDialog(string i_Extensions,
            string i_DefaultExtension,
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
        public static FileInfo ShowSaveFileDialog(string i_ExtensionsFilter,
            string i_DefaultExtension,
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

        #region Indefinite Progress Dialog

        /// <summary>
        /// Displays an indefinite progress dialog using the data in the given view model. <br />
        /// The progress bound in the ViewModel is then executed in a separate task 
        /// when the dialog is 'Loaded', with the ability to cancel it either programatically 
        /// or by closing the dialog.
        /// </summary>
        /// <param name="i_IndefiniteProgressDialogViewModel">Dialog's ViewModel.</param>
        /// <param name="i_Owner"></param>
        /// <typeparam name="TProgressResult">Type of the result 
        /// returned by the progress.</typeparam>
        /// <returns>Dialog result indicating whether the progress 
        /// has completed successfully or not.</returns>
        public static bool? ShowIndefiniteProgressDialog<TProgressResult>(
            IndefiniteProgressDialogViewModel<TProgressResult>
                i_IndefiniteProgressDialogViewModel, Window i_Owner = null)
        {
            var progressDialog =
                new IndefiniteProgressDialog(i_IndefiniteProgressDialogViewModel)
                {
                    Owner = i_Owner
                };
            return progressDialog.ShowDialog();
        }

        /// <summary>
        /// Displays an indefinite progress dialog using the given data. <br />
        /// The progress consists of the given action and is executed by the given executor 
        /// when the dialog is 'Loaded', with the ability to cancel it either programatically 
        /// or by closing the dialog.
        /// </summary>
        /// <param name="i_CancellableTaskExecutor">
        ///     Executor with cancellation capabilities.</param>
        /// <param name="i_CancellationTokenSource">Cancellation token's source.</param>
        /// <param name="i_ProgressAction">Action to execute as progress.</param>
        /// <param name="i_CompletionCallback">Callback to call 
        ///     when progress is complete.</param>
        /// <param name="i_CancellationCallback">Callback to call 
        ///     when progress is canceled.</param>
        /// <param name="i_DialogTitle">Dialog's title.</param>
        /// <param name="i_ProgressDescription">Text displayed in the dialog 
        ///     describing the progress.</param>
        /// <param name="i_IsRtlDisplay">Boolean value indicating whether 
        ///     dialog should be displayed Right-To-Left.</param>
        /// <param name="i_Owner"></param>
        /// <returns>Dialog result indicating whether the progress 
        /// has completed successfully or not.</returns>
        public static bool? ShowIndefiniteProgressDialog(
            ICancellableTaskExecutor<CancellationToken> i_CancellableTaskExecutor,
            CancellationTokenSource i_CancellationTokenSource,
            Action<CancellationToken> i_ProgressAction, Action i_CompletionCallback = null,
            Action i_CancellationCallback = null, string i_DialogTitle = null,
            string i_ProgressDescription = null, bool i_IsRtlDisplay = false,
            Window i_Owner = null)
        {
            var progressDialogVM = new IndefiniteProgressDialogViewModel<NullObject>(
                i_CancellableTaskExecutor, i_ProgressAction, i_CancellationTokenSource,
                i_CompletionCallback, i_CancellationCallback)
            {
                DialogTitle = i_DialogTitle,
                ProgressText = i_ProgressDescription,
                DialogFlowDirection = i_IsRtlDisplay
                                          ? FlowDirection.RightToLeft
                                          : FlowDirection.LeftToRight
            };
            var progressDialog =
                new IndefiniteProgressDialog(progressDialogVM) { Owner = i_Owner };
            return progressDialog.ShowDialog();
        }

        /// <summary>
        /// Displays an indefinite progress dialog using the given data. <br />
        /// The progress consists of the given function and is executed by the given executor 
        /// when the dialog is 'Loaded', with the ability to cancel it either programatically 
        /// or by closing the dialog.
        /// </summary>
        /// <param name="i_CancellableTaskExecutor">
        ///     Executor with cancellation capabilities.</param>
        /// <param name="i_CancellationTokenSource">Cancellation token's source.</param>
        /// <param name="i_ProgressFunction">Function to execute as progress.</param>
        /// <param name="i_CompletionCallback">Callback to call 
        ///     when progress is complete.</param>
        /// <param name="i_CancellationCallback">Callback to call 
        ///     when progress is canceled.</param>
        /// <param name="i_DialogTitle">Dialog's title.</param>
        /// <param name="i_ProgressDescription">Text displayed in the dialog 
        ///     describing the progress.</param>
        /// <param name="i_IsRtlDisplay">Boolean value indicating whether 
        ///     dialog should be displayed Right-To-Left.</param>
        /// <param name="i_Owner"></param>
        /// /// <typeparam name="TProgressResult">Type of the result 
        /// returned by the progress.</typeparam>
        /// <returns>Dialog result indicating whether the progress 
        /// has completed successfully or not.</returns>
        public static bool? ShowIndefiniteProgressDialog<TProgressResult>(
            ICancellableTaskExecutor<CancellationToken> i_CancellableTaskExecutor,
            CancellationTokenSource i_CancellationTokenSource,
            Func<CancellationToken, TProgressResult> i_ProgressFunction,
            Action<TProgressResult> i_CompletionCallback = null,
            Action i_CancellationCallback = null, string i_DialogTitle = null,
            string i_ProgressDescription = null, bool i_IsRtlDisplay = false,
            Window i_Owner = null)
        {
            var progressDialogVM = new IndefiniteProgressDialogViewModel<TProgressResult>(
                i_CancellableTaskExecutor, i_ProgressFunction, i_CancellationTokenSource,
                i_CompletionCallback, i_CancellationCallback)
            {
                DialogTitle = i_DialogTitle,
                ProgressText = i_ProgressDescription,
                DialogFlowDirection = i_IsRtlDisplay
                                          ? FlowDirection.RightToLeft
                                          : FlowDirection.LeftToRight
            };

            var progressDialog =
                new IndefiniteProgressDialog(progressDialogVM) { Owner = i_Owner };
            return progressDialog.ShowDialog();
        }

        #endregion

        #endregion

        #region Properties



        #endregion
    }
}