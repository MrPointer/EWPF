using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace EWPF.MVVM.ViewModel
{
    public class IndefiniteProgressDialogViewModel : BaseViewModel
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

        private Action<CancellationToken> m_ProgressAction;
        private Func<CancellationToken, bool> m_ProgressFunction;
        private readonly CancellationToken m_ProgressCancellationToken;

        #region Commands

        private ICommand m_HandleViewLoadedCommand;

        #endregion

        #endregion

        #region Constructors

        /// <inheritdoc />
        public IndefiniteProgressDialogViewModel(Action<CancellationToken> i_ProgressAction,
            CancellationToken i_CancellationToken, Action i_CancellationCallback,
            Action i_CompletionCallback)
        {
            m_ProgressAction = i_ProgressAction;
            m_ProgressCancellationToken = i_CancellationToken;
        }

        /// <inheritdoc />
        public IndefiniteProgressDialogViewModel(Func<CancellationToken, bool> i_ProgressFunction,
            CancellationToken i_CancellationToken)
        {
            m_ProgressFunction = i_ProgressFunction;
            m_ProgressCancellationToken = i_CancellationToken;
        }

        #endregion

        #region Methods

        #region Event Handlers



        #endregion

        #region Commands

        /// <summary>
        /// Handles the bound view's 'Loaded' event 
        /// by executing the progress action set in the constructor.
        /// </summary>
        /// <param name="i_O">Irrelevant.</param>
        private void HandleViewLoaded(object i_O)
        {
            if (m_ProgressAction != null)
                m_ProgressAction(m_ProgressCancellationToken);
            else if (m_ProgressFunction != null)
                m_ProgressFunction(m_ProgressCancellationToken);
            else
                throw new NullReferenceException("Progress action or function must be set");
        }

        #endregion

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

        #endregion

        #endregion
    }
}