using System;
using System.Reflection;
using System.Windows;
using EWPF.MVVM.Services;
using EWPF.MVVM.ViewModel;
using EWPF.Utility;

namespace EWPF.Dialogs
{
    /// <summary>
    /// Interaction logic for IndefiniteProgressDialog.xaml
    /// </summary>
    public partial class IndefiniteProgressDialog : Window, IWindowService
    {
        #region Constants

        private const string cm_VIEW_MODEL_WINDOW_SERVICE_PROPERTY_NAME = "WindowService";

        #endregion

        #region Constructors

        /// <inheritdoc />
        public IndefiniteProgressDialog(object i_DataContext)
        {
            DataContext = i_DataContext;
            InitializeComponent();
            AssignServices();
        }

        #endregion

        #region Implementation of IService

        /// <inheritdoc />
        public void AssignServices()
        {
            var vmType = DataContext.GetType();
            var vmWindowServiceProperty = vmType.GetProperty(
                cm_VIEW_MODEL_WINDOW_SERVICE_PROPERTY_NAME,
                BindingFlags.Public | BindingFlags.Instance);
            if (vmWindowServiceProperty == null)
            {
                throw new Exception(string.Format("{0} property isn't found in {1}",
                    cm_VIEW_MODEL_WINDOW_SERVICE_PROPERTY_NAME, vmType));
            }
            vmWindowServiceProperty.SetValue(DataContext, this);
        }

        #endregion

        #region Implementation of IWindowService

        /// <inheritdoc />
        public void CloseWindow(bool? i_WindowResult)
        {
            WindowUtility.CloseWindow(this, i_WindowResult, Dispatcher);
        }

        /// <inheritdoc />
        public void OpenWindow(object i_Content, object i_DataContext = null)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public void SetIcon(string i_IconPath) { throw new System.NotImplementedException(); }

        #endregion
    }
}
