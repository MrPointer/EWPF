using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using EWPF.MVVM;
using EWPF.MVVM.Services;
using EWPF.Utility;

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

        private const string cm_ACTIVE_THEME_PROPERTY_NAME = "ActiveTheme";
        private EWPFTheme m_ActiveTheme;

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

            GetStartupTheme();

            // Set custom icons to message boxes
            string iconsDirPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\")) +
                "Icons" + Path.DirectorySeparatorChar;
            string errorIconPath = iconsDirPath + "ErrorIcon.png";
            string warningIconPath = iconsDirPath + "WarningIcon.png";
            string questionIconPath = iconsDirPath + "QuestionIcon.png";
            string informationIconPath = iconsDirPath + "InformationIcon.png";
            MessageBoxUtility.SetCustomIcons(errorIconPath, warningIconPath, questionIconPath, informationIconPath);
        }

        #endregion

        #region Methods

        #region Commands

        /// <summary>
        /// Shows a message box based on its' given type.
        /// </summary>
        /// <param name="i_State">Type of the message box to show.</param>
        private void ShowMessageBox(object i_State)
        {
            if (MessageBoxService == null) return;
            MessageBoxService.Show(@"Test", @"Hello World! This is a sample content for a sample message box." +
            "\n" + "Did you like it? Please contribute the EWPF team online on github!",
                MessageBoxButton.OK, MessageBoxImage.Question);
        }

        #endregion

        #region Other

        /// <summary>
        /// Applies a new theme based on the <see cref="ActiveTheme"/> property.
        /// </summary>
        private void ApplyTheme()
        {
            ThemeUtility.LoadTheme(ActiveTheme);
        }

        /// <summary>
        /// Finds the active theme on startup and updates the <see cref="ActiveTheme"/> 
        /// property to display it in the bound view's ComboBox.
        /// </summary>
        private void GetStartupTheme()
        {
            m_ActiveTheme = ThemeUtility.GetCurrentEWPFTheme();
            ActiveTheme = m_ActiveTheme;
        }

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

        /// <summary>
        /// Gets or sets the active theme.
        /// </summary>
        public EWPFTheme ActiveTheme
        {
            get { return m_ActiveTheme; }
            set
            {
                if (m_ActiveTheme == value) return;
                m_ActiveTheme = value;
                OnPropertyChanged(cm_ACTIVE_THEME_PROPERTY_NAME, this);
                ApplyTheme();
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