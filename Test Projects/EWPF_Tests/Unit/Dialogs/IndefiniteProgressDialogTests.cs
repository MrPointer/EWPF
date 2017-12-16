using System;
using System.Threading;
using EWPF.Dialogs;
using NUnit.Framework;

namespace EWPF_Tests.Unit.Dialogs
{
    [TestFixture]
    public class IndefiniteProgressDialogTests
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
        public void Constructor_NullDataContext_ThrowsArgumentNullException()
        {
            Assert.Catch<ArgumentNullException>(() => new IndefiniteProgressDialog(null));
        }

        #endregion

        #region Properties



        #endregion
    }
}