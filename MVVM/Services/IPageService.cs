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
        void GoToPage(object i_NavigatedPage);

        /// <summary>
        /// Navigates to the next page in the navigation system.
        /// </summary>
        void NextPage();

        /// <summary>
        /// Navigates to the previous page in the navigation system.
        /// </summary>
        void PreviousPage();

        #endregion

        #region Properties

        #endregion
    }
}