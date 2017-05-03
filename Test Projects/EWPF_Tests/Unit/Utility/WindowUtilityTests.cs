using System;
using EWPF.Utility;
using NUnit.Framework;

namespace EWPF_Tests.Unit.Utility
{
    [TestFixture]
    public class WindowUtilityTests
    {
        #region Events



        #endregion

        #region Fields



        #endregion

        #region Constructors



        #endregion

        #region Methods

        [Test]
        public void CloseWindow_NullWindowParam_ThrowsArgumentNullException()
        {
            var exception = Assert.Catch<ArgumentNullException>(
                () => WindowUtility.CloseWindow(null, true));
            StringAssert.Contains("can't be null", exception.Message);
        }

        #endregion

        #region Properties



        #endregion
    }
}