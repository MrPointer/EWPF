using System;
using EWPF.MVVM;

namespace EWPF_Tests.Unit.MVVM
{
    /// <summary>
    /// A static factory class providing static methods to make instances of MVVM-related objects using a factory.
    /// </summary>
    public static class MvvmFactory
    {
        #region Events

        #endregion

        #region Fields



        #endregion

        #region Constructors

        #endregion

        #region Methods

        /// <summary>
        /// Makes a new instance of the <see cref="TestableViewModel"/> class.
        /// </summary>
        /// <returns>Created instance.</returns>
        public static TestableViewModel MakeTestViewModel()
        {
            return new TestableViewModel();
        }

        /// <summary>
        /// Makes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="i_WhatToDo">Action to execute when invoked.</param>
        /// <param name="i_WhenToDo">Predicate to check to determine weather the given action can be performed or not.</param>
        /// <returns>Created instance.</returns>
        public static RelayCommand MakeRelayCommand(Action<object> i_WhatToDo, Predicate<object> i_WhenToDo)
        {
            return new RelayCommand(i_WhatToDo, i_WhenToDo);
        }

        /// <summary>
        /// Makes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <returns>Created instance.</returns>
        public static RelayCommand MakeRelayCommand()
        {
            return new RelayCommand();
        }

        #endregion

        #region Properties



        #endregion
    }
}