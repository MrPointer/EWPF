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

        #endregion

        #endregion
    }
}
