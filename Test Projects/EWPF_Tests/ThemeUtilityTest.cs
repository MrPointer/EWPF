using System;
using System.Collections.Generic;
using EWPF.Utility;
using NUnit.Framework;

namespace EWPF_UnitTests
{
    [TestFixture]
    public class ThemeUtilityTest
    {
        #region Events



        #endregion

        #region Fields



        #endregion

        #region Constructors



        #endregion

        #region Methods

        [Test]
        [Ignore("GUI Test")]
        public void LoadTheme_BadValue_ThrowsException()
        {
            const EWPFTheme badEWPFThemeValue = (EWPFTheme)4;
            Assert.Catch<ArgumentOutOfRangeException>(() => ThemeUtility.LoadTheme(badEWPFThemeValue));
        }

        [Test]
        public void GetIconPath_BadName_ThrowsKeyException()
        {
            const string badIconName = "abcd";
            Assert.Catch<KeyNotFoundException>(() => ThemeUtility.GetIconPath(badIconName));
        }

        [Test]
        public void GetIconPath_EmptyOrNullName_ThrowsArgumentException()
        {
            string emptyName = string.Empty;
            Assert.Catch<ArgumentException>(() => ThemeUtility.GetIconPath(emptyName));
            Assert.Catch<ArgumentException>(() => ThemeUtility.GetIconPath(null));
        }

        [Test]
        public void GetIcon_EmptyOrNullName_ThrowsArgumentException()
        {
            string emptyName = string.Empty;
            Assert.Catch<ArgumentException>(() => ThemeUtility.GetIcon(emptyName));
            Assert.Catch<ArgumentException>(() => ThemeUtility.GetIcon(null));
        }

        #endregion

        #region Properties



        #endregion
    }
}