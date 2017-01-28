using System;
using System.Windows;
using EWPF.Utility;
using Moq;
using NUnit.Framework;

namespace EWPF_Tests.Unit
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
            var catchedException = Assert.Catch<ArgumentNullException>(
                () => WindowUtility.CloseWindow(null, true));
            StringAssert.Contains("can't be null", catchedException.Message);
        }

        #endregion

        #region Properties



        #endregion
    }
}