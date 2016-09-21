namespace EWPF.MVVM.Services
{
    /// <summary>
    /// A base interface declaring a single method that requires caller to assign the implemented services to its' view model.
    /// </summary>
    public interface IService
    {
        #region Fields

        #endregion

        #region Methods

        /// <summary>
        /// Assigns all implemented services to the bound view model. 
        /// </summary>
        void AssignServices();

        #endregion

        #region Properties

        #endregion
    }
}