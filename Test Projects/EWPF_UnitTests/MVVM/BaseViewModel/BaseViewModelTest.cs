using System;
using System.ComponentModel;
using NUnit.Framework;

namespace EWPF_UnitTests.MVVM.BaseViewModel
{
    [TestFixture]
    public class BaseViewModelTest
    {
        #region Events



        #endregion

        #region Fields

        private const string cm_TESTABLE_PROPERTY_NAME = "TestProperty";

        #endregion

        #region Constructors



        #endregion

        #region Methods

        [Test]
        public void OnPropertyChanged_NullInvoker_NotFailing()
        {
            var testVM = MakeTestViewModel();
            Assert.DoesNotThrow(() => testVM.OnPropertyChanged(cm_TESTABLE_PROPERTY_NAME, null));
        }

        [Test]
        public void OnPropertyChanged_DifferentValue_ChangesState()
        {
            var testVM = MakeTestViewModel();
            testVM.TestProperty = "Hello Unit Test";
            testVM.TestProperty = "New Test";
            Assert.True(testVM.HasPropertyChanged);
        }

        [Test]
        public void OnPropertyChanged_SameValue_DoesntChangeState()
        {
            var testVM = MakeTestViewModel();
            testVM.TestProperty = "Hello Unit Test";
            testVM.TestProperty = "Hello Unit Test";
            Assert.False(testVM.HasPropertyChanged);
        }

        #region Factory Methods

        /// <summary>
        /// Makes a new instance of the <see cref="TestableViewModel"/> class.
        /// </summary>
        /// <returns>Created instance.</returns>
        private static TestableViewModel MakeTestViewModel()
        {
            return new TestableViewModel();
        }

        #endregion

        #endregion

        #region Properties



        #endregion
    }
}