using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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

        /// <summary>
        /// Sets the window's icon from the given path.
        /// </summary>
        /// <param name="i_IconPath">Path to an icon file, which could be any WPF-supported image format.</param>
        public void SetIcon(string i_IconPath)
        {
            if (string.IsNullOrEmpty(i_IconPath)) // Do nothing - Icon is left blank
                return;
            switch (i_IconPath)
            {
                case IconStrings.ERROR:
                    IconImage.Source = SystemIcons.Error.ToImageSource();
                    break;

                case IconStrings.WARNING:
                    IconImage.Source = SystemIcons.Warning.ToImageSource();
                    break;

                case IconStrings.QUESTION:
                    IconImage.Source = SystemIcons.Question.ToImageSource();
                    break;

                case IconStrings.INFORMATION:
                    IconImage.Source = SystemIcons.Information.ToImageSource();
                    break;

                default:
                    var iconFileInfo = new FileInfo(i_IconPath);
                    if (!iconFileInfo.Exists)
                        throw new ArgumentException(@"Given icon's path doesn't exist", i_IconPath);
                    // ToDo: Check file's extension
                    IconImage.Source = new BitmapImage(new Uri(i_IconPath));
                    break;
            }
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

    /// <summary>
    /// A static class listing all native icons as strings.
    /// </summary>
    internal static class IconStrings
    {
        /// <summary>
        /// Error/Stop/Hand icon.
        /// </summary>
        internal const string ERROR = "Error";

        /// <summary>
        /// Warning/Exclemation icon.
        /// </summary>
        internal const string WARNING = "Warning";

        /// <summary>
        /// Question icon.
        /// </summary>
        internal const string QUESTION = "Question";

        /// <summary>
        /// Information icon.
        /// </summary>
        internal const string INFORMATION = "Information";
    }

    /// <summary>
    /// A static utility class providing utility methods for <see cref="Icon"/> objects.
    /// </summary>
    internal static class IconUtilities
    {
        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr i_HObject);

        public static ImageSource ToImageSource(this Icon i_Icon)
        {
            var bitmap = i_Icon.ToBitmap();
            var hBitmap = bitmap.GetHbitmap();

            ImageSource wpfBitmap = Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            if (!DeleteObject(hBitmap))
                throw new Win32Exception();

            return wpfBitmap;
        }
    }
}
