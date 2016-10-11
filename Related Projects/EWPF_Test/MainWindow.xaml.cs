using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using EWPF.MVVM.Services;
using EWPF.Utility;
using EWPF_Test.MVVM;

namespace EWPF_Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMessageBoxService
    {
        private bool m_IsActive;

        public MainWindow()
        {
            InitializeComponent();
            m_IsActive = true;
            AssignServices();
            //DisplayBusyIndicatorAnimation();
            //DisplayButtonStatesPereodically();
        }

        /// <summary>
        /// Displays a playback-like animation on the busy indicator using its' start and stop methods.
        /// </summary>
        private void DisplayBusyIndicatorAnimation()
        {
            Task.Factory.StartNew(() =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    BusyIndicator.IsAnimated = true;
                }));
                Thread.Sleep(1500);
                Dispatcher.Invoke(new Action(() =>
                {
                    BusyIndicator.IsAnimated = false;
                }));
            });
        }

        private void DisplayButtonStatesPereodically()
        {
            Task.Factory.StartNew(() =>
            {
                while (m_IsActive)
                {
                    Thread.Sleep(2000);
                    Dispatcher.Invoke(new Action(() =>
                    {
                        MainBtn.IsEnabled = true;
                    }));
                    Thread.Sleep(5000);
                    Dispatcher.Invoke(new Action(() =>
                    {
                        MainBtn.IsEnabled = false;
                    }));
                }
            });
        }

        #region Implementation of IService

        /// <summary>
        /// Assigns all implemented services to the bound view model. 
        /// </summary>
        public void AssignServices()
        {
            var mainVM = DataContext as MainViewModel;
            if (mainVM == null)
                throw new InvalidCastException("Couldn't cast data context to MainViewModel object.");
            mainVM.MessageBoxService = this;
        }

        #endregion

        #region Implementation of IMessageBoxService

        /// <summary>
        /// Shows an EWPF themed message box based on the given parameters.
        /// </summary>
        /// <param name="i_Caption">Message box's caption(Header).</param>
        /// <param name="i_Content">Message box's content(Body).</param>
        /// <param name="i_Buttons">Message box's button/s.</param>
        /// <param name="i_Icon">Message box's icon.</param>
        /// <param name="i_DefaultResult">Message box's default return result.</param>
        /// <param name="i_ExtraOptions">Extra message box options.</param>
        /// <returns>Message box's result after an interaction with the user.</returns>
        public MessageBoxResult Show(string i_Caption, string i_Content, MessageBoxButton i_Buttons, MessageBoxImage i_Icon,
            MessageBoxResult i_DefaultResult = MessageBoxResult.OK, MessageBoxOptions i_ExtraOptions = MessageBoxOptions.None)
        {
            return MessageBoxUtility.ShowMessageBox(i_Caption, i_Content, i_Buttons, i_Icon, i_ExtraOptions, this);
        }

        /// <summary>
        /// Shows a windows native message box based on the given parameters.
        /// </summary>
        /// <param name="i_Caption">Message box's caption(Header).</param>
        /// <param name="i_Content">Message box's content(Body).</param>
        /// <param name="i_Buttons">Message box's button/s.</param>
        /// <param name="i_Icon">Message box's icon.</param>
        /// <param name="i_DefaultResult">Message box's default return result.</param>
        /// <param name="i_ExtraOptions">Extra message box options.</param>
        /// <returns>Message box's result after an interaction with the user.</returns>
        public MessageBoxResult ShowNative(string i_Caption, string i_Content, MessageBoxButton i_Buttons, MessageBoxImage i_Icon,
            MessageBoxResult i_DefaultResult = MessageBoxResult.OK, MessageBoxOptions i_ExtraOptions = MessageBoxOptions.None)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Overrides of Window

        /// <summary>Raises the <see cref="E:System.Windows.Window.Closed" /> event.</summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            m_IsActive = false;
        }

        #endregion
    }
}
