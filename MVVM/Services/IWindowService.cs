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

        #endregion

        #region Properties

        #endregion
    }
}