using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

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

        #region Implementation of INotifyPropertiesChanged

        /// <summary>
        /// Fires a notification to the WPF binding system, telling it a given property has changed it's value.
        /// </summary>
        /// <param name="i_PropertyName">The changed property name</param>
        /// <param name="i_InvokerViewModel">The View Model instance that requested the notification</param>
        public void OnPropertyChanged(string i_PropertyName, BaseViewModel i_InvokerViewModel = null)
        {
            if (i_InvokerViewModel == null)
                i_InvokerViewModel = this;
            IsPropertyNameValid(i_PropertyName, i_InvokerViewModel);
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
            if (i_InvokerViewModel == null)
                throw new ArgumentNullException("i_InvokerViewModel",
                    @"Invoker can't be null - Must be set to the view model's instance");

            OnManyPropertiesChanged(i_PropertiesNames, i_InvokerViewModel);
        }

        /// <summary>
        /// Fires multiple notifications to the WPF binding system for each property name in a given collection of properties.
        /// </summary>
        /// <param name="i_PropertiesNames">Collection of property names as IEnumerable</param>
        /// <param name="i_InvokerViewModel">The View Model instance that requested the notification</param>
        public void OnPropertiesChanged(IEnumerable<string> i_PropertiesNames, BaseViewModel i_InvokerViewModel = null)
        {
            if (i_InvokerViewModel == null)
                i_InvokerViewModel = this;
            OnManyPropertiesChanged(i_PropertiesNames, i_InvokerViewModel);
        }

        #endregion

        #region Property Setters

        /// <summary>
        /// Sets a new given value to the given property the 'MVVM' way.
        /// <para />
        /// It checks for equality first and only if they're unequal - It assigns the new value.
        /// </summary>
        /// <typeparam name="T">Type of the property (and value) to set.</typeparam>
        /// <param name="i_PropertyToSet">Reference to the property that should be set. Ref is used to keep it permanent.</param>
        /// <param name="i_NewValueToAssign">Reference to the value that should be assigned. Must not be null.</param>
        /// <returns>True if a new value has been set, false otherwise.</returns>
        public static bool SetValue<T>(ref T i_PropertyToSet, T i_NewValueToAssign)
        {
            if (i_NewValueToAssign == null)
                return false;

            if (i_PropertyToSet != null && i_PropertyToSet.Equals(i_NewValueToAssign))
                return false;
            i_PropertyToSet = i_NewValueToAssign;
            return true;
        }

        /// <summary>
        /// Sets a new given collection value to the given collection property, the 'MVVM' way.
        /// <para />
        /// It checks for equality first and only if they're unequal - It assigns the new value.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="IEnumerable{T}"/> being set.</typeparam>
        /// <typeparam name="TS">Type of the elements the <see cref="IEnumerable{T}"/> type holds.</typeparam>
        /// <param name="i_PropertyToSet">Reference to the property that should be set. Ref is used to keep it permanent.</param>
        /// <param name="i_NewValueToAssign">Reference to the value that should be assigned. Must not be null.</param>
        /// <param name="i_Comparer">Reference to a comparer that is capable of comparing two values of the TS type.</param>
        /// <returns>True if a new value has been set, false otherwise.</returns>
        public static bool SetCollectionValue<T, TS>(ref T i_PropertyToSet, T i_NewValueToAssign,
            IEqualityComparer<TS> i_Comparer = null)
            where T : IEnumerable<TS>
        {
            if (i_NewValueToAssign == null)
                return false;

            if (i_PropertyToSet != null && i_PropertyToSet.SequenceEqual(i_NewValueToAssign, i_Comparer))
                return false;
            i_PropertyToSet = i_NewValueToAssign;
            return true;
        }

        #endregion

        #region Validation    

        /// <summary>
        /// Verifies that a given property name matches a real, public, instance property on this object. 
        /// </summary>
        /// <param name="i_PropertyName">Property name to verify.</param>
        /// <param name="i_Invoker">Reference to the view model storing the given property.</param>
        internal bool IsPropertyNameValid(string i_PropertyName, BaseViewModel i_Invoker)
        {
            if (string.IsNullOrEmpty(i_PropertyName))
                throw new ArgumentException(@"Property name can't be null or empty", "i_PropertyName");
            if (i_Invoker == null)
                throw new ArgumentNullException("i_Invoker", @"Invoker can't be null - Must be set to the view model's instance");

            if (TypeDescriptor.GetProperties(i_Invoker)[i_PropertyName] != null) // Property found
                return true;

            // Property may be missing DIRECTLY on the given object, but is actually referred to as an inner property
            // If given string has a dot (.) in it, this might be the case, otherwise, it's not valid
            if (!Regex.IsMatch(i_PropertyName, @"\."))
            {
                if (ThrowOnInvalidPropertyName)
                    throw new ArgumentException(@"Given property name is direct but invalid", "i_PropertyName");
                return false;
            }

            string splitPropertyName = i_PropertyName;
            var invokingViewModel = i_Invoker;

            PropertyDescriptor seekedPropertyDescriptor = null; // Will store the property we're searching for
            string parentPropertyName; // Stores the name of the property before the "." character/s
            bool isLastChildProperty = false; // Indicates weather the tested inner property is the last in "hierarchy"
            bool isPropertyExist = true;

            do
            {
                // Take all chars until first dot, excluded
                parentPropertyName = i_PropertyName.Substring(0, i_PropertyName.IndexOf('.'));

                // Try to get the parent property descriptor - If fail, fail everything
                if (invokingViewModel != null)
                    seekedPropertyDescriptor = TypeDescriptor.GetProperties(invokingViewModel)[parentPropertyName];
                else
                    seekedPropertyDescriptor = seekedPropertyDescriptor.GetChildProperties()[parentPropertyName];

                if (seekedPropertyDescriptor == null) // Property not found
                    isPropertyExist = false; // Could write return, but preferred the 'logical' method instead

                else // Property found
                {
                    // Remove the previously subbed string from the original one
                    splitPropertyName = splitPropertyName.Remove(0, parentPropertyName.Length + 1);
                    isLastChildProperty = !Regex.IsMatch(splitPropertyName, @"\.");
                    if (isLastChildProperty) // We've reached the end of the hierarchy
                    {
                        seekedPropertyDescriptor = seekedPropertyDescriptor.GetChildProperties()[splitPropertyName];
                        isPropertyExist = seekedPropertyDescriptor != null; // Check if property exists
                    }
                    invokingViewModel = null;
                }
            } while (isPropertyExist && !isLastChildProperty);

            if (isPropertyExist) return true;
            if (ThrowOnInvalidPropertyName)
                throw new ArgumentException(@"Given property name is nested but invalid", "i_PropertyName");
            return false;
        }

        #endregion

        #region Helper

        /// <summary>
        /// Fires multiple notifications to the WPF binding system for each property name in a given collection of properties.
        /// It also validates each property before firing the notification.
        /// </summary>
        /// <param name="i_PropertiesNames">Collection of property names as IEnumerable</param>
        /// <param name="i_Invoker">The View Model instance that requested the notification</param>
        internal void OnManyPropertiesChanged(IEnumerable<string> i_PropertiesNames, BaseViewModel i_Invoker)
        {
            if (i_PropertiesNames == null)
                throw new ArgumentNullException("i_PropertiesNames", @"Collection of properties can't be null");
            if (i_Invoker == null)
                throw new ArgumentNullException("i_Invoker", @"Invoker can't be null - Must be set to the view model's instance");

            foreach (string property in i_PropertiesNames)
            {
                bool isNameVerified = IsPropertyNameValid(property, i_Invoker);
                if (!isNameVerified)
                    throw new ArgumentException(@"One of the properties' name is invalid", "i_PropertiesNames");
                if (PropertyChanged != null)
                    PropertyChanged(i_Invoker, new PropertyChangedEventArgs(property));
            }
        }

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Indicates weather to throw an exception in case when a given property name isn't valid or just return false.
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