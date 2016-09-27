namespace EWPF.MVVM.Services
{
    /// <summary>
    /// An interface declaring methods to interact with the page navigation system of the bound view.
    /// <para/>
    /// This is a service-like interface implementing the DI pattern.
    /// </summary>
    public interface IPageService : IService
    {
        #region Fields

        #endregion

        #region Methods

        /// <summary>
        /// Navigates UI to the given page.
        /// </summary>
        /// <param name="i_NavigatedPage">Page to navigate to, stored as an object since user can represent it any way it'll like.</param>
        /// <param name="i_ExtraData">Extra data required for the operation to complete.</param>
        void GoToPage(object i_NavigatedPage, params object[] i_ExtraData);

        #endregion

        #region Properties

        #endregion
    }
}