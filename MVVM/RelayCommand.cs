using System;
using System.Windows.Input;

namespace EWPF.MVVM
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

        private EventHandler m_InternalCanExecuteChanged;

        #endregion

        #region Constructors

        /// <summary> 
        /// Constructs a new <see cref="RelayCommand"/> object, with all properties defaulting to null.
        /// </summary>
        public RelayCommand()
        {

        }

        /// <summary>
        /// Constructs a new RelayCommand object based on the given Action and Predicate parameters.
        /// </summary>
        /// <param name="i_WhatToDo">Action to execute when this command executes.</param>
        /// <param name="i_WhenToDo">Predicate to check to determine weather the command is eligable to execute.</param>
        public RelayCommand(Action<object> i_WhatToDo, Predicate<object> i_WhenToDo = null)
        {
            WhatToDo = i_WhatToDo;
            WhenToDo = i_WhenToDo;
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
            return WhenToDo != null && WhenToDo(i_Parameter);
        }

        /// <summary>
        /// Executes the stored action using the given object parameter.
        /// </summary>
        /// <param name="i_Parameter">Extra data required for the action.</param>
        public void Execute(object i_Parameter)
        {
            if (WhatToDo == null)
                throw new NullReferenceException("Action to be executed can't be null");
            WhatToDo(i_Parameter);
        }

        /// <summary>
        /// Raises an event to reevaluate the state of the command, resulting in a call to <see cref="CanExecute"/>.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            //CommandManager.InvalidateRequerySuggested();
            if (WhenToDo == null) return;
            var handler = m_InternalCanExecuteChanged;
            handler?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the action to execute when the command should be performed.
        /// </summary>
        public Action<object> WhatToDo { get; set; }

        /// <summary>
        /// Gets or sets the predicate to check to determine weather when the command should be performed.
        /// </summary> 
        public Predicate<object> WhenToDo { get; set; }

        /// <summary>
        /// A subscription to this event also subscribes to <see cref="CommandManager.RequerySuggested"/>, 
        /// making it eligible for reevaluation.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                m_InternalCanExecuteChanged += value;
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                m_InternalCanExecuteChanged -= value;
                CommandManager.RequerySuggested -= value;
            }
        }

        #endregion
    }
}