using System.Diagnostics;
using System.Reflection;
using System.Threading;
using EWPF.Utility;
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

        #region Properties



        #endregion
    }
}