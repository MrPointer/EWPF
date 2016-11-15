using System;

namespace EWPF.MVVM.Services
{
    /// <summary>
    /// An interface delcaring methods to dispatch actions on the bound view's dispatcher.
    /// <para/>
    /// This is a service-like interface implementing the DI pattern.
    /// </summary>
    public interface IDispatcherService
    {
        #region Fields

        #endregion

        #region Methods

        /// <summary>
        /// Request the dispatcher to execute the given action on the main UI thread.
        /// </summary>
        /// <param name="i_Action"></param>
        void DispatchAction(Action i_Action);

        #endregion

        #region Properties

        #endregion
    }
}