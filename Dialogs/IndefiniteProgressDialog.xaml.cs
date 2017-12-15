using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using EWPF.MVVM.Services;
using EWPF.MVVM.ViewModel;
using EWPF.Utility;
using KISLogger;

namespace EWPF.Dialogs
{
    /// <summary>
    /// Interaction logic for IndefiniteProgressDialog.xaml
    /// </summary>
    public partial class IndefiniteProgressDialog : Window, IWindowService
    {
        #region Constants

        private const string cm_VIEW_MODEL_WINDOW_SERVICE_PROPERTY_NAME = "WindowService";

        private const string cm_VIEW_MODEL_HANDLE_VIEW_LOADED_COMMAND_NAME =
            "HandleViewLoadedCommand";

        private const string cm_VIEW_MODEL_CANCEL_PROGRESS_COMMAND_NAME = "CancelProgressCommand";

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

        #region Methods

        #region Event Handlers

        /// <summary>
        /// Handles a <see cref="FrameworkElement.Loaded"/> event by 
        /// executing a matching command in the bound view-model.
        /// </summary>
        /// <param name="i_Sender">Window instance.</param>
        /// <param name="i_E">Irrelevant.</param>
        private void OnWindowLoaded(object i_Sender, RoutedEventArgs i_E)
        {
            try
            {
                var handleViewLoadedCommand =
                        GetCommandFromViewModelProperty(cm_VIEW_MODEL_HANDLE_VIEW_LOADED_COMMAND_NAME);
                if (handleViewLoadedCommand.CanExecute(null))
                    handleViewLoadedCommand.Execute(null);
            }
            catch (Exception ex)
            {
                Log.LogException(ex);
            }
        }

        /// <summary>
        /// Handles a <see cref="Window.Closed"/> event by canceling the executed progress
        /// in the bound view-model.
        /// </summary>
        /// <param name="i_Sender">Window instance.</param>
        /// <param name="i_E">Irrelevant.</param>
        private void OnWindowClosed(object i_Sender, EventArgs i_E)
        {
            try
            {
                var cancelProgressCommand =
            GetCommandFromViewModelProperty(cm_VIEW_MODEL_CANCEL_PROGRESS_COMMAND_NAME);
                if (cancelProgressCommand.CanExecute(null))
                    cancelProgressCommand.Execute(null);
            }
            catch (Exception ex)
            {
                Log.LogException(ex);
            }
        }

        #endregion

        #region Implementation of IService

        /// <inheritdoc />
        public void AssignServices()
        {
            var vmType = DataContext.GetType();
            var vmWindowServiceProperty =
                vmType.GetProperty(cm_VIEW_MODEL_WINDOW_SERVICE_PROPERTY_NAME,
                    typeof(IWindowService));
            if (vmWindowServiceProperty == null)
            {
                throw new Exception(string.Format("{0} property not found in {1}",
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

        /// <summary>
        /// Retrieves a command property with the given name 
        /// from the <see cref="FrameworkElement.DataContext"/> using reflection.
        /// </summary>
        /// <param name="i_CommandName">Command's property name. Case sensitive.</param>
        /// <returns>Retrieved command.</returns>
        /// <exception cref="Exception">Command property with given name not found 
        /// in <see cref="FrameworkElement.DataContext"/>'s underlying type.</exception>
        /// <exception cref="InvalidCastException">Command property 
        /// couldn't be cast to <see cref="ICommand"/>.</exception>
        private ICommand GetCommandFromViewModelProperty(string i_CommandName)
        {
            var vmType = DataContext.GetType();
            var commandProperty = vmType.GetProperty(i_CommandName, typeof(ICommand));
            if (commandProperty == null)
            {
                throw new Exception(string.Format("{0} property not found in {1}",
                    i_CommandName, vmType));
            }
            var command = commandProperty.GetValue(DataContext) as ICommand;
            if (command == null)
            {
                throw new InvalidCastException(string.Format("Expected {0} to be a {1}",
                    commandProperty, typeof(ICommand)));
            }
            return command;
        }

        #endregion
    }
}
