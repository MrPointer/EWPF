using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using EWPF.MVVM.Services;
using EWPF.Utility;
using EWPF_Demo.Model;
using EWPF_Demo.ViewModel;

namespace EWPF_Demo.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMessageBoxService
    {
        public MainWindow()
        {
            InitializeComponent();
            AssignServices();
        }

        #region Implementation of IService

        /// <inheritdoc />
        public void AssignServices()
        {
            var mainVM = DataContext as MainViewModel;
            if (mainVM == null)
                throw new InvalidCastException("Couldn't cast data context to MainViewModel object.");
            mainVM.MessageBoxService = this;
        }

        #endregion

        #region Implementation of IMessageBoxService

        /// <inheritdoc />
        public MessageBoxResult Show(string i_Caption, string i_Content, MessageBoxButton i_Buttons, MessageBoxImage i_Icon,
            MessageBoxResult i_DefaultResult = MessageBoxResult.OK, MessageBoxOptions i_ExtraOptions = MessageBoxOptions.None)
        {
            return MessageBoxUtility.ShowMessageBox(i_Caption, i_Content, i_Buttons, i_Icon, i_ExtraOptions,
                ConstantValues.DEFAULT_LANGUAGE, this);
        }

        /// <inheritdoc />
        public MessageBoxResult ShowNative(string i_Caption, string i_Content, MessageBoxButton i_Buttons, MessageBoxImage i_Icon,
            MessageBoxResult i_DefaultResult = MessageBoxResult.OK, MessageBoxOptions i_ExtraOptions = MessageBoxOptions.None)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
