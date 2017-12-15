using System.Threading;
using KISCore.Execution;

namespace EWPF_Tests.Factory
{
    public static class TestExecutorFactory
    {
        #region Fields



        #endregion

        #region Constructors



        #endregion

        #region Methods

        public static ICancellableTaskExecutor<CancellationToken> CreateCancellableTaskExecutor()
        {
            return new CancellableTaskExecutor();
        }

        #endregion

        #region Properties



        #endregion

    }
}