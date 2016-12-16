using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace EWPF.Utility
{
    /// <summary>
    /// A static utility class providing utility methods to operate on app-global themes.
    /// </summary>
    public static class ThemeUtility
    {
        #region Events

        #endregion

        #region Fields

        private const string cm_THEME_NAME_KEY = "ThemeName";
        private const string cm_LIGHT_THEME_NAME = "Light Theme";
        private const string cm_DARK_THEME_NAME = "Dark Theme";
        private const string cm_WEB_THEME_NAME = "Web Theme";

        private static IDictionary<string, string> sm_ThemeIcons;

        #endregion

        #region Constructors

        #endregion

        #region Methods

        public static bool LoadTheme(EWPFTheme i_Theme)
        {
            var appResourceDict = Application.Current.Resources;
            ResourceDictionary themeDictionary = null;
            bool isThemeDictFound = false;

            // Find the theme dictionary out of all dictionaries
            foreach (var resourceDictionary in appResourceDict.MergedDictionaries)
            {
                string themeName = resourceDictionary[cm_THEME_NAME_KEY] as string;
                if (string.IsNullOrEmpty(themeName)) continue;
                isThemeDictFound = true;
                themeDictionary = resourceDictionary;
                break;
            }
            if (!isThemeDictFound) // Not a single dictionary is a theme dictionary
                return false;

            // Load a new theme based on it's type
            Uri themeUri;
            switch (i_Theme)
            {
                case EWPFTheme.Light:
                    themeUri = new Uri("pack://application:,,,/EWPF;component/Themes/LightTheme.xaml", UriKind.RelativeOrAbsolute);
                    break;

                case EWPFTheme.Dark:
                    themeUri = new Uri("pack://application:,,,/EWPF;component/Themes/DarkTheme.xaml", UriKind.RelativeOrAbsolute);
                    break;

                case EWPFTheme.Web:
                    themeUri = new Uri("pack://application:,,,/EWPF;component/Themes/WebTheme.xaml", UriKind.RelativeOrAbsolute);
                    break;

                default:
                    throw new ArgumentOutOfRangeException("i_Theme", i_Theme, @"Unexpected theme type");
            }
            var loadedThemeDictionary = new ResourceDictionary { Source = themeUri };
            appResourceDict.MergedDictionaries.Remove(themeDictionary);
            appResourceDict.MergedDictionaries.Insert(0, loadedThemeDictionary);
            return true;
        }

        /// <summary>
        /// Finds the currently active theme through the app's dictionary and returns it.
        /// <para />
        /// This method assumes that the active theme is one of EWPF's built-in themes.
        /// </summary>
        /// <returns>An enum value corresponding to the active EWPF's built-in theme.</returns>
        public static EWPFTheme GetCurrentEWPFTheme()
        {
            var appResourceDict = Application.Current.Resources;

            // Find the theme dictionary out of all dictionaries
            foreach (var resourceDictionary in appResourceDict.MergedDictionaries)
            {
                string themeName = resourceDictionary[cm_THEME_NAME_KEY] as string;
                if (string.IsNullOrEmpty(themeName)) continue;
                switch (themeName)
                {
                    case cm_LIGHT_THEME_NAME:
                        return EWPFTheme.Light;
                    case cm_DARK_THEME_NAME:
                        return EWPFTheme.Dark;
                    case cm_WEB_THEME_NAME:
                        return EWPFTheme.Web;
                    default:
                        throw new Exception("A theme has been found but it's not an EWPF built-in theme. " +
                                            "Please use the appropriate method.");
                }
            }
            throw new Exception("No theme has been found in the app at all");
        }

        #endregion

        #region Properties



        #endregion
    }

    /// <summary>
    /// An enum listing all themes provided by the EWPF library as enum members.
    /// </summary>
    public enum EWPFTheme
    {
        /// <summary>
        /// Main light theme.
        /// </summary>
        Light,
        /// <summary>
        /// Main dark theme.
        /// </summary>
        Dark,
        /// <summary>
        /// Web Theme 1.
        /// </summary>
        Web
    }
}