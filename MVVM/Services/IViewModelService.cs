namespace EWPF.MVVM.Services
{
    /// <inheritdoc />
    /// <summary>
    /// An interface designed to assign ViewModel instances to the implementing view 
    /// in a loose coupled way, providing an easy solution for ViewModel constructors that take parameters.
    /// </summary>
    public interface IViewModelService : IService
    {
        #region Fields

        #endregion

        #region Methods

        /// <summary>
        /// Sets the given ViewModel to one of the view's properties, usually the DataContext.
        /// </summary>
        /// <param name="i_ViewModel">ViewModel instance to set.</param>
        void SetViewModel(BaseViewModel i_ViewModel);

        #endregion

        #region Properties

        #endregion
    }
}