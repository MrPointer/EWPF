namespace EWPF.MVVM.Services
{
    /// <summary>
    /// An interface designed to report the progress made in some sort of a task, 
    /// as well as when it has started and completed.
    /// </summary>
    public interface IProgressService
    {
        #region Fields

        #endregion

        #region Methods

        /// <summary>
        /// Indicates that the bound task has started.
        /// </summary>
        void StartProgress();

        /// <summary>
        /// Indicates that the bound task has been stopped/canceled.
        /// </summary>
        void StopProgress();

        /// <summary>
        /// Indicates that the bound task has finished.
        /// </summary>
        void FinishProgress();

        /// <summary>
        /// Reports a change in the bound task affecting its progress.
        /// </summary>
        /// <param name="i_Amount">Amount of change.</param>
        void ReportProgress(int i_Amount);

        /// <summary>
        /// Reports a change in the bound task affecting its progress.
        /// </summary>
        /// <param name="i_Amount">Amount of change.</param>
        void ReportProgress(double i_Amount);

        #endregion

        #region Properties

        #endregion
    }
}