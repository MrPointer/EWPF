using System.Windows;
using System.Windows.Input;
using EWPF.MVVM;
using EWPF.MVVM.Services;

namespace EWPF_Test.MVVM
{
    /// <summary>
    /// A class representing a view model bound to the MainWindow view.
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        #region Events



        #endregion

        #region Fields

        #region Model

        private const string cm_TEXT_BOX_CONTENT_PROPERTY_NAME = "TextboxContent";
        private string m_TextboxContent;

        /// <summary>
        /// Used to test a deep-hierarchy searching method defined in the base class.
        /// </summary>
        private const string cm_TEXT_BOX_LENGTH_PROPERTY_NAME = "TextboxContent.Length";

        #endregion

        #region Commands

        private RelayCommand m_ButtonClickCommand;

        #endregion

        #region Other



        #endregion

        #endregion

        #region Constructors

        public MainViewModel()
        {
            TextboxContent = "Hello World!";
            // Test Code!
            OnPropertyChanged(cm_TEXT_BOX_LENGTH_PROPERTY_NAME, this);
        }

        #endregion

        #region Methods

        #region Command Executions

        /// <summary>
        /// Shows a message box based on its' given type.
        /// </summary>
        /// <param name="i_State">Type of the message box to show.</param>
        private void ShowMessageBox(object i_State)
        {
            if (MessageBoxService == null) return;
            MessageBoxService.Show(@"Test", @"Hello World! This is a sample content for a sample message box." +
            "\n" + "Did you like it? Please contribute the EWPF team online on github!",
                MessageBoxButton.YesNoCancel, MessageBoxImage.Information);
        }

        #endregion

        #region Other



        #endregion

        #endregion

        #region Properties

        #region Model

        /// <summary>
        /// Gets or sets the content displayed inside the text box.
        /// </summary>
        public string TextboxContent
        {
            get { return m_TextboxContent; }
            set
            {
                if (m_TextboxContent != null && m_TextboxContent == value) return;
                m_TextboxContent = value;
                OnPropertyChanged(cm_TEXT_BOX_CONTENT_PROPERTY_NAME, this);
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Command used to show a message box upon a click on the button.
        /// </summary>
        public ICommand ButtonClickCommand
        {
            get { return m_ButtonClickCommand ?? (m_ButtonClickCommand = new RelayCommand(ShowMessageBox, i_O => true)); }
        }

        #endregion

        #region Other

        /// <summary>
        /// Gets or sets a reference to a meesage box service used to show message boxes on top of the bound view.
        /// </summary>
        public IMessageBoxService MessageBoxService { get; set; }

        #endregion

        #endregion
    }
}