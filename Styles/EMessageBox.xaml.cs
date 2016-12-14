using System;
using System.Windows;
using EWPF.MVVM.Services;
using EWPF.MVVM.ViewModel;

namespace EWPF.Styles
{
    /// <summary>
    /// Interaction logic for EMessageBox.xaml
    /// </summary>
    public partial class EMessageBox : Window, IWindowService
    {
        /// <summary>
        /// Constructs a new EWPF themed message box object.
        /// </summary>
        public EMessageBox()
        {
            InitializeComponent();
            AssignServices();
        }

        #region Methods

        #region Implementation of IService

        /// <summary>
        /// Assigns all implemented services to the bound view model. 
        /// </summary>
        public void AssignServices()
        {
            var mboxVM = DataContext as EMsgBoxViewModel;
            if (mboxVM == null)
                throw new InvalidCastException("Couldn't cast data context to EMsgBox view model.");
            mboxVM.WindowService = this;
        }

        #endregion

        #region Implementation of IWindowService

        /// <summary>
        /// Requests the bound view's window to close itself.
        /// </summary>
        /// <param name="i_WindowResult"></param>
        public void CloseWindow(bool? i_WindowResult)
        {
            switch (i_WindowResult)
            {
                case true:
                    Result = EDialogResult.Positive;
                    break;

                case null:
                    Result = EDialogResult.Neutral;
                    break;

                default:
                    Result = EDialogResult.Negative;
                    break;
            }
            Close();
        }

        /// <summary>
        /// Requests the bound view's window to open a new window on top of it.
        /// </summary>
        /// <param name="i_Content">Usually used to indentify the window that should be opened - An enum is recommended.</param>
        /// <param name="i_DataContext">Window's data context (optional - if needs to be passed at runtime).</param>
        public void OpenWindow(object i_Content, object i_DataContext = null)
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Gets the dialog result of the message box as it should be, since <see cref="Window.DialogResult"/> returns only true or false,
        /// which ignores a third 'neutral' state.
        /// </summary>
        public EDialogResult Result { get; private set; }

        #endregion
    }

    /// <summary>
    /// An enum listing values of a custom dialog result.
    /// </summary>
    public enum EDialogResult
    {
        /// <summary>
        /// Positive result - Yes/OK.
        /// </summary>
        Positive,
        /// <summary>
        /// Negative result - No/Cancel.
        /// </summary>
        Negative,
        /// <summary>
        /// Neutral result - Cancel.
        /// </summary>
        Neutral
    }
}
