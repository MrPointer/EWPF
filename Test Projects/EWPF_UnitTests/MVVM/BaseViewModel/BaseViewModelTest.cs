using System;
using System.Collections.Generic;
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
        private const string cm_NESTED_TESTABLE_PROPERTY_NAME = "NestedProperty";

        #endregion

        #region Constructors



        #endregion

        #region Methods

        #region Verify Property Name

        [Test]
        public void VerifyPropertyName_NullInvoker_ThrowsArgumentNullException()
        {
            var testVM = MakeTestViewModel();
            Assert.Catch<ArgumentNullException>(() => testVM.IsPropertyNameValid(cm_TESTABLE_PROPERTY_NAME, null));
        }

        [Test]
        public void VerifyPropertyName_NullOrEmptyName_ThrowsArgumentExcpetion()
        {
            var testVM = MakeTestViewModel();
            Assert.Catch<ArgumentException>(() => testVM.IsPropertyNameValid(null, testVM));
            Assert.Catch<ArgumentException>(() => testVM.IsPropertyNameValid(string.Empty, testVM));
        }

        [Test]
        public void VerifyPropertyName_DirectExistingProperty_ReturnsTrue()
        {
            var testVM = MakeTestViewModel();
            bool isVerified = testVM.IsPropertyNameValid(cm_TESTABLE_PROPERTY_NAME, testVM);
            Assert.True(isVerified);
        }

        [Test]
        public void VerifyPropertyName_DirectNonExistingProperty_ReturnsFalse()
        {
            var testVM = MakeTestViewModel();
            bool isVerified = testVM.IsPropertyNameValid("NonExistent", testVM);
            Assert.False(isVerified);
        }

        [Test]
        public void VerifyPropertyName_IndirectExistingProperty_ReturnsTrue()
        {
            var testVM = MakeTestViewModel();
            const string nestedPropertyName = cm_NESTED_TESTABLE_PROPERTY_NAME + "." + "Property";
            bool isValid = testVM.IsPropertyNameValid(nestedPropertyName, testVM);
            Assert.True(isValid);
        }

        [Test]
        public void VerifyPropertyName_IndirectNonExistingProperty_ReturnsFalse()
        {
            var testVM = MakeTestViewModel();
            const string nestedPropertyName = cm_NESTED_TESTABLE_PROPERTY_NAME + "." + "NonExisting";
            bool isValid = testVM.IsPropertyNameValid(nestedPropertyName, testVM);
            Assert.False(isValid);
        }

        #endregion

        #region On Property Changed

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

        #endregion

        #region On Properties Changed

        [Test]
        public void OnPropertiesChanged_NullCollection_ThrowsArgumentNullException()
        {
            var testVM = MakeTestViewModel();
            Assert.Catch<ArgumentNullException>(() => testVM.OnPropertiesChanged(null, testVM));
            Assert.Catch<ArgumentNullException>(() => testVM.OnPropertiesChanged(testVM, null));
        }

        [Test]
        public void OnPropertiesChanged_NullInvoker_ParamsVersion_ThrowsArgumentNullException()
        {
            var testVM = MakeTestViewModel();
            Assert.Catch<ArgumentNullException>(() => testVM.OnPropertiesChanged(null, string.Empty));
        }

        [Test]
        public void OnPropertiesChanged_NullInvoker_EnumerableVersion_NotFailing()
        {
            var testVM = MakeTestViewModel();
            Assert.DoesNotThrow(() => testVM.OnPropertiesChanged(new List<string>(), null));
        }

        #endregion

        #region On Many Properties Changed

        [Test]
        public void OnManyPropertiesChanged_NullCollection_ThrowsArgumentNullException()
        {
            var testVM = MakeTestViewModel();
            Assert.Catch<ArgumentNullException>(() => testVM.OnManyPropertiesChanged(null, testVM));
        }

        [Test]
        public void OnManyPropertiesChanged_NullInvoker_ThrowsArgumentNullException()
        {
            var testVM = MakeTestViewModel();
            Assert.Catch<ArgumentNullException>(() => testVM.OnManyPropertiesChanged(new List<string>(), null));
        }

        [Test]
        public void OnManyPropertiesChanged_ExistingSingleItemCollection_NotFailing()
        {
            var testVM = MakeTestViewModel();
            var propertyNames = new List<string> { cm_TESTABLE_PROPERTY_NAME };
            Assert.DoesNotThrow(() => testVM.OnManyPropertiesChanged(propertyNames, testVM));
        }

        [Test]
        public void OnManyPropertiesChanged_NonExistingSingleItemCollection_NotFailing()
        {
            var testVM = MakeTestViewModel();
            var propertyNames = new List<string> { "NonExisting" };
            Assert.DoesNotThrow(() => testVM.OnManyPropertiesChanged(propertyNames, testVM));
        }

        #endregion

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