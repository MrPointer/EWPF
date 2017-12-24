using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using EWPF.Dialogs;
using EWPF.MVVM.Services;
using EWPF.MVVM.ViewModel;
using EWPF_Tests.Factory;
using Moq;
using NUnit.Framework;

namespace EWPF_Tests.Integration.MVVM
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

        #region Do Progress

        [Test]
        public void DoProgress_ValidProgressAction_Executes()
        {
            var taskExecutor = TestExecutorFactory.CreateCancellableTaskExecutor();
            var fakeWindowService = new Mock<IWindowService>();
            var cancellationTokenSource = new CancellationTokenSource();

            int x = 0;
            var progressAction = new Action<CancellationToken>(i_Token => x = 5);

            var progressDialogVM = new IndefiniteProgressDialogViewModel<bool>(
                taskExecutor, progressAction,
                cancellationTokenSource)
            {
                WindowService = fakeWindowService.Object
            };

            var progressTask = progressDialogVM.DoProgress();
            progressTask.Wait();

            Assert.True(progressTask.IsCompleted);
            Assert.AreEqual(5, x);

            cancellationTokenSource.Dispose();
        }

        [Test]
        public void DoProgress_ValidProgressAction_ExecutesWithoutBlocking()
        {
            var taskExecutor = TestExecutorFactory.CreateCancellableTaskExecutor();
            var fakeWindowService = new Mock<IWindowService>();
            var cancellationTokenSource = new CancellationTokenSource();

            int x = 0;
            var progressAction = new Action<CancellationToken>(i_Token =>
            {
                Task.Delay(TimeSpan.FromMilliseconds(300)).Wait();
                x = 5;
            });

            var progressDialogVM = new IndefiniteProgressDialogViewModel<bool>(
                taskExecutor, progressAction,
                cancellationTokenSource)
            {
                WindowService = fakeWindowService.Object
            };

            var progressTask = progressDialogVM.DoProgress();
            x = 3;

            Assert.False(progressTask.IsCompleted);
            Assert.AreEqual(3, x);

            cancellationTokenSource.Dispose();
        }

        [Test]
        public void DoProgress_ValidProgressFunction_Executes()
        {
            var taskExecutor = TestExecutorFactory.CreateCancellableTaskExecutor();
            var fakeWindowService = new Mock<IWindowService>();

            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                bool flag = false;
                var progressFunction = new Func<CancellationToken, bool>(i_Token =>
                {
                    flag = true;
                    return flag;
                });

                var progressDialogVM = new IndefiniteProgressDialogViewModel<bool>(
                    taskExecutor, progressFunction,
                    cancellationTokenSource)
                {
                    WindowService = fakeWindowService.Object
                };

                var progressTask = progressDialogVM.DoProgress();
                progressTask.Wait();

                Assert.True(progressTask.IsCompleted);
                Assert.True(flag);
            }
        }

        [Test]
        public void DoProgress_ValidProgressFunction_ExecutesWithoutBlocking()
        {
            var taskExecutor = TestExecutorFactory.CreateCancellableTaskExecutor();
            var fakeWindowService = new Mock<IWindowService>();

            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                int x = 0;
                var progressFunction = new Func<CancellationToken, int>(i_Token =>
                {
                    Task.Delay(TimeSpan.FromMilliseconds(300), i_Token).Wait(i_Token);
                    x = 5;
                    return x;
                });

                var progressDialogVM = new IndefiniteProgressDialogViewModel<int>(
                    taskExecutor, progressFunction,
                    cancellationTokenSource)
                {
                    WindowService = fakeWindowService.Object
                };

                var progressTask = progressDialogVM.DoProgress();
                x = 3;

                Assert.False(progressTask.IsCompleted);
                Assert.AreEqual(3, x);
            }
        }

        #endregion

        #region Cancel Progress

        [Test]
        public void CancelProgress_ExecutedTask_IsCanceled()
        {
            var taskExecutor = TestExecutorFactory.CreateCancellableTaskExecutor();
            var fakeWindowService = new Mock<IWindowService>();
            var cancellationTokenSource = new CancellationTokenSource();

            var progressAction = new Action<CancellationToken>(i_Token =>
            {
                Task.Delay(TimeSpan.FromMilliseconds(100), i_Token).Wait(i_Token);
                i_Token.ThrowIfCancellationRequested();
            });

            var progressDialogVM = new IndefiniteProgressDialogViewModel<bool>(
                taskExecutor, progressAction,
                cancellationTokenSource)
            {
                WindowService = fakeWindowService.Object
            };

            var progressTask = progressDialogVM.DoProgress();
            progressDialogVM.CancelProgress(null);

            try
            {
                progressTask.Wait();
            }
            catch (AggregateException)
            {
                Assert.AreEqual(TaskStatus.Canceled, progressTask.Status);
            }
            finally
            {
                cancellationTokenSource.Dispose();
            }
        }

        [Test]
        public void CancelProgress_ValidCancellationCallback_IsCalled()
        {
            var taskExecutor = TestExecutorFactory.CreateCancellableTaskExecutor();
            var fakeWindowService = new Mock<IWindowService>();
            var cancellationTokenSource = new CancellationTokenSource();

            var progressAction = new Action<CancellationToken>(i_Token =>
            {
                Task.Delay(TimeSpan.FromMilliseconds(100), i_Token).Wait(i_Token);
                i_Token.ThrowIfCancellationRequested();
            });
            int x = 0;
            var cancellationCallback = new Action(() => x = 5);

            var progressDialogVM = new IndefiniteProgressDialogViewModel<bool>(
                taskExecutor, progressAction,
                cancellationTokenSource, i_CancellationCallback: cancellationCallback)
            {
                WindowService = fakeWindowService.Object
            };

            var progressTask = progressDialogVM.DoProgress();
            progressDialogVM.CancelProgress(null);
            try
            {
                progressTask.Wait();
            }
            catch (AggregateException)
            {
                Assert.AreEqual(5, x);
            }
            finally
            {
                cancellationTokenSource.Dispose();
            }
        }

        [Test]
        public void CancelProgress_RequestsToCloseWindow()
        {
            var taskExecutor = TestExecutorFactory.CreateCancellableTaskExecutor();
            var fakeWindowService = new Mock<IWindowService>();
            var cancellationTokenSource = new CancellationTokenSource();

            var progressAction = new Action<CancellationToken>(i_Token =>
            {
                Task.Delay(TimeSpan.FromMilliseconds(100), i_Token).Wait(i_Token);
                i_Token.ThrowIfCancellationRequested();
            });
            fakeWindowService.Setup(i_Service => i_Service.CloseWindow(false)).Verifiable();

            var progressDialogVM = new IndefiniteProgressDialogViewModel<bool>(
                taskExecutor, progressAction,
                cancellationTokenSource)
            {
                WindowService = fakeWindowService.Object
            };

            var progressTask = progressDialogVM.DoProgress();
            progressDialogVM.CancelProgress(null);
            try
            {
                progressTask.Wait();
            }
            catch (AggregateException)
            {
                fakeWindowService.Verify(i_Service => i_Service.CloseWindow(false));
            }
            finally
            {
                cancellationTokenSource.Dispose();
            }
        }

        #endregion

        #region GUI

        [Test]
        [Apartment(ApartmentState.STA)]
        public void Constructor_ValidDataContext_AssignsWindowServiceToViewModel()
        {
            var taskExecutor = TestExecutorFactory.CreateCancellableTaskExecutor();

            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var progressDialogVM =
                    new IndefiniteProgressDialogViewModel<bool>(taskExecutor,
                        i_ProgressAction: null,
                        i_CancellationTokenSource: cancellationTokenSource);
                var progressDialog = new IndefiniteProgressDialog(progressDialogVM);

                Assert.AreSame(progressDialog, progressDialogVM.WindowService);
            }
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        [Category("UI")]
        public void LoadedEvent_ExecutesProgress()
        {
            var taskExecutor = TestExecutorFactory.CreateCancellableTaskExecutor();

            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                int x = 0;
                var progressAction = new Action<CancellationToken>(i_Token => x = 5);
                var progressDialogVM =
                    new IndefiniteProgressDialogViewModel<bool>(taskExecutor, progressAction,
                        cancellationTokenSource);
                var progressDialog = new IndefiniteProgressDialog(progressDialogVM);

                progressDialog.ShowDialog();

                Assert.AreEqual(5, x);
            }
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        [Category("UI")]
        public void ClosedEvent_CancelsProgressExecution()
        {
            var taskExecutor = TestExecutorFactory.CreateCancellableTaskExecutor();

            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                int x = 0;
                var progressAction = new Action<CancellationToken>(i_Token =>
                {
                    Task.Delay(TimeSpan.FromSeconds(3), i_Token).Wait(i_Token);
                    x = 5;
                });
                var progressDialogVM =
                    new IndefiniteProgressDialogViewModel<bool>(taskExecutor, progressAction,
                        cancellationTokenSource);
                var progressDialog = new IndefiniteProgressDialog(progressDialogVM);

                Task.Factory.StartNew(() =>
                {
                    Task.Delay(TimeSpan.FromSeconds(0.5)).Wait();
                    progressDialog.Dispatcher.Invoke(() => progressDialog.Close(),
                        DispatcherPriority.Send);
                }, cancellationTokenSource.Token);
                progressDialog.ShowDialog();

                Assert.AreEqual(0, x);
            }
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        [Category("UI")]
        public void SuccessfulProgressExecution_ClosesWindowWithPositiveResult()
        {
            var taskExecutor = TestExecutorFactory.CreateCancellableTaskExecutor();

            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var progressAction = new Action<CancellationToken>(i_Token => { });
                var progressDialogVM =
                    new IndefiniteProgressDialogViewModel<bool>(taskExecutor, progressAction,
                        cancellationTokenSource);
                var progressDialog = new IndefiniteProgressDialog(progressDialogVM);

                var dialogResult = progressDialog.ShowDialog();

                Assert.True(dialogResult);
            }
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        [Category("UI")]
        public void UnsuccessfulProgressExecution_ClosesWindowWithNegativeResult()
        {
            var taskExecutor = TestExecutorFactory.CreateCancellableTaskExecutor();

            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var progressAction = new Action<CancellationToken>(i_Token =>
                {
                    throw new Exception();
                });
                var progressDialogVM =
                    new IndefiniteProgressDialogViewModel<bool>(taskExecutor, progressAction,
                        cancellationTokenSource);
                var progressDialog = new IndefiniteProgressDialog(progressDialogVM);

                var dialogResult = progressDialog.ShowDialog();

                Assert.False(dialogResult);
            }
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        [Category("UI")]
        public void CanceledProgressExecution_ClosesWindowWithNegativeResult()
        {
            var taskExecutor = TestExecutorFactory.CreateCancellableTaskExecutor();

            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var progressAction = new Action<CancellationToken>(i_Token =>
                {
                    Task.Delay(TimeSpan.FromSeconds(3), i_Token).Wait(i_Token);
                });
                var progressDialogVM =
                    new IndefiniteProgressDialogViewModel<bool>(taskExecutor, progressAction,
                        cancellationTokenSource);
                var progressDialog = new IndefiniteProgressDialog(progressDialogVM);

                Task.Run(() =>
                {
                    Task.Delay(TimeSpan.FromSeconds(0.5)).Wait();
                    progressDialog.Dispatcher.Invoke(() => progressDialog.Close(),
                        DispatcherPriority.Send);
                });
                var dialogResult = progressDialog.ShowDialog();

                Assert.False(dialogResult);
            }
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        [Category("UI")]
        public void Execution_ThrowsException_ExceptionCallback_IsCalled()
        {
            var taskExecutor = TestExecutorFactory.CreateCancellableTaskExecutor();

            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                int x = 0;
                var progressAction = new Action<CancellationToken>(i_Token =>
                {
                    throw new Exception();
                });
                var exceptionCallback = new Action<Exception>(i_Exception => x = 5);
                var progressDialogVM =
                    new IndefiniteProgressDialogViewModel<bool>(taskExecutor, progressAction,
                        cancellationTokenSource, i_ExceptionCallback: exceptionCallback);
                var progressDialog = new IndefiniteProgressDialog(progressDialogVM);

                progressDialog.ShowDialog();

                Assert.AreEqual(5, x);
            }
        }

        #endregion

        #endregion

        #region Properties



        #endregion
    }
}