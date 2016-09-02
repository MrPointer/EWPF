using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace EWPF
{
    /// <summary>
    /// A base class for the ViewModel hierarchy, representing a basic view model that all view models should inherit from.
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertiesChanged
    {
        #region Events

        #endregion

        #region Fields
        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new BaseViewModel object, setting the <see cref="ThrowOnInvalidPropertyName"/>'s value to false.
        /// </summary>
        protected BaseViewModel()
        {
            ThrowOnInvalidPropertyName = false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Verifies that a given property name matches a real, public, instance property on this object. 
        /// </summary>
        /// <param name="i_PropertName">The property name to verify</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private void VerifyPropertyName(string i_PropertName)
        {
            if (TypeDescriptor.GetProperties(this)[i_PropertName] != null) return;
            string msg = "Invalid property name: " + i_PropertName;
            if (ThrowOnInvalidPropertyName)
                throw new Exception(msg);
            Debug.Fail(msg);
        }

        /// <summary>
        /// Fires multiple notifications to the WPF binding system for each property name in a given collection of properties.
        /// It also validates each property before firing the notification.
        /// </summary>
        /// <param name="i_PropertiesNames">Collection of property names as IEnumerable</param>
        /// <param name="i_InvokerViewModel">The View Model instance that requested the notification</param>
        private void OnManyProperitesChanged(IEnumerable<string> i_PropertiesNames, BaseViewModel i_InvokerViewModel)
        {
            if (i_InvokerViewModel == null) i_InvokerViewModel = this;
            if (PropertyChanged == null) return;
            foreach (string property in i_PropertiesNames)
            {
                VerifyPropertyName(property);
                PropertyChanged(i_InvokerViewModel, new PropertyChangedEventArgs(property));
            }
        }

        /// <summary>
        /// Fires a notification to the WPF binding system, telling it a given property has changed it's value.
        /// </summary>
        /// <param name="i_PropertyName">The changed property name</param>
        /// <param name="i_InvokerViewModel">The View Model instance that requested the notification</param>
        public void OnPropertyChanged(string i_PropertyName, BaseViewModel i_InvokerViewModel = null)
        {
            VerifyPropertyName(i_PropertyName);
            if (i_InvokerViewModel == null) i_InvokerViewModel = this;
            if (PropertyChanged != null)
                PropertyChanged(i_InvokerViewModel, new PropertyChangedEventArgs(i_PropertyName));
        }

        /// <summary>
        /// Fires multiple notifications to the WPF binding system for each property name in a given collection of properties.
        /// </summary>
        /// <param name="i_InvokerViewModel">The View Model instance that requested the notification</param>
        /// <param name="i_PropertiesNames">Collection of property names as an array of strings</param>
        public void OnPropertiesChanged(BaseViewModel i_InvokerViewModel, params string[] i_PropertiesNames)
        {
            OnManyProperitesChanged(i_PropertiesNames, i_InvokerViewModel);
        }

        /// <summary>
        /// Fires multiple notifications to the WPF binding system for each property name in a given collection of properties.
        /// </summary>
        /// <param name="i_PropertiesNames">Collection of property names as IEnumerable</param>
        /// <param name="i_InvokerViewModel">The View Model instance that requested the notification</param>
        public void OnPropertiesChanged(IEnumerable<string> i_PropertiesNames, BaseViewModel i_InvokerViewModel = null)
        {
            OnManyProperitesChanged(i_PropertiesNames, i_InvokerViewModel);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates weather to throw an exception in case when a given property name isn't valid or just fail the whole debug operation.
        /// </summary>
        public bool ThrowOnInvalidPropertyName { get; set; }

        /// <summary>
        /// The View Model's display name, used to display it on the screen if needed.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// An event raised when a property's value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}