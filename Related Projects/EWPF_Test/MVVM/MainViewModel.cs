using EWPF.MVVM;

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



        #endregion

        #region Other



        #endregion

        #endregion
    }
}