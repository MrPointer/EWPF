﻿using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EWPF.MVVM;
using EWPF.MVVM.Services;
using EWPF.Utility;
using EWPF_Demo.Model;

namespace EWPF_Demo.ViewModel
{
    /// <inheritdoc />
    /// <summary>
    /// A class representing a view model bound to the MainWindow view.
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        #region Events



        #endregion

        #region Fields

        #region Model

        private const string cm_TEXT_BOX_CONTENT_PROPERTY_NAME = "TextBoxContent";
        private string m_TextBoxContent;

        private const string cm_ACTIVE_THEME_PROPERTY_NAME = "ActiveTheme";
        private EWPFTheme m_ActiveTheme;

        private const string cm_PERSONS_PROPERTY_NAME = "Persons";
        private ObservableCollection<Person> m_Persons;

        /// <summary>
        /// Used to test a deep-hierarchy searching method defined in the base class.
        /// </summary>
        private const string cm_TEXT_BOX_LENGTH_PROPERTY_NAME = "TextBoxContent.Length";

        #endregion

        #region Commands

        private RelayCommand m_ButtonClickCommand;
        private ICommand m_ViewLoadedCommand;

        #endregion

        #region Other



        #endregion

        #endregion

        #region Constructors

        public MainViewModel()
        {
            Persons = new ObservableCollection<Person>
            {
                new Person {FirstName = "John", LastName = "Snow", Age = 21},
                new Person {FirstName = "Dayneris", LastName = "Targarian", Age = 20},
                new Person {FirstName = "Tyrion", LastName = "Lanister", Age = 35},
                new Person {FirstName = "Aryia", LastName = "Stark", Age = 17}
            };
            TextBoxContent = "Hello World!";
            OnPropertyChanged(cm_TEXT_BOX_LENGTH_PROPERTY_NAME, this);
        }

        #endregion

        #region Methods

        #region Event Handlers

        /// <summary>
        /// Handles the `Loaded` event of the bound view by initializing various aspects 
        /// of the view, such as <see cref="TextBox"/> contents, 
        /// <see cref="MessageBox"/> icons, etc.
        /// </summary>
        /// <param name="i_State">Irrelevant.</param>
        private void HandleViewLoaded(object i_State)
        {
            GetStartupTheme();

            // Set custom icons to message boxes
            string iconsDirPath =
                Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    @"..\..\..\..\")) +
                "Icons" + Path.DirectorySeparatorChar;

            ThemeUtility.LoadThemeIcons(iconsDirPath, ThemeUtility.CommonIconExtensions);

            string errorIconPath = ThemeUtility.GetIconPath("ErrorIcon");
            string warningIconPath = ThemeUtility.GetIconPath("WarningIcon");
            string questionIconPath = ThemeUtility.GetIconPath("QuestionIcon");
            string informationIconPath = ThemeUtility.GetIconPath("InformationIcon");

            MessageBoxUtility.SetCustomIcons(errorIconPath, warningIconPath,
                questionIconPath,
                informationIconPath);
        }

        #endregion

        #region Commands

        /// <summary>
        /// Shows a message box based on its' given type.
        /// </summary>
        /// <param name="i_State">Type of the message box to show.</param>
        private void ShowMessageBox(object i_State)
        {
            if (MessageBoxService == null) return;
            var result = MessageBoxService.Show(@"Test",
                @"Hello World! This is a sample content for a sample message box."
                + Environment.NewLine +
                "Did you like it? Please contribute the EWPF team online on Github!",
                MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            Console.WriteLine(result);
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
        public string TextBoxContent
        {
            get { return m_TextBoxContent; }
            set
            {
                SetValue(ref m_TextBoxContent, value);
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
                SetValue(ref m_ActiveTheme, value);
                OnPropertyChanged(cm_ACTIVE_THEME_PROPERTY_NAME, this);
                ApplyTheme();
            }
        }

        /// <summary>
        /// Gets or sets a collection of <see cref="Person"/>s.
        /// </summary>
        public ObservableCollection<Person> Persons
        {
            get { return m_Persons; }
            set
            {
                SetCollectionValue<ObservableCollection<Person>, Person>(ref m_Persons, value);
                OnPropertyChanged(cm_PERSONS_PROPERTY_NAME, this);
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Command used to show a message box upon a click on the button.
        /// </summary>
        public ICommand ButtonClickCommand
        {
            get
            {
                return m_ButtonClickCommand ?? (m_ButtonClickCommand =
                  new RelayCommand(ShowMessageBox, i_O => true));
            }
        }

        /// <summary>
        /// Command used to handle the `Loaded` event of the bound view.
        /// </summary>
        public ICommand ViewLoadedCommand
        {
            get
            {
                return m_ViewLoadedCommand ??
                       (m_ViewLoadedCommand =
                            new RelayCommand(HandleViewLoaded, i_O => true));
            }
        }

        #endregion

        #region Other

        /// <summary>
        /// Gets or sets a reference to a message box service used to show message boxes 
        /// on top of the bound view.
        /// </summary>
        public IMessageBoxService MessageBoxService { get; set; }

        #endregion

        #endregion
    }
}