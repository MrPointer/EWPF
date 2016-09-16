using System.Collections.Generic;
using System.ComponentModel;

namespace EWPF.MVVM
{
    /// <summary>
    /// An interface declaring methods to notify WPF's binding system that a bounded property's value has changed.
    /// </summary>
    public interface INotifyPropertiesChanged : INotifyPropertyChanged
    {
        #region Fields

        #endregion

        #region Methods

        /// <summary>
        /// Notify that a single property value has changed.
        /// </summary>
        /// <param name="i_PropertyName">Property's name as listed in code.</param>
        /// <param name="i_InvokerViewModel">Reference to the invoking class.</param>
        void OnPropertyChanged(string i_PropertyName, BaseViewModel i_InvokerViewModel = null);

        /// <summary>
        /// Notify that numerous properties' values has changed.
        /// </summary>
        /// <param name="i_InvokerViewModel">Reference to the invoking class.</param>
        /// <param name="i_PropertiesNames">Properties' names as listed in code.</param>
        void OnPropertiesChanged(BaseViewModel i_InvokerViewModel, params string[] i_PropertiesNames);

        /// <summary>
        /// Notify that numerous properties' values has changed.
        /// </summary>
        /// <param name="i_PropertiesNames">IEnumerable of strings containing the peroperties' names as listed in code.</param>
        /// <param name="i_InvokerViewModel">Reference to the invoking class.</param>
        void OnPropertiesChanged(IEnumerable<string> i_PropertiesNames, BaseViewModel i_InvokerViewModel = null);

        #endregion

        #region Properties

        #endregion
    }
}