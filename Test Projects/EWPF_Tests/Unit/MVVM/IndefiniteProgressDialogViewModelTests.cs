using System;
using System.Threading;
using System.Threading.Tasks;
using EWPF.MVVM.Services;
using EWPF.MVVM.ViewModel;
using EWPF_Tests.Factory;
using KISCore.Execution;
using Moq;
using NUnit.Framework;

namespace EWPF_Tests.Unit.MVVM
{
    [TestFixture]
    public class IndefiniteProgressDialogViewModelTests
    {
        #region Events



        #endregion

        #region Fields



        #endregion

        #region Constructors



        #endregion

        #region Methods

        #region Constructor

        [Test]
        public void ConstructorAction_NullExecutor_ThrowsArgumentNullException()
        {
            Assert.Catch<ArgumentNullException>(() =>
                new IndefiniteProgressDialogViewModel<bool>(null, null,
                    null,
                    i_ActionCompletionCallback: null, i_CancellationCallback: null));
        }

        [Test]
        public void ConstructorFunction_NullExecutor_ThrowsArgumentNullException()
        {
            Assert.Catch<ArgumentNullException>(() =>
                new IndefiniteProgressDialogViewModel<bool>(null, null,
                    null,
                    i_FunctionCompletionCallback: null, i_CancellationCallback: null));
        }

        [Test]
        public void
            ConstructorAction_NullCancellationTokenSource_ThrowsArgumentNullException()
        {
            var fakeTaskExecutor = new Mock<ICancellableTaskExecutor<CancellationToken>>();
            Assert.Catch<ArgumentNullException>(() =>
                new IndefiniteProgressDialogViewModel<bool>(fakeTaskExecutor.Object,
                    null, null,
                    i_ActionCompletionCallback: null, i_CancellationCallback: null));
        }

        [Test]
        public void
            ConstructorFunction_NullCancellationTokenSource_ThrowsArgumentNullException()
        {
            var fakeTaskExecutor = new Mock<ICancellableTaskExecutor<CancellationToken>>();
            Assert.Catch<ArgumentNullException>(() =>
                new IndefiniteProgressDialogViewModel<bool>(fakeTaskExecutor.Object,
                    null, null,
                    i_FunctionCompletionCallback: null, i_CancellationCallback: null));
        }

        #endregion

        #region Do Progress

        [Test]
        public void DoProgress_NullWindowService_ThrowsNullReferenceException()
        {
            var fakeExecutor = new Mock<ICancellableTaskExecutor<CancellationToken>>();
            var cancellationTokenSource = new CancellationTokenSource();
            var progressDialogVM = new IndefiniteProgressDialogViewModel<bool>(
                fakeExecutor.Object, i_ProgressAction: null,
                i_CancellationTokenSource: cancellationTokenSource);

            var caughtException =
                Assert.CatchAsync<NullReferenceException>(() =>
                    progressDialogVM.DoProgress());
            StringAssert.Contains("must be set", caughtException.Message);
        }

        [Test]
        public void DoProgress_NullActionNullFunction_ThrowsNullReferenceException()
        {
            var fakeExecutor = new Mock<ICancellableTaskExecutor<CancellationToken>>();
            var fakeWindowService = new Mock<IWindowService>();
            var cancellationTokenSource = new CancellationTokenSource();
            var progressDialogVM = new IndefiniteProgressDialogViewModel<bool>(
                fakeExecutor.Object, i_ProgressAction: null,
                i_CancellationTokenSource: cancellationTokenSource)
            {
                WindowService = fakeWindowService.Object
            };

            var caughtException =
                Assert.CatchAsync<NullReferenceException>(() =>
                    progressDialogVM.DoProgress());
            StringAssert.Contains("Progress action or function must be set",
                caughtException.Message);
        }

        [Test]
        public async Task DoProgress_ActionProgress_Executes()
        {
            var fakeExecutor = new Mock<ICancellableTaskExecutor<CancellationToken>>();
            var fakeWindowService = new Mock<IWindowService>();
            var cancellationTokenSource = new CancellationTokenSource();

            var fakeAction = new Action<CancellationToken>(i_Token => { });
            fakeExecutor.Setup(i_Executor =>
                    i_Executor.Execute(fakeAction, cancellationTokenSource.Token))
                .Returns(Task.Delay(0)).Verifiable();

            var progressDialogVM = new IndefiniteProgressDialogViewModel<bool>(
                fakeExecutor.Object, fakeAction,
                cancellationTokenSource)
            {
                WindowService = fakeWindowService.Object
            };

            await progressDialogVM.DoProgress();
            fakeExecutor.Verify();
        }

        [Test]
        public async Task DoProgress_FunctionProgress_Executes()
        {
            var fakeExecutor = new Mock<ICancellableTaskExecutor<CancellationToken>>();
            var fakeWindowService = new Mock<IWindowService>();
            var cancellationTokenSource = new CancellationTokenSource();

            var fakeFunction = new Func<CancellationToken, bool>(i_Token => true);
            fakeExecutor.Setup(i_Executor =>
                    i_Executor.Execute(fakeFunction, cancellationTokenSource.Token))
                .Returns(Task.FromResult(true)).Verifiable();

            var progressDialogVM = new IndefiniteProgressDialogViewModel<bool>(
                fakeExecutor.Object, fakeFunction,
                cancellationTokenSource)
            {
                WindowService = fakeWindowService.Object
            };

            await progressDialogVM.DoProgress();
            fakeExecutor.Verify();
        }

        [Test]
        public void DoProgress_NullActionCompletionCallback_DoesNotThrow()
        {
            var fakeExecutor = new Mock<ICancellableTaskExecutor<CancellationToken>>();
            var fakeWindowService = new Mock<IWindowService>();
            var cancellationTokenSource = new CancellationTokenSource();

            var fakeAction = new Action<CancellationToken>(i_Token => { });
            fakeExecutor.Setup(i_Executor =>
                    i_Executor.Execute(fakeAction, cancellationTokenSource.Token))
                .Returns(Task.Delay(0));

            var progressDialogVM = new IndefiniteProgressDialogViewModel<bool>(
                fakeExecutor.Object, fakeAction,
                cancellationTokenSource)
            {
                WindowService = fakeWindowService.Object
            };

            Assert.DoesNotThrowAsync(async () => await progressDialogVM.DoProgress());
        }

        [Test]
        public void DoProgress_NullFunctionCompletionCallback_DoesNotThrow()
        {
            var fakeExecutor = new Mock<ICancellableTaskExecutor<CancellationToken>>();
            var fakeWindowService = new Mock<IWindowService>();
            var cancellationTokenSource = new CancellationTokenSource();

            var fakeFunction = new Func<CancellationToken, bool>(i_Token => true);
            fakeExecutor.Setup(i_Executor =>
                    i_Executor.Execute(fakeFunction, cancellationTokenSource.Token))
                .Returns(Task.FromResult(true)).Verifiable();

            var progressDialogVM = new IndefiniteProgressDialogViewModel<bool>(
                fakeExecutor.Object, fakeFunction,
                cancellationTokenSource)
            {
                WindowService = fakeWindowService.Object
            };

            Assert.DoesNotThrowAsync(async () => await progressDialogVM.DoProgress());
        }

        [Test]
        public async Task DoProgress_ValidActionCompletionCallback_IsCalled()
        {
            var fakeExecutor = new Mock<ICancellableTaskExecutor<CancellationToken>>();
            var fakeWindowService = new Mock<IWindowService>();
            var cancellationTokenSource = new CancellationTokenSource();

            var fakeAction = new Action<CancellationToken>(i_Token => { });
            fakeExecutor.Setup(i_Executor =>
                    i_Executor.Execute(fakeAction, cancellationTokenSource.Token))
                .Returns(Task.Delay(0));
            int x = 0;
            var completionCallback = new Action(() => x = 5);

            var progressDialogVM = new IndefiniteProgressDialogViewModel<bool>(
                fakeExecutor.Object, fakeAction,
                cancellationTokenSource, completionCallback)
            {
                WindowService = fakeWindowService.Object
            };

            await progressDialogVM.DoProgress();

            Assert.AreEqual(5, x);
        }

        [Test]
        public async Task DoProgress_ValidFunctionCompletionCallback_IsCalled()
        {
            var fakeExecutor = new Mock<ICancellableTaskExecutor<CancellationToken>>();
            var fakeWindowService = new Mock<IWindowService>();
            var cancellationTokenSource = new CancellationTokenSource();

            var fakeFunction = new Func<CancellationToken, bool>(i_Token => true);
            fakeExecutor.Setup(i_Executor =>
                    i_Executor.Execute(fakeFunction, cancellationTokenSource.Token))
                .Returns(Task.FromResult(true)).Verifiable();
            bool flag = false;
            var completionCallback = new Action<bool>(i_B => flag = i_B);

            var progressDialogVM = new IndefiniteProgressDialogViewModel<bool>(
                fakeExecutor.Object, fakeFunction,
                cancellationTokenSource, completionCallback)
            {
                WindowService = fakeWindowService.Object
            };

            await progressDialogVM.DoProgress();
            Assert.True(flag);
        }

        [Test]
        public void DoProgress_ExecutionCanceled_NullCancellationCallback_DoesNotThrow()
        {
            var fakeExecutor = new Mock<ICancellableTaskExecutor<CancellationToken>>();
            var fakeWindowService = new Mock<IWindowService>();
            var cancellationTokenSource = new CancellationTokenSource();

            var fakeAction = new Action<CancellationToken>(i_Token => { });
            fakeExecutor.Setup(i_Executor =>
                    i_Executor.Execute(fakeAction, cancellationTokenSource.Token))
                .Throws(new AggregateException(new TaskCanceledException()));

            var progressDialogVM = new IndefiniteProgressDialogViewModel<bool>(
                fakeExecutor.Object, fakeAction,
                cancellationTokenSource)
            {
                WindowService = fakeWindowService.Object
            };

            Assert.DoesNotThrowAsync(async () => await progressDialogVM.DoProgress());
        }

        [Test]
        public async Task DoProgress_ExecutionCanceled_ValidCancellationCallback_IsCalled()
        {
            var fakeExecutor = new Mock<ICancellableTaskExecutor<CancellationToken>>();
            var fakeWindowService = new Mock<IWindowService>();
            var cancellationTokenSource = new CancellationTokenSource();

            var fakeAction = new Action<CancellationToken>(i_Token => { });
            fakeExecutor.Setup(i_Executor =>
                    i_Executor.Execute(fakeAction, cancellationTokenSource.Token))
                .Throws(new AggregateException(new TaskCanceledException()));
            int x = 0;
            var cancellationCallback = new Action(() => x = 5);

            var progressDialogVM = new IndefiniteProgressDialogViewModel<bool>(
                fakeExecutor.Object, fakeAction,
                cancellationTokenSource, null, cancellationCallback)
            {
                WindowService = fakeWindowService.Object
            };

            await progressDialogVM.DoProgress();
            Assert.AreEqual(5, x);
        }

        [Test]
        public void DoProgress_ExecutionThrowsException_ExceptionHandledInternally()
        {
            var fakeExecutor = new Mock<ICancellableTaskExecutor<CancellationToken>>();
            var fakeWindowService = new Mock<IWindowService>();
            var cancellationTokenSource = new CancellationTokenSource();

            var fakeAction = new Action<CancellationToken>(i_Token => { });
            fakeExecutor.Setup(i_Executor =>
                    i_Executor.Execute(fakeAction, cancellationTokenSource.Token))
                .Throws<Exception>();

            var progressDialogVM = new IndefiniteProgressDialogViewModel<bool>(
                fakeExecutor.Object, fakeAction,
                cancellationTokenSource)
            {
                WindowService = fakeWindowService.Object
            };

            Assert.DoesNotThrowAsync(async () => await progressDialogVM.DoProgress());
        }

        [Test]
        public async Task DoProgress_ExecutionThrowsException_WindowRequestedToClose()
        {
            var fakeExecutor = new Mock<ICancellableTaskExecutor<CancellationToken>>();
            var fakeWindowService = new Mock<IWindowService>();
            var cancellationTokenSource = new CancellationTokenSource();

            var fakeAction = new Action<CancellationToken>(i_Token => { });
            fakeExecutor.Setup(i_Executor =>
                    i_Executor.Execute(fakeAction, cancellationTokenSource.Token))
                .Throws<Exception>();
            fakeWindowService.Setup(i_Service => i_Service.CloseWindow(false)).Verifiable();

            var progressDialogVM = new IndefiniteProgressDialogViewModel<bool>(
                fakeExecutor.Object, fakeAction,
                cancellationTokenSource)
            {
                WindowService = fakeWindowService.Object
            };

            await progressDialogVM.DoProgress();
            fakeWindowService.Verify(i_Service => i_Service.CloseWindow(false));
        }

        #endregion        

        #endregion

        #region Properties



        #endregion
    }
}