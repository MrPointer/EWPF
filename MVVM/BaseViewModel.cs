using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace EWPF.MVVM
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
        /// <param name="i_PropertyName">The property name to verify</param>
        /// <param name="i_Invoker"></param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private void VerifyPropertyName(string i_PropertyName, object i_Invoker)
        {
            if (TypeDescriptor.GetProperties(i_Invoker)[i_PropertyName] != null) // Property found
                return;

            // Property may be missing DIRECTLY on the given object, but is actually referred to as an inner property

            PropertyDescriptor seekedPropertyDescriptor = null; // Will store the property we're seraching for
            string parentPropertyName; // Stores the name of the property before the "." character/s
            bool isLastChildProperty = false; // Indicates weather the tested inner property is the last in "hierarchy"
            bool isPropertyExist = true;

            do
            {
                // Take all chars until first dot, excluded
                parentPropertyName = i_PropertyName.Substring(0, i_PropertyName.IndexOf('.'));

                // Try to get the parent property descriptor - If fail, fail everything
                if (i_Invoker != null) // Given view model object is used
                    seekedPropertyDescriptor = TypeDescriptor.GetProperties(i_Invoker)[parentPropertyName];
                else if (seekedPropertyDescriptor != null) // Inner property is used
                    seekedPropertyDescriptor = seekedPropertyDescriptor.GetChildProperties()[parentPropertyName];

                if (seekedPropertyDescriptor == null) // Property not found
                    isPropertyExist = false;
                else // Property found
                {
                    // Remove the previously subbed string from the original one
                    i_PropertyName = i_PropertyName.Remove(0, parentPropertyName.Length + 1);
                    isLastChildProperty = !i_PropertyName.Contains(".");
                    if (isLastChildProperty) // We've reached the end of the hierarchy
                    {
                        seekedPropertyDescriptor = seekedPropertyDescriptor.GetChildProperties()[i_PropertyName];
                        isPropertyExist = seekedPropertyDescriptor != null; // Check if property exists
                    }
                    i_Invoker = null;
                }
            } while (isPropertyExist && !isLastChildProperty);

            if (isPropertyExist) return;
            string msg = "Invalid property name: " + i_PropertyName;
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
        private void OnManyPropertiesChanged(IEnumerable<string> i_PropertiesNames, BaseViewModel i_InvokerViewModel)
        {
            if (i_InvokerViewModel == null) i_InvokerViewModel = this;
            if (PropertyChanged == null) return;
            foreach (string property in i_PropertiesNames)
            {
                VerifyPropertyName(property, i_InvokerViewModel);
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
            VerifyPropertyName(i_PropertyName, i_InvokerViewModel);
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
            OnManyPropertiesChanged(i_PropertiesNames, i_InvokerViewModel);
        }

        /// <summary>
        /// Fires multiple notifications to the WPF binding system for each property name in a given collection of properties.
        /// </summary>
        /// <param name="i_PropertiesNames">Collection of property names as IEnumerable</param>
        /// <param name="i_InvokerViewModel">The View Model instance that requested the notification</param>
        public void OnPropertiesChanged(IEnumerable<string> i_PropertiesNames, BaseViewModel i_InvokerViewModel = null)
        {
            OnManyPropertiesChanged(i_PropertiesNames, i_InvokerViewModel);
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