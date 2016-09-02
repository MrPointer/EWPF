using System;
using System.Windows.Input;

namespace EWPF
{
    /// <summary>
    /// Represents a typical WPF command (ICommand) but stores Action and Predicate objects within it,
    ///  used to execute the proper methods when needed in a generic way
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Events
        #endregion

        #region Fields

        /// <summary>
        /// Stores the method to execute when the command should be performed.
        /// </summary>
        private readonly Action<object> m_WhatToDo;

        /// <summary>
        /// Stores the method to check validity of execution when the command should be performed.
        /// </summary> 
        private readonly Predicate<object> m_WhenToDo;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new RelayCommand object based on the given Action and Predicate parameters.
        /// </summary>
        /// <param name="i_WhatToDo">Action to execute when this command executes.</param>
        /// <param name="i_WhenToDo">Predicate to check to determine weather the command is eligable to execute.</param>
        public RelayCommand(Action<object> i_WhatToDo, Predicate<object> i_WhenToDo = null)
        {
            m_WhatToDo = i_WhatToDo;
            m_WhenToDo = i_WhenToDo;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks the stored predicate against null and if not executes the predicate itself, and return its' output.
        /// </summary>
        /// <param name="i_Parameter">Extra data required for the predicate.</param>
        /// <returns>True if a predicate exists and its' output is also true, false otherwise.</returns>
        public bool CanExecute(object i_Parameter)
        {
            return m_WhenToDo != null && m_WhenToDo(i_Parameter);
        }

        /// <summary>
        /// Executes the stored action using the given object parameter.
        /// </summary>
        /// <param name="i_Parameter">Extra data required for the action.</param>
        public void Execute(object i_Parameter)
        {
            m_WhatToDo(i_Parameter);
        }

        /// <summary>
        /// Raises an event to reevaluate the state of the command, resulting in a call to <see cref="CanExecute"/>.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        #endregion

        #region Properties

        /// <summary>
        /// A subscription to this event also subscribes to <see cref="CommandManager.RequerySuggested"/>, 
        /// making it eligible for reevaluation.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        #endregion
    }
}