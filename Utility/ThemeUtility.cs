using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Markup;

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

        private const string cm_THEME_FILE_EXTENSION = "xaml";

        private static IDictionary<string, string> sm_ThemeIcons;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the class's internal fields.
        /// </summary>
        static ThemeUtility()
        {
            sm_ThemeIcons = new Dictionary<string, string>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads the given EWPF built-in theme instead of the current one.
        /// </summary>
        /// <param name="i_Theme">EWPF built-in theme to load.</param>
        /// <returns>True if theme has been loaded successfully, false otherwise.</returns>
        public static bool LoadTheme(EWPFTheme i_Theme)
        {
            var appResourceDict = Application.Current.Resources;
            var themeDictionary = GetCurrentTheme();
            if (themeDictionary == null) // Theme dictionary doesn't exist
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
        /// Loads a custom theme file from the given parameters instead of the current one, 
        /// and possibly loads its' bound icons as well.
        /// </summary>
        /// <param name="i_ThemeFilePath">Theme file's path on the file system.</param>
        /// <remarks>
        /// The custom theme must follow EWPF's theme standard, which requires a set of colors and brushes, 
        /// and a top element specifying the theme's name.
        /// </remarks>
        /// <returns>True if theme has been loaded successfully, false otherwise.</returns>
        public static bool LoadTheme(string i_ThemeFilePath)
        {
            if (string.IsNullOrEmpty(i_ThemeFilePath))
                throw new ArgumentException(@"Theme file's path can't be null or empty", i_ThemeFilePath);

            var fileInfo = new FileInfo(i_ThemeFilePath);
            if (!fileInfo.Exists)
                throw new ArgumentException(@"Given path doesn't exist on the file system");
            if (fileInfo.Extension.ToLower() != cm_THEME_FILE_EXTENSION)
                throw new ArgumentException(@"Given theme file must have a " + cm_THEME_FILE_EXTENSION + " extension");

            ResourceDictionary loadedDictionary;
            // Load theme from file and dispose the stream as quickly as possible
            using (var fileStream = new FileStream(i_ThemeFilePath, FileMode.Open, FileAccess.Read))
            {
                loadedDictionary = XamlReader.Load(fileStream) as ResourceDictionary;
                if (loadedDictionary == null)
                    throw new InvalidCastException("Coludn't cast loaded XAML root element to a ResourceDictionary");
            }
            // Check that the loaded resource dictionary is actually a EWPF-constrainted theme.
            string themeName = loadedDictionary[cm_THEME_NAME_KEY] as string;
            if (string.IsNullOrEmpty(themeName))
                return false;

            // Get the currently active theme
            var currentTheme = GetCurrentTheme();
            if (currentTheme == null)
                return false;

            var appResourceDictionary = Application.Current.Resources;
            appResourceDictionary.MergedDictionaries.Remove(currentTheme);
            appResourceDictionary.MergedDictionaries.Insert(0, loadedDictionary);
            return true;
        }

        /// <summary>
        /// Load theme's special bounded icons into an internal dictionary, 
        /// storing keys as the icon's file name without the extension, and the values as the icon's full path.
        /// <para />
        /// The icons could be retrieved later using a !---------METHOD TO COMPLETE---------!
        /// </summary>
        /// <param name="i_IconsDirectoryPath">Path to the parent directory of all theme's icons.</param>
        /// <returns>True if icons were loaded successfully, false otherwise.</returns>
        public static bool LoadThemeIcons(string i_IconsDirectoryPath = null)
        {
            // ToDo: Implement later
            return false;
        }

        /// <summary>
        /// Finds the currently active theme through the app's dictionary and returns it.
        /// <para />
        /// This method assumes that the active theme is one of EWPF's built-in themes.
        /// </summary>
        /// <returns>An enum value corresponding to the active EWPF's built-in theme.</returns>
        public static EWPFTheme GetCurrentEWPFTheme()
        {
            var currentTheme = GetCurrentTheme();
            if (currentTheme == null)
                throw new Exception("No theme has been found in the app at all");

            string themeName = currentTheme[cm_THEME_NAME_KEY] as string;
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

        #region Helper Methods

        /// <summary>
        /// Finds the currently active theme in the app, and returns it as a <see cref="ResourceDictionary"/> object.
        /// </summary>
        /// <returns>Active theme represented as a full <see cref="ResourceDictionary"/> object.</returns>
        private static ResourceDictionary GetCurrentTheme()
        {
            var appResourceDict = Application.Current.Resources;

            // Find the theme dictionary out of all dictionaries
            foreach (var resourceDictionary in appResourceDict.MergedDictionaries)
            {
                string themeName = resourceDictionary[cm_THEME_NAME_KEY] as string;
                if (string.IsNullOrEmpty(themeName))
                    continue;
                return resourceDictionary;
            }
            return null; // Not a single dictionary is a theme dictionary
        }

        #endregion

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