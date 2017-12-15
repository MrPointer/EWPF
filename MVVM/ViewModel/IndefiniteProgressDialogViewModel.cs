using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EWPF.Controls;
using EWPF.Dialogs;
using EWPF.MVVM.Services;
using KISCore.Execution;

namespace EWPF.MVVM.ViewModel
{
    /// <summary>
    /// A class representing a generic view model bound to the 
    /// <see cref="Dialogs.IndefiniteProgressDialog"/> view.
    /// </summary>
    /// <typeparam name="TProgressResult">Type of the progressed action's result.</typeparam>
    public class IndefiniteProgressDialogViewModel<TProgressResult> : BaseViewModel
    {
        #region Events



        #endregion

        #region Fields

        private const string cm_PROGRESS_TEXT_PROPERTY_NAME = "ProgressText";
        private string m_ProgressText;

        private const string cm_DIALOG_TITLE_PROPERTY_NAME = "DialogTitle";
        private string m_DialogTitle;

        private const string cm_FLOW_DIRECTION_PROPERTY_NAME = "FlowDirection";
        private FlowDirection m_DialogFlowDirection;

        private readonly ICancellableTaskExecutor<CancellationToken> m_TaskExecutor;

        private readonly Action<CancellationToken> m_ProgressAction;
        private readonly Action m_ActionCompletionCallback;

        private readonly Func<CancellationToken, TProgressResult> m_ProgressFunction;
        private readonly Action<TProgressResult> m_FunctionCompletionCallback;

        private readonly Action m_CancellationCallback;
        private readonly CancellationToken m_ProgressCancellationToken;

        #region Commands

        private ICommand m_HandleViewLoadedCommand;
        private ICommand m_CancelProgressCommand;

        #endregion

        #endregion

        #region Constructors

        /// <inheritdoc />
        public IndefiniteProgressDialogViewModel(
            ICancellableTaskExecutor<CancellationToken> i_TaskExecutor,
            Action<CancellationToken> i_ProgressAction, CancellationToken i_CancellationToken,
            Action i_CancellationCallback, Action i_ActionCompletionCallback)
        {
            m_TaskExecutor = i_TaskExecutor;
            m_ProgressAction = i_ProgressAction;
            m_ProgressCancellationToken = i_CancellationToken;
            m_CancellationCallback = i_CancellationCallback;
            m_ActionCompletionCallback = i_ActionCompletionCallback;
        }

        /// <inheritdoc />
        public IndefiniteProgressDialogViewModel(
            ICancellableTaskExecutor<CancellationToken> i_TaskExecutor,
            Func<CancellationToken, TProgressResult> i_ProgressFunction,
            CancellationToken i_CancellationToken,
            Action i_CancellationCallback, Action<TProgressResult> i_FunctionCompletionCallback)
        {
            m_TaskExecutor = i_TaskExecutor;
            m_ProgressFunction = i_ProgressFunction;
            m_ProgressCancellationToken = i_CancellationToken;
            m_CancellationCallback = i_CancellationCallback;
            m_FunctionCompletionCallback = i_FunctionCompletionCallback;
        }

        #endregion

        #region Methods

        #region Event Handlers



        #endregion

        /// <summary>
        /// Handles the bound view's 'Loaded' event 
        /// by executing the progress action set in the constructor.
        /// </summary>
        /// <param name="i_O">Irrelevant.</param>
        private async void HandleViewLoaded(object i_O)
        {
            if (WindowService == null)
                throw new NullReferenceException("Window service must be set");

            if (m_ProgressAction == null && m_ProgressFunction == null)
                throw new NullReferenceException("Progress action or function must be set");

            try
            {
                if (m_ProgressAction != null)
                {
                    await m_TaskExecutor.Execute(m_ProgressAction, m_ProgressCancellationToken);
                    m_ActionCompletionCallback();
                }
                else if (m_ProgressFunction != null)
                {
                    var progressResult = await m_TaskExecutor.Execute(m_ProgressFunction,
                                             m_ProgressCancellationToken);
                    m_FunctionCompletionCallback(progressResult);
                }
            }
            catch (AggregateException aggregateException)
            {
                HandleFaultyExecution(aggregateException);
            }
            WindowService.CloseWindow(true);
        }

        /// <summary>
        /// Handles a faulty execution that has thrown an exception. <br />
        /// If the fault is a cancellation request, the method requests to close 
        /// the bound view's window.
        /// </summary>
        /// <param name="i_ThrownAggregateException">Exception representing the fault.</param>
        private void HandleFaultyExecution(AggregateException i_ThrownAggregateException)
        {
            if (i_ThrownAggregateException.InnerExceptions.Any(i_Exception =>
                i_Exception.GetType() == typeof(TaskCanceledException)))
            {
                WindowService.CloseWindow(false);
                m_CancellationCallback();
            }
            else
                throw i_ThrownAggregateException;
        }

        private void CancelProgress(object i_O)
        {
            
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the text displaying progress.
        /// </summary>
        public string ProgressText
        {
            get { return m_ProgressText; }
            set
            {
                SetValue(ref m_ProgressText, value);
                OnPropertyChanged(cm_PROGRESS_TEXT_PROPERTY_NAME, this);
            }
        }

        /// <summary>
        /// Gets or sets the dialog's title, shortly describing the progressed action.
        /// </summary>
        public string DialogTitle
        {
            get { return m_DialogTitle; }
            set
            {
                SetValue(ref m_DialogTitle, value);
                OnPropertyChanged(cm_DIALOG_TITLE_PROPERTY_NAME, this);
            }
        }

        /// <summary>
        /// Gets or sets the text/flow direction of the bound view.
        /// </summary>
        public FlowDirection DialogFlowDirection
        {
            get { return m_DialogFlowDirection; }
            set
            {
                SetValue(ref m_DialogFlowDirection, value);
                OnPropertyChanged(cm_FLOW_DIRECTION_PROPERTY_NAME, this);
            }
        }

        /// <summary>
        /// Gets or sets a service used to interact directly with the bound view's window.
        /// </summary>
        public IWindowService WindowService { get; set; }

        #region Commands

        /// <summary>
        /// Command used to handle the 'Loaded' event of the bound view.
        /// </summary>
        public ICommand HandleViewLoadedCommand
        {
            get
            {
                return m_HandleViewLoadedCommand ??
                       (m_HandleViewLoadedCommand =
                            new RelayCommand(HandleViewLoaded, i_O => true));
            }
        }

        /// <summary>
        /// Command used to cancel the executed progress.
        /// </summary>
        public ICommand CancelProgressCommand
        {
            get
            {
                return m_CancelProgressCommand ??
                       (m_CancelProgressCommand = new RelayCommand(CancelProgress, i_O => true));
            }
        }

        #endregion

        #endregion
    }
}