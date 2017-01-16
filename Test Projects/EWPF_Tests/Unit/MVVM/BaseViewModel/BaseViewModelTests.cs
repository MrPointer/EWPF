using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace EWPF_Tests.Unit.MVVM.BaseViewModel
{
    [TestFixture]
    public class BaseViewModelTests
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

        #region Set Value

        [Test]
        public void SetValue_NullProperty_ReturnsTrue()
        {
            var testVM = MvvmFactory.MakeTestViewModel();
            byte? valueToAssign = 2;
            byte? propertyToSet = null;
            bool isValueSet = testVM.SetValue(ref propertyToSet, valueToAssign);
            Assert.True(isValueSet);
        }

        [Test]
        public void SetValue_NullValue_ThrowsArgumentNullException()
        {
            var testVM = MvvmFactory.MakeTestViewModel();
            byte? propertyToSet = 0;
            byte? valueToAssign = null;
            Assert.Catch<ArgumentNullException>(() => testVM.SetValue(ref propertyToSet, valueToAssign));
        }

        [Test]
        public void SetValue_EqualByVal_ReturnsFalse()
        {
            var testVM = MvvmFactory.MakeTestViewModel();
            byte propertyToSet = 10;
            const byte valueToAssign = 10;
            bool isValueSet = testVM.SetValue(ref propertyToSet, valueToAssign);
            Assert.False(isValueSet);
        }

        [Test]
        public void SetValue_EqualByRef_ReturnsFalse()
        {
            var testVM = MvvmFactory.MakeTestViewModel();
            var propertyToSet = new object();
            var valueToAssign = propertyToSet;
            bool isValueSet = testVM.SetValue(ref propertyToSet, valueToAssign);
            Assert.False(isValueSet);
        }

        [Test]
        public void SetValue_UnequalByValue_ReturnsTrue()
        {
            var testVM = MvvmFactory.MakeTestViewModel();
            byte propertyToSet = 10;
            const byte valueToAssign = 5;
            bool isValueSet = testVM.SetValue(ref propertyToSet, valueToAssign);
            Assert.True(isValueSet);
        }

        [Test]
        public void SetValue_UnequalByValue_ChangesPassedReference()
        {
            var testVM = MvvmFactory.MakeTestViewModel();
            byte propertyToSet = 10;
            const byte valueToAssign = 5;
            testVM.SetValue(ref propertyToSet, valueToAssign);
            Assert.ByVal(propertyToSet, new EqualConstraint(valueToAssign));
        }

        #endregion

        #region Set Collection Value

        [Test]
        public void SetValue_NullValue_ThrowsNullArgumentException()
        {
            var testVM = MvvmFactory.MakeTestViewModel();
            IEnumerable<byte> propertyToTest = null;
            IEnumerable<byte> valueToAssign = null;
            Assert.Catch<ArgumentNullException>(() =>
            testVM.SetCollectionValue<IEnumerable<byte>, byte>(ref propertyToTest, valueToAssign));
        }

        [Test]
        public void SetValue_EqualCollectionsByVal_ReturnsFalse()
        {
            var testVM = MvvmFactory.MakeTestViewModel();
            var propertyToSet = new List<byte> { 1, 2 };
            var valueToAssign = new List<byte> { 1, 2 };
            bool isValueSet = testVM.SetCollectionValue<List<byte>, byte>(ref propertyToSet, valueToAssign);
            Assert.False(isValueSet);
        }

        [Test]
        public void SetValue_UnequalCollectionByVal_ReturnsTrue()
        {
            var testVM = MvvmFactory.MakeTestViewModel();
            var propertyToSet = new List<byte> { 1, 2 };
            var valueToAssign = new List<byte> { 1, 3, 5 };
            bool isValueSet = testVM.SetCollectionValue<List<byte>, byte>(ref propertyToSet, valueToAssign);
            Assert.True(isValueSet);
        }

        #endregion

        #region Is Property Name Value

        [Test]
        public void IsPropertyNameValid_NullInvoker_ThrowsArgumentNullException()
        {
            var testVM = MvvmFactory.MakeTestViewModel();
            Assert.Catch<ArgumentNullException>(() => testVM.IsPropertyNameValid(cm_TESTABLE_PROPERTY_NAME, null));
        }

        [Test]
        public void IsPropertyNameValid_NullOrEmptyName_ThrowsArgumentExcpetion()
        {
            var testVM = MvvmFactory.MakeTestViewModel();
            Assert.Catch<ArgumentException>(() => testVM.IsPropertyNameValid(null, testVM));
            Assert.Catch<ArgumentException>(() => testVM.IsPropertyNameValid(string.Empty, testVM));
        }

        [Test]
        public void IsPropertyNameValid_DirectExistingProperty_ReturnsTrue()
        {
            var testVM = MvvmFactory.MakeTestViewModel();
            bool isVerified = testVM.IsPropertyNameValid(cm_TESTABLE_PROPERTY_NAME, testVM);
            Assert.True(isVerified);
        }

        [Test]
        public void IsPropertyNameValid_DirectNonExistingProperty_ReturnsFalse()
        {
            var testVM = MvvmFactory.MakeTestViewModel();
            bool isVerified = testVM.IsPropertyNameValid("NonExistent", testVM);
            Assert.False(isVerified);
        }

        [Test]
        public void IsPropertyNameValid_DirectNonExistingProperty_ThrowRequested_ThrowsArgumentExcpetion()
        {
            var testVM = MvvmFactory.MakeTestViewModel();
            testVM.ThrowOnInvalidPropertyName = true;
            Assert.Catch<ArgumentException>(() => testVM.IsPropertyNameValid("NonExistant", testVM));
        }

        [Test]
        public void IsPropertyNameValid_IndirectExistingProperty_ReturnsTrue()
        {
            var testVM = MvvmFactory.MakeTestViewModel();
            const string nestedPropertyName = cm_NESTED_TESTABLE_PROPERTY_NAME + "." + "Property";
            bool isValid = testVM.IsPropertyNameValid(nestedPropertyName, testVM);
            Assert.True(isValid);
        }

        [Test]
        public void IsPropertyNameValid_DeepIndirectNonExistingProperty_ReturnsFalse()
        {
            var testVM = MvvmFactory.MakeTestViewModel();
            const string nestedPropertyName = cm_NESTED_TESTABLE_PROPERTY_NAME + "." + nameof(testVM.NestedProperty.Property) +
                "." + "NonExistant";
            bool isValid = testVM.IsPropertyNameValid(nestedPropertyName, testVM);
            Assert.False(isValid);
        }

        [Test]
        public void IsPropertyNameValid_IndirectNonExistingProperty_ReturnsFalse()
        {
            var testVM = MvvmFactory.MakeTestViewModel();
            const string nestedPropertyName = cm_NESTED_TESTABLE_PROPERTY_NAME + "." + "NonExisting";
            bool isValid = testVM.IsPropertyNameValid(nestedPropertyName, testVM);
            Assert.False(isValid);
        }

        [Test]
        public void IsPropertyNameValid_IndirectNonExistingProperty_ThrowRequested_ThrowsArgumentExcpetion()
        {
            var testVM = MvvmFactory.MakeTestViewModel();
            testVM.ThrowOnInvalidPropertyName = true;
            const string nestedPropertyName = nameof(testVM.NestedProperty) + "." + "NonExisting";
            Assert.Catch<ArgumentException>(() => testVM.IsPropertyNameValid(nestedPropertyName, testVM));
        }

        #endregion

        #region On Property Changed

        [Test]
        public void OnPropertyChanged_NullInvoker_NotFailing()
        {
            var testVM = MvvmFactory.MakeTestViewModel();
            Assert.DoesNotThrow(() => testVM.OnPropertyChanged(cm_TESTABLE_PROPERTY_NAME, null));
        }

        [Test]
        public void OnPropertyChanged_DifferentValue_ChangesState()
        {
            var testVM = MvvmFactory.MakeTestViewModel();
            testVM.TestProperty = "Hello Unit Test";
            testVM.TestProperty = "New Test";
            Assert.True(testVM.HasPropertyChanged);
        }

        [Test]
        public void OnPropertyChanged_SameValue_DoesntChangeState()
        {
            var testVM = MvvmFactory.MakeTestViewModel();
            testVM.TestProperty = "Hello Unit Test";
            testVM.TestProperty = "Hello Unit Test";
            Assert.False(testVM.HasPropertyChanged);
        }

        #endregion

        #region On Properties Changed

        [Test]
        public void OnPropertiesChanged_NullCollection_ThrowsArgumentNullException()
        {
            var testVM = MvvmFactory.MakeTestViewModel();
            Assert.Catch<ArgumentNullException>(() => testVM.OnPropertiesChanged(null, testVM));
            Assert.Catch<ArgumentNullException>(() => testVM.OnPropertiesChanged(testVM, null));
        }

        [Test]
        public void OnPropertiesChanged_NullInvoker_ParamsVersion_ThrowsArgumentNullException()
        {
            var testVM = MvvmFactory.MakeTestViewModel();
            Assert.Catch<ArgumentNullException>(() => testVM.OnPropertiesChanged(null, string.Empty));
        }

        [Test]
        public void OnPropertiesChanged_NullInvoker_EnumerableVersion_NotFailing()
        {
            var testVM = MvvmFactory.MakeTestViewModel();
            Assert.DoesNotThrow(() => testVM.OnPropertiesChanged(new List<string>(), null));
        }

        [Test]
        public void OnPropertiesChanged_ExistingMultipleItemCollection_ParamsVersion_NotFailing()
        {
            var testVM = MvvmFactory.MakeTestViewModel();
            const string testPropertyName = nameof(testVM.TestProperty);
            const string nestedPropertyName = nameof(testVM.NestedProperty);
            Assert.DoesNotThrow(() => testVM.OnPropertiesChanged(testVM, testPropertyName, nestedPropertyName));
        }

        [Test]
        public void OnPropertiesChanged_ExistingMultipleItemCollection_EnumerableVersion_NotFailing()
        {
            var testVM = MvvmFactory.MakeTestViewModel();
            var singleItemList = new List<string> { nameof(testVM.TestProperty), nameof(testVM.NestedProperty) };
            Assert.DoesNotThrow(() => testVM.OnPropertiesChanged(singleItemList, testVM));
        }

        #endregion

        #region On Many Properties Changed

        [Test]
        public void OnManyPropertiesChanged_NullCollection_ThrowsArgumentNullException()
        {
            var testVM = MvvmFactory.MakeTestViewModel();
            Assert.Catch<ArgumentNullException>(() => testVM.OnManyPropertiesChanged(null, testVM));
        }

        [Test]
        public void OnManyPropertiesChanged_NullInvoker_ThrowsArgumentNullException()
        {
            var testVM = MvvmFactory.MakeTestViewModel();
            Assert.Catch<ArgumentNullException>(() => testVM.OnManyPropertiesChanged(new List<string>(), null));
        }

        [Test]
        public void OnManyPropertiesChanged_SingleExistingItemCollection_NotFailing()
        {
            var testVM = MvvmFactory.MakeTestViewModel();
            var propertyNames = new List<string> { cm_TESTABLE_PROPERTY_NAME };
            Assert.DoesNotThrow(() => testVM.OnManyPropertiesChanged(propertyNames, testVM));
        }

        [Test]
        public void OnManyPropertiesChanged_SingleNonExistingItemCollection_ThrowsArgumentException()
        {
            var testVM = MvvmFactory.MakeTestViewModel();
            var propertyNames = new List<string> { "NonExisting" };
            Assert.Catch<ArgumentException>(() => testVM.OnManyPropertiesChanged(propertyNames, testVM));
        }

        [Test]
        public void OnManyPropertiesChanged_SomeExistingSomeNotCollection_ThrowsArgumentException()
        {
            var testVM = MvvmFactory.MakeTestViewModel();
            const string testPropertyName = nameof(testVM.TestProperty);
            const string nonExistingPropertyName = "NonExisting";
            Assert.Catch<ArgumentException>(() => testVM.OnPropertiesChanged(testVM, testPropertyName, nonExistingPropertyName));
        }

        #endregion

        #endregion

        #region Properties



        #endregion
    }
}