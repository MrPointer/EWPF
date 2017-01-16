using System;
using System.Collections.Generic;
using EWPF.Utility;
using NUnit.Framework;
using System.Windows;

namespace EWPF_Tests.Unit
{
    [TestFixture]
    public class ThemeUtilityTests
    {
        #region Events



        #endregion

        #region Fields



        #endregion

        #region Constructors



        #endregion

        #region Methods

        [Test]
        public void LoadThemeEnumParam_ValueOutOfRange_ThrowsArgumentOutOfRangeException()
        {
            var mainDictionary = MakeResourceDictionary();
            var mergedDictionary = MakeResourceDictionary("abc");
            mainDictionary.MergedDictionaries.Add(mergedDictionary);
            ThemeUtility.ApplicationDictionary = mainDictionary;

            const EWPFTheme badEWPFThemeValue = (EWPFTheme)4;
            Assert.Catch<ArgumentOutOfRangeException>(() => ThemeUtility.LoadTheme(badEWPFThemeValue));
        }

        #region Get Icon

        [Test]
        public void GetIconPath_BadName_ThrowsKeyNotFoundException()
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

        #region Factory Methods

        /// <summary>
        /// Makes a new resource dictionary containing the given theme name as a value
        /// with the <see cref="ThemeUtility.THEME_NAME_KEY"/> key.
        /// <para />
        /// This dictionary functions as a merged dictionary to be added to a main dictionary later.
        /// </summary>
        /// <param name="i_ThemeName">Theme name to add as the value of the dictionary.</param>
        /// <returns>Reference to the created dictionary.</returns>
        public static ResourceDictionary MakeResourceDictionary(string i_ThemeName)
        {
            var createdResourceDictionary = new ResourceDictionary
            {
                {ThemeUtility.THEME_NAME_KEY, i_ThemeName}
            };
            return createdResourceDictionary;
        }

        /// <summary>
        /// Makes a new resource dictionary which functions as the main dictionary.
        /// </summary>
        /// <returns>Reference to the created dictionary.</returns>
        public static ResourceDictionary MakeResourceDictionary()
        {
            return new ResourceDictionary();
        }

        #endregion

        #endregion

        #region Properties



        #endregion
    }
}