using System.Windows;
using System.Windows.Input;
using EWPF.MVVM.Services;

namespace EWPF.MVVM.ViewModel
{
    /// <summary>
    /// A class representing a view model bound to the EWPF themed message box.
    /// </summary>
    internal class EMsgBoxViewModel : BaseViewModel
    {
        #region Events



        #endregion

        #region Fields

        #region Model

        private const string cm_TITLE_PROPERTY_NAME = "Title";
        private string m_Title;

        private const string cm_CONTENT_PROPERTY_NAME = "Content";
        private string m_Content;

        private const string cm_POSITIVE_TEXT_PROPERTY_NAME = "PositiveText";
        private string m_PositiveText;

        private const string cm_NEGATIVE_TEXT_PROEPRTY_NAME = "NegativeText";
        private string m_NegativeText;

        private const string cm_NEUTRAL_TEXT_PROPERTY_NAME = "NeutralText";
        private string m_NeutralText;

        private const string cm_FLOW_DIRECTION_PROPERTY_NAME = "MBoxFlowDirection";
        private FlowDirection m_MBoxFlowDirection;

        #endregion

        #region Commands

        private RelayCommand m_PositiveCommand;
        private RelayCommand m_NegativeCommand;
        private RelayCommand m_NeutralCommand;

        #endregion

        #region Other



        #endregion

        #endregion

        #region Constructors



        #endregion

        #region Methods

        #region Command Executions

        private void HandlePositive(object i_State)
        {
            if (WindowService == null) return;
            WindowService.CloseWindow(true);
        }

        private void HandleNegative(object i_State)
        {
            if (WindowService == null) return;
            WindowService.CloseWindow(false);
        }

        private void HandleNeutral(object i_State)
        {
            if (WindowService == null) return;
            WindowService.CloseWindow(null);
        }

        #endregion

        #region Other



        #endregion

        #endregion

        #region Properties

        #region Model

        /// <summary>
        /// Gets or sets message box's title/cation.
        /// </summary>
        public string Title
        {
            get { return m_Title; }
            set
            {
                if (m_Title == value) return;
                m_Title = value;
                OnPropertyChanged(cm_TITLE_PROPERTY_NAME, this);
            }
        }

        /// <summary>
        /// Gets or sets message box's content.
        /// </summary>
        public string Content
        {
            get { return m_Content; }
            set
            {
                if (m_Content == value) return;
                m_Content = value;
                OnPropertyChanged(cm_CONTENT_PROPERTY_NAME, this);
            }
        }

        /// <summary>
        /// Gets or sets the text displayed in the positive button.
        /// </summary>
        public string PositiveText
        {
            get { return m_PositiveText; }
            set
            {
                if (m_PositiveText == value) return;
                m_PositiveText = value;
                OnPropertyChanged(cm_POSITIVE_TEXT_PROPERTY_NAME, this);
            }
        }

        /// <summary>
        /// Gets or sets the text displayed in the negative button.
        /// </summary>
        public string NegativeText
        {
            get { return m_NegativeText; }
            set
            {
                if (m_NegativeText == value) return;
                m_NegativeText = value;
                OnPropertyChanged(cm_NEGATIVE_TEXT_PROEPRTY_NAME, this);
            }
        }

        /// <summary>
        /// Gets or sets the text displayed in the neutral button.
        /// <para/>
        /// For exmaple, if the buttons displayed are "Yes", "No", and "Cancel" - "Cancel" is the neutral button.
        /// </summary>
        public string NeutralText
        {
            get { return m_NeutralText; }
            set
            {
                if (m_NeutralText == value) return;
                m_NeutralText = value;
                OnPropertyChanged(cm_NEUTRAL_TEXT_PROPERTY_NAME, this);
            }
        }

        /// <summary>
        /// Gets or sets message box's flow direction.
        /// </summary>
        public FlowDirection MBoxFlowDirection
        {
            get { return m_MBoxFlowDirection; }
            set
            {

                if (m_MBoxFlowDirection == value) return;
                m_MBoxFlowDirection = value;
                OnPropertyChanged(cm_FLOW_DIRECTION_PROPERTY_NAME, this);
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Command used to handle positive button clicks.
        /// </summary>
        public ICommand PositiveCommand
        {
            get { return m_PositiveCommand ?? (m_PositiveCommand = new RelayCommand(HandlePositive, i_O => true)); }
        }

        /// <summary>
        /// Command used to handle negative button clicks.
        /// </summary>
        public ICommand NegativeCommand
        {
            get { return m_NegativeCommand ?? (m_NegativeCommand = new RelayCommand(HandleNegative, i_O => true)); }
        }

        /// <summary>
        /// Command used to handle neutral button clicks.
        /// </summary>
        public ICommand NeutralCommand
        {
            get { return m_NeutralCommand ?? (m_NeutralCommand = new RelayCommand(HandleNeutral, i_O => true)); }
        }

        #endregion

        #region Other

        /// <summary>
        /// Gets or sets a reference to a window service used to interact with the view's bound window.
        /// </summary>
        public IWindowService WindowService { get; set; }

        #endregion

        #endregion
    }
}