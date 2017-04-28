﻿using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using EWPF.MVVM.Services;
using EWPF.Utility;
using NUnit.Framework;

namespace EWPF_Tests.Unit.Utility
{
    [TestFixture]
    public class DialogUtilityTests
    {
        #region Events



        #endregion

        #region Fields



        #endregion

        #region Constructors



        #endregion

        #region Methods

        #region Show Dialog

        [Test]
        public void ShowDialog_NullDialogName_ThrowsArgumentNullException()
        {
            Assert.Catch<ArgumentNullException>(() => DialogUtility.ShowDialog(null));
        }

        [Test]
        public void ShowDialog_EmptyOrWhitespaceDialogName_ThrowsArgumentException()
        {
            var caughtException = Assert.Catch<ArgumentException>(
                () => DialogUtility.ShowDialog(string.Empty));
            StringAssert.Contains("can't be empty", caughtException.Message);

            caughtException = Assert.Catch<ArgumentException>(() => DialogUtility.ShowDialog(" "));
            StringAssert.Contains("contain only whitespaces", caughtException.Message);
        }

        [Test]
        public void ShowDialog_EmptyOrWhitespaceNamespace_ThrowsArgumentException()
        {
            const string cDialogName = "abc";
            var caughtException = Assert.Catch<ArgumentException>(
                () => DialogUtility.ShowDialog(cDialogName, string.Empty));
            StringAssert.Contains("can't be empty", caughtException.Message);

            caughtException = Assert.Catch<ArgumentException>(
                () => DialogUtility.ShowDialog(cDialogName, " "));
            StringAssert.Contains("contain only whitespaces", caughtException.Message);
        }

        [Test]
        public void ShowDialog_EmptyOrWhitespaceAssemblyName_ThrowsArgumentException()
        {
            const string cDialogName = "abc";
            const string cNamespace = "def";
            var caughtException = Assert.Catch<ArgumentException>(
                () => DialogUtility.ShowDialog(cDialogName, cNamespace,string.Empty));
            StringAssert.Contains("can't be empty", caughtException.Message);

            caughtException = Assert.Catch<ArgumentException>(
                () => DialogUtility.ShowDialog(cDialogName, cNamespace," "));
            StringAssert.Contains("contain only whitespaces", caughtException.Message);
        }

        [Test]
        public void ShowDialog_NonExistingDialogName_ThrowsArgumentException()
        {
            const string cNonExistingDialogName = "abc";
            Assert.Catch<ArgumentException>(() => DialogUtility.ShowDialog(cNonExistingDialogName));
        }

        [Test]
        public void ShowDialog_ExistingDialogName_NotWindowType_ThrowsInvalidCastException()
        {
            const string cExistingDialogName = "fakeDialogNonWindow";
            var caughtException =
                Assert.Catch<InvalidCastException>(() => DialogUtility.ShowDialog(cExistingDialogName));
            StringAssert.Contains("to a Window object", caughtException.Message);
        }

        [Test]
        [Category("Long")]
        public void ShowDialog_ExistingDialog_SpecificNamespace_ExecutesFaster()
        {
            var normalStopwatch = new Stopwatch();
            const string cExistingDialogName = "fakeDialog";
            normalStopwatch.Start();
            // Catch an exception because the actual code can't be fully run in a unit test, 
            // requires STA threading model.
            Assert.Catch<TargetInvocationException>(() => DialogUtility.ShowDialog(cExistingDialogName));
            normalStopwatch.Stop();

            var namespaceOptimizedStopwatch = new Stopwatch();
            string currentNamespaceName = GetType().Namespace;
            namespaceOptimizedStopwatch.Start();
            // Catch an exception because the actual code can't be fully run in a unit test, 
            // requires STA threading model.
            Assert.Catch<TargetInvocationException>(
                () => DialogUtility.ShowDialog(cExistingDialogName, currentNamespaceName));
            namespaceOptimizedStopwatch.Stop();

            Assert.Less(namespaceOptimizedStopwatch.Elapsed, normalStopwatch.Elapsed);
        }

        [Test]
        [Category("Long")]
        public void ShowDialog_ExistingDialog_SpecificNamespace_SpecificAssembly_ExecutesEvenFaster()
        {
            var normalStopwatch = new Stopwatch();
            const string cExistingDialogName = "fakeDialog";
            normalStopwatch.Start();
            // Catch an exception because the actual code can't be fully run in a unit test, 
            // requires STA threading model.
            Assert.Catch<TargetInvocationException>(() => DialogUtility.ShowDialog(cExistingDialogName));
            normalStopwatch.Stop();

            var namespaceOptimizedStopwatch = new Stopwatch();
            string currentNamespaceName = GetType().Namespace;
            namespaceOptimizedStopwatch.Start();
            // Catch an exception because the actual code can't be fully run in a unit test, 
            // requires STA threading model.
            Assert.Catch<TargetInvocationException>(
                () => DialogUtility.ShowDialog(cExistingDialogName, currentNamespaceName));
            namespaceOptimizedStopwatch.Stop();

            var assemblyOptimizedStopwatch = new Stopwatch();
            string currentAssemblyName = Assembly.GetExecutingAssembly().FullName;
            assemblyOptimizedStopwatch.Start();
            // Catch an exception because the actual code can't be fully run in a unit test, 
            // requires STA threading model.
            Assert.Catch<TargetInvocationException>(
                () => DialogUtility.ShowDialog(cExistingDialogName, currentNamespaceName, currentAssemblyName));
            assemblyOptimizedStopwatch.Stop();

            // First, validate that namespace optimization is better, because even though there is a
            // separate test for this just a method above,
            // results may be different in each execution (highly unlikely)
            Assert.Less(namespaceOptimizedStopwatch.Elapsed, normalStopwatch.Elapsed);
            Assert.Less(assemblyOptimizedStopwatch.Elapsed, namespaceOptimizedStopwatch.Elapsed);
        }

        #endregion

        #endregion

        #region Properties



        #endregion
    }

    /// <summary>
    /// A class representing a fake dialog which doesn't inherits the <see cref="Window"/> type, 
    /// but registers the <see cref="DialogAttribute"/> to mark itself a dialog. <br />
    /// This class is used only for testing purposes and is declared explicitly instead of using 'Moq' 
    /// because it solves some issues regarding assembly sources.
    /// </summary>
    [Dialog("fakeDialogNonWindow")]
    public class FakeDialogNonWindow
    {

    }

    /// <summary>
    /// A class representing a fake dialog which inherits the <see cref="Window"/> type and also 
    /// registers itself as a dialog with the <see cref="DialogAttribute"/>. <br />
    /// This class is used only for testing purposes and is declared explicitly instead of using 'Moq' 
    /// because it solves some issues regarding assembly sources.
    /// </summary>
    [Dialog("fakeDialog")]
    public class FakeDialog : Window
    {

    }
}