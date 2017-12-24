using System;
using System.Threading;
using EWPF.MVVM.ViewModel;
using KISCore.Execution;

namespace EWPF.MVVM.Services
{
    /// <summary>
    /// An interface designed to display progress dialogs on top of the view. <br />
    /// This is a service-like interface implementing the DI pattern.
    /// </summary>
    public interface IProgressDialogService
    {
        #region Fields

        #endregion

        #region Methods

        /// <summary>
        /// Displays an indefinite progress dialog using the data in the given view model. <br />
        /// The progress bound in the ViewModel is then executed in a separate task 
        /// when the dialog is 'Loaded', with the ability to cancel it either programatically 
        /// or by closing the dialog.
        /// </summary>
        /// <param name="i_IndefiniteProgressDialogViewModel">Dialog's ViewModel.</param>
        /// <typeparam name="TProgressResult">Type of the result 
        /// returned by the progress.</typeparam>
        /// <returns>Dialog result indicating whether the progress 
        /// has completed successfully or not.</returns>
        bool? ShowIndefiniteProgressDialog<TProgressResult>(
            IndefiniteProgressDialogViewModel<TProgressResult>
                i_IndefiniteProgressDialogViewModel);

        /// <summary>
        /// Displays an indefinite progress dialog using the given data. <br />
        /// The progress consists of the given action and is executed by the given executor 
        /// when the dialog is 'Loaded', with the ability to cancel it either programatically 
        /// or by closing the dialog.
        /// </summary>
        /// <param name="i_CancellableTaskExecutor">
        /// Executor with cancellation capabilities.</param>
        /// <param name="i_CancellationTokenSource">Cancellation token's source.</param>
        /// <param name="i_ProgressAction">Action to execute as progress.</param>
        /// <param name="i_CompletionCallback">Callback to call 
        /// when progress is complete.</param>
        /// <param name="i_CancellationCallback">Callback to call 
        /// when progress is canceled.</param>
        /// <param name="i_DialogTitle">Dialog's title.</param>
        /// <param name="i_ProgressDescription">Text displayed in the dialog 
        /// describing the progress.</param>
        /// <param name="i_IsRtlDisplay">Boolean value indicating whether 
        /// dialog should be displayed Right-To-Left.</param>
        /// <returns>Dialog result indicating whether the progress 
        /// has completed successfully or not.</returns>
        bool? ShowIndefiniteProgressDialog(
            ICancellableTaskExecutor<CancellationToken> i_CancellableTaskExecutor,
            CancellationTokenSource i_CancellationTokenSource,
            Action<CancellationToken> i_ProgressAction, Action i_CompletionCallback = null,
            Action i_CancellationCallback = null, string i_DialogTitle = null,
            string i_ProgressDescription = null, bool i_IsRtlDisplay = false);

        /// <summary>
        /// Displays an indefinite progress dialog using the given data. <br />
        /// The progress consists of the given function and is executed by the given executor 
        /// when the dialog is 'Loaded', with the ability to cancel it either programatically 
        /// or by closing the dialog.
        /// </summary>
        /// <param name="i_CancellableTaskExecutor">
        /// Executor with cancellation capabilities.</param>
        /// <param name="i_CancellationTokenSource">Cancellation token's source.</param>
        /// <param name="i_ProgressFunction">Function to execute as progress.</param>
        /// <param name="i_CompletionCallback">Callback to call 
        /// when progress is complete.</param>
        /// <param name="i_CancellationCallback">Callback to call 
        /// when progress is canceled.</param>
        /// <param name="i_DialogTitle">Dialog's title.</param>
        /// <param name="i_ProgressDescription">Text displayed in the dialog 
        /// describing the progress.</param>
        /// <param name="i_IsRtlDisplay">Boolean value indicating whether 
        /// dialog should be displayed Right-To-Left.</param>
        /// /// <typeparam name="TProgressResult">Type of the result 
        /// returned by the progress.</typeparam>
        /// <returns>Dialog result indicating whether the progress 
        /// has completed successfully or not.</returns>
        bool? ShowIndefiniteProgressDialog<TProgressResult>(
            ICancellableTaskExecutor<CancellationToken> i_CancellableTaskExecutor,
            CancellationTokenSource i_CancellationTokenSource,
            Func<CancellationToken, TProgressResult> i_ProgressFunction,
            Action<TProgressResult> i_CompletionCallback = null,
            Action i_CancellationCallback = null, string i_DialogTitle = null,
            string i_ProgressDescription = null, bool i_IsRtlDisplay = false);

        #endregion

        #region Properties

        #endregion
    }
}