using System;
using System.Collections.Generic;
using System.Windows;
using EWPF.Themes;
using EWPF.Utility;
using NUnit.Framework;

namespace EWPF_Tests.Unit.Utility
{
    [TestFixture]
    public class ThemeUtilityTests
    {
        #region Events



        #endregion

        #region Fields



        #endregion

        #region Constructors

        [TearDown]
        public void ResetClass()
        {
            ThemeUtility.ApplicationDictionary = null;
            GC.Collect();
        }

        #endregion

        #region Methods

        #region Load Theme

        [Test]
        public void LoadThemeEnumParam_ValueOutOfRange_ThrowsArgumentOutOfRangeException()
        {
            ThemeUtility.ApplicationDictionary = MergeDictionaries("abc");

            const EWPFTheme badEWPFThemeValue = (EWPFTheme)4;
            Assert.Catch<ArgumentOutOfRangeException>(() => ThemeUtility.LoadTheme(badEWPFThemeValue));
        }

        [Test]
        public void LoadThemeEnumParam_NonExistingDictionary_ReturnsFalse()
        {
            var mainDictionary = MakeResourceDictionary();
            ThemeUtility.ApplicationDictionary = mainDictionary;

            bool isThemeLoaded = ThemeUtility.LoadTheme(EWPFTheme.Light);
            Assert.False(isThemeLoaded);
        }

        [Test]
        [Category("ConstantDependent")]
        public void LoadThemeEnumParam_InvalidUri_ThrowsUriFormatException()
        {
            ThemeUtility.ApplicationDictionary = MergeDictionaries("abc");

            Assert.Catch<UriFormatException>(() => ThemeUtility.LoadTheme(EWPFTheme.Light));
           }

        #endregion

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

        #region Helper Methods

        /// <summary>
        /// Creates a resource dictionary containing the given theme name as a value,
        /// and merges it into another resource dictionary which is returned later.
        /// </summary>
        /// <param name="i_ThemeName">Theme name to add as a value of the merged dictionary.</param>
        /// <returns>Reference to the main-merging dictionary.</returns>
        private static ResourceDictionary MergeDictionaries(string i_ThemeName)
        {
            var mainDictionary = MakeResourceDictionary();
            var mergedDictionary = MakeResourceDictionary(i_ThemeName);
            mainDictionary.MergedDictionaries.Add(mergedDictionary);
            return mainDictionary;
        }

        #endregion

        #endregion

        #region Properties



        #endregion
    }
}