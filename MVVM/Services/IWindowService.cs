namespace EWPF.MVVM.Services
{
    /// <summary>
    /// An interface declaring methods to interact with the bound view's window.
    /// <para/>
    /// This is a service-like interface implementing the DI pattern.
    /// </summary>
    public interface IWindowService : IService
    {
        #region Fields

        #endregion

        #region Methods

        /// <summary>
        /// Requests the bound view's window to close itself.
        /// </summary>
        /// <param name="i_WindowResult">Result returned by this window.</param>
        void CloseWindow(bool? i_WindowResult);

        /// <summary>
        /// Requests the bound view's window to open a new window on top of it.
        /// </summary>
        /// <param name="i_Content">Usually used to indentify the window that should be opened - An enum is recommended.</param>
        /// <param name="i_DataContext">Window's data context (optional - if needs to be passed at runtime).</param>
        void OpenWindow(object i_Content, object i_DataContext = null);

        #endregion

        #region Properties

        #endregion
    }
}