﻿using System;
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
        public void SetValue_NullPropertyToSet_ReturnsTrue()
        {
            const string valueToAssign = "abc";
            string propertyToSet = null;
            bool isValueSet = EWPF.MVVM.BaseViewModel.SetValue(ref propertyToSet, valueToAssign);
            Assert.True(isValueSet);
        }

        [Test]
        public void SetValue_NullValueToAssign_ReturnsFalse()
        {
            string propertyToSet = null;
            bool isSetSuccessfully = EWPF.MVVM.BaseViewModel.SetValue(ref propertyToSet, null);

            Assert.False(isSetSuccessfully);
        }

        [Test]
        public void SetValue_EqualByVal_ReturnsFalse()
        {
            byte propertyToSet = 10;
            const byte valueToAssign = 10;
            bool isValueSet = EWPF.MVVM.BaseViewModel.SetValue(ref propertyToSet, valueToAssign);
            Assert.False(isValueSet);
        }

        [Test]
        public void SetValue_EqualByRef_ReturnsFalse()
        {
            var propertyToSet = new object();
            var valueToAssign = propertyToSet;
            bool isValueSet = EWPF.MVVM.BaseViewModel.SetValue(ref propertyToSet, valueToAssign);
            Assert.False(isValueSet);
        }

        [Test]
        public void SetValue_UnequalByValue_ReturnsTrue()
        {
            byte propertyToSet = 10;
            const byte valueToAssign = 5;
            bool isValueSet = EWPF.MVVM.BaseViewModel.SetValue(ref propertyToSet, valueToAssign);
            Assert.True(isValueSet);
        }

        [Test]
        public void SetValue_UnequalByValue_ChangesPassedReference()
        {
            byte propertyToSet = 10;
            const byte valueToAssign = 5;
            EWPF.MVVM.BaseViewModel.SetValue(ref propertyToSet, valueToAssign);
            Assert.ByVal(propertyToSet, new EqualConstraint(valueToAssign));
        }

        #endregion

        #region Set Collection Value

        [Test]
        public void SetCollectionValue_NullPropertyToSet_ReturnsTrue()
        {
            IEnumerable<byte> propertyToSet = null;
            var valueToAssign = new List<byte>();
            bool isValueSet =
                EWPF.MVVM.BaseViewModel.SetCollectionValue<IEnumerable<byte>, byte>(
                    ref propertyToSet, valueToAssign);
            Assert.True(isValueSet);
        }

        [Test]
        public void SetCollectionValue_NullValueToAssign_ReturnsFalse()
        {
            IEnumerable<byte> propertyToTest = null;
            bool isSetSuccessfully = EWPF.MVVM.BaseViewModel.SetCollectionValue<IEnumerable<byte>, byte>(
                ref propertyToTest, null);

            Assert.False(isSetSuccessfully);
        }

        [Test]
        public void SetValue_EqualCollectionsByVal_ReturnsFalse()
        {
            var propertyToSet = new List<byte> { 1, 2 };
            var valueToAssign = new List<byte> { 1, 2 };
            bool isValueSet = EWPF.MVVM.BaseViewModel.SetCollectionValue<List<byte>, byte>(ref propertyToSet, valueToAssign);
            Assert.False(isValueSet);
        }

        [Test]
        public void SetValue_UnequalCollectionByVal_ReturnsTrue()
        {
            var propertyToSet = new List<byte> { 1, 2 };
            var valueToAssign = new List<byte> { 1, 3, 5 };
            bool isValueSet = EWPF.MVVM.BaseViewModel.SetCollectionValue<List<byte>, byte>(ref propertyToSet, valueToAssign);
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
            Assert.Catch<ArgumentException>(() => testVM.IsPropertyNameValid("NonExistent", testVM));
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
            const string nestedPropertyName = cm_NESTED_TESTABLE_PROPERTY_NAME + "." +
                                              "testVM.NestedProperty.Property" +
                                              "." + "NonExistent";
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
            const string nestedPropertyName = "testVM.NestedProperty.NonExisting";
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
            const string testPropertyName = "TestProperty";
            const string nestedPropertyName = "NestedProperty";
            Assert.DoesNotThrow(() => testVM.OnPropertiesChanged(testVM, testPropertyName, nestedPropertyName));
        }

        [Test]
        public void OnPropertiesChanged_ExistingMultipleItemCollection_EnumerableVersion_NotFailing()
        {
            var testVM = MvvmFactory.MakeTestViewModel();
            var singleItemList = new List<string> { "TestProperty", "NestedProperty" };
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
            const string testPropertyName = "testVM.TestProperty";
            const string nonExistingPropertyName = "NonExisting";
            Assert.Catch<ArgumentException>(() => testVM.OnPropertiesChanged(testVM, testPropertyName, nonExistingPropertyName));
        }

        #endregion

        #endregion

        #region Properties



        #endregion
    }
}