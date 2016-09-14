using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace EWPF_Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //DisplayBusyIndicatorAnimation();
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
    }
}
