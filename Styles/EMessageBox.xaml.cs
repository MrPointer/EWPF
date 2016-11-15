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
            DialogResult = i_WindowResult;
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
    }
}
