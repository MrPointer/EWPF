using System;
using NUnit.Framework;

namespace EWPF_UnitTests.MVVM.BaseViewModel
{
    [TestFixture]
    public class RelayCommandTest
    {
        #region Events



        #endregion

        #region Fields

        private const string cm_NULL_ACTION_EXCPETION_MESSAGE = "Action to be executed can't be null";

        #endregion

        #region Constructors



        #endregion

        #region Methods

        #region Can Execute

        [Test]
        public void CanExecute_NullPredicate_ReturnsFalse()
        {
            var relayCommand = MvvmFactory.MakeRelayCommand();
            bool canExecute = relayCommand.CanExecute(null);
            Assert.False(canExecute);
        }

        [Test]
        public void CanExecute_FalsePredicate_ReturnsFalse()
        {
            var relayCommand = MvvmFactory.MakeRelayCommand(null, i_O => false);
            bool canExecute = relayCommand.CanExecute(null);
            Assert.False(canExecute);
        }

        [Test]
        public void CanExecute_TruePredicate_ReturnsTrue()
        {
            var relayCommand = MvvmFactory.MakeRelayCommand(null, i_O => true);
            bool canExecute = relayCommand.CanExecute(null);
            Assert.True(canExecute);
        }

        #endregion

        #region Execute

        [Test]
        public void Execute_NullAction_ThrowsNullReferenceException()
        {
            var relayCommand = MvvmFactory.MakeRelayCommand();
            var nullReferenceException = Assert.Catch<NullReferenceException>(() => relayCommand.Execute(null));
            StringAssert.AreEqualIgnoringCase(cm_NULL_ACTION_EXCPETION_MESSAGE, nullReferenceException.Message);
        }

        [Test]
        public void Execute_ValidAction_ChangesState()
        {
            var testVM = MvvmFactory.MakeTestViewModel();
            var action = new Action<object>(i_O => testVM.TestProperty = "Hello Unit Tests");
            var relayCommand = MvvmFactory.MakeRelayCommand(action, null);
            relayCommand.Execute(null);
            Assert.True(testVM.HasPropertyChanged);
        }

        #endregion

        #region Raise Can Execute Changed

        [Test]
        public void RaiseCanExecuteChanged_NewValue_ChangesState()
        {
            bool canExecute = false;
            var relayCommand = MvvmFactory.MakeRelayCommand();
            // ReSharper disable once AccessToModifiedClosure
            relayCommand.WhenToDo = i_O => canExecute;
            canExecute = true;
            relayCommand.RaiseCanExecuteChanged();
            bool canExecuteUpdated = relayCommand.CanExecute(null);
            Assert.True(canExecuteUpdated);
        }

        #endregion

        #region Can Execute Changed

        [Test]
        public void CanExecuteChanged_RegisteredMethod_IsUpdated()
        {
            var testVM = MvvmFactory.MakeTestViewModel();
            bool canExecute = false;

            // ReSharper disable once AccessToModifiedClosure
            var relayCommand = MvvmFactory.MakeRelayCommand(null, i_O => canExecute);

            relayCommand.CanExecuteChanged += (i_Sender, i_Args) =>
            {
                testVM.HasPropertyChanged = true;
            };

            canExecute = true;
            relayCommand.RaiseCanExecuteChanged();

            Assert.True(testVM.HasPropertyChanged);
        }

        #endregion

        #endregion

        #region Properties



        #endregion
    }
}