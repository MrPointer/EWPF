using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows;
using EWPF.Dialogs;
using EWPF.MVVM.Services;
using EWPF.MVVM.ViewModel;
using EWPF.Utility;
using EWPF_Tests.Factory;
using NUnit.Framework;

namespace EWPF_Tests.Integration.Utility
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
        [Apartment(ApartmentState.STA)]
        [Category("UI")]
        public void ShowDialog_ValidDefaultDialog_ReturnsFalse()
        {
            const string cValidDialogName = "fakeDialog";
            var dialogResult = DialogUtility.ShowDialog(cValidDialogName);
            Assert.False(dialogResult);
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        [Category("UI")]
        public void ShowDialog_SingleParameterConstructorDialog_ReturnsFalse()
        {
            const string cDialogName = "parameterCtorDialog";
            var dataContext = new object();
            var dialogResult = DialogUtility.ShowDialog(cDialogName, null, dataContext);
            Assert.False(dialogResult);
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
                () => DialogUtility.ShowDialog(cExistingDialogName, i_Namespace: currentNamespaceName));
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
                () => DialogUtility.ShowDialog(cExistingDialogName, i_Namespace: currentNamespaceName));
            namespaceOptimizedStopwatch.Stop();

            var assemblyOptimizedStopwatch = new Stopwatch();
            string currentAssemblyName = Assembly.GetExecutingAssembly().FullName;
            assemblyOptimizedStopwatch.Start();
            // Catch an exception because the actual code can't be fully run in a unit test, 
            // requires STA threading model.
            Assert.Catch<TargetInvocationException>(
                () => DialogUtility.ShowDialog(cExistingDialogName, i_Namespace: currentNamespaceName, i_AssemblyName: currentAssemblyName));
            assemblyOptimizedStopwatch.Stop();

            // First, validate that namespace optimization is better, because even though there is a
            // separate test for this just a method above,
            // results may be different in each execution (highly unlikely)
            Assert.Less(namespaceOptimizedStopwatch.Elapsed, normalStopwatch.Elapsed);
            Assert.Less(assemblyOptimizedStopwatch.Elapsed, namespaceOptimizedStopwatch.Elapsed);
        }

        #endregion

        #region Show Indefinite Progress Dialog

        [Test]
        [Apartment(ApartmentState.STA)]
        [Category("UI")]
        public void ShowIndefiniteProgressDialog_ValidViewModel_DisplaysDialog()
        {
            var taskExecutor = TestExecutorFactory.CreateCancellableTaskExecutor();
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var progressAction = new Action<CancellationToken>(i_Token => { });
                var progressDialogVM =
                    new IndefiniteProgressDialogViewModel<bool>(taskExecutor, progressAction,
                        cancellationTokenSource);

                var dialogResult =
                    DialogUtility.ShowIndefiniteProgressDialog(progressDialogVM);

                Assert.True(dialogResult);
            }
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        [Category("UI")]
        public void
            ShowIndefiniteProgressDialog_ValidViewModelValidProgressAction_ExecutesAction()
        {
            var taskExecutor = TestExecutorFactory.CreateCancellableTaskExecutor();
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                int x = 0;
                var progressAction = new Action<CancellationToken>(i_Token => x = 5);
                var progressDialogVM =
                    new IndefiniteProgressDialogViewModel<bool>(taskExecutor, progressAction,
                        cancellationTokenSource);

                DialogUtility.ShowIndefiniteProgressDialog(progressDialogVM);

                Assert.AreEqual(5, x);
            }
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        [Category("UI")]
        public void ShowIndefiniteProgressDialog_ValidAction_Executes()
        {
            var taskExecutor = TestExecutorFactory.CreateCancellableTaskExecutor();
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                int x = 0;
                var progressAction = new Action<CancellationToken>(i_Token => x = 5);
                DialogUtility.ShowIndefiniteProgressDialog(taskExecutor,
                    cancellationTokenSource, progressAction);

                Assert.AreEqual(5, x);
            }
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        [Category("UI")]
        public void ShowIndefiniteProgressDialog_ValidFunction_Executes()
        {
            var taskExecutor = TestExecutorFactory.CreateCancellableTaskExecutor();
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                int x = 0;
                var progressFunction = new Func<CancellationToken, int>(i_Token =>
                 {
                     x = 5;
                     return x;
                 });
                DialogUtility.ShowIndefiniteProgressDialog(taskExecutor,
                    cancellationTokenSource, progressFunction);

                Assert.AreEqual(5, x);
            }
        }

        #endregion

        #endregion

        #region Properties



        #endregion
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

    [Dialog("parameterCtorDialog")]
    public class ParameterConstructorDialog : Window
    {
        /// <inheritdoc />
        public ParameterConstructorDialog(object i_DataContext)
        {
            DataContext = i_DataContext;
        }
    }
}