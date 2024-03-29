﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using EWPF.Themes;

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

        internal const string THEME_NAME_KEY = "ThemeName";
        private const string cm_LIGHT_THEME_NAME = "Light Theme";
        private const string cm_DARK_THEME_NAME = "Dark Theme";
        private const string cm_WEB_THEME_NAME = "Web Theme";

        private const string cm_THEME_FILE_EXTENSION = "xaml";

        private static readonly IDictionary<string, string> sm_ThemeIcons;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the class's internal fields.
        /// </summary>
        static ThemeUtility()
        {
            sm_ThemeIcons = new Dictionary<string, string>();
            FillCommonExtensions();
        }

        #endregion

        #region Methods

        #region Loaders

        /// <summary>
        /// Loads the given EWPF built-in theme instead of the current one.
        /// </summary>
        /// <param name="i_Theme">EWPF built-in theme to load.</param>
        /// <returns>True if theme has been loaded successfully, false otherwise.</returns>
        public static bool LoadTheme(EWPFTheme i_Theme)
        {
            var appResourceDict = ApplicationDictionary ?? Application.Current.Resources;
            var themeDictionary = GetCurrentTheme();
            if (themeDictionary == null) // Theme dictionary doesn't exist
                return false;

            // Load a new theme based on it's type
            string themeUriString;
            switch (i_Theme)
            {
                case EWPFTheme.Light:
                    themeUriString = ThemeUri.LIGHT_THEME;
                    break;

                case EWPFTheme.Dark:
                    themeUriString = ThemeUri.DARK_THEME;
                    break;

                case EWPFTheme.Web:
                    themeUriString = ThemeUri.WEB_THEME;
                    break;

                default:
                    throw new ArgumentOutOfRangeException("i_Theme",
                        i_Theme, @"Unexpected theme type");
            }

            if (!Uri.IsWellFormedUriString(themeUriString, UriKind.RelativeOrAbsolute))
                throw new UriFormatException(i_Theme + " URI is malformed");
            if (!Regex.IsMatch(themeUriString, ThemeUri.THEME_URI_REGEX_PATTERN))
                throw new ThemeUriFormatException(themeUriString + " is an invalid WPF-Theme URI");
            var themeUri = new Uri(themeUriString, UriKind.RelativeOrAbsolute);

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
                throw new
                    ArgumentException(@"Theme file's path can't be null or empty",
                                      i_ThemeFilePath);

            var fileInfo = new FileInfo(i_ThemeFilePath);
            if (!fileInfo.Exists)
                throw new ArgumentException(@"Given path doesn't exist on the file system");
            if (fileInfo.Extension.ToLower() != cm_THEME_FILE_EXTENSION)
                throw new ArgumentException(@"Given theme file must have a " +
                                            cm_THEME_FILE_EXTENSION +
                                            " extension");

            ResourceDictionary loadedDictionary;
            // Load theme from file and dispose the stream as quickly as possible
            using (var fileStream = new FileStream(i_ThemeFilePath, FileMode.Open, FileAccess.Read))
            {
                loadedDictionary = XamlReader.Load(fileStream) as ResourceDictionary;
                if (loadedDictionary == null)
                    throw new
                        InvalidCastException("Couldn't cast loaded XAML root element to a ResourceDictionary");
            }
            // Check that the loaded resource dictionary is actually a EWPF-constrained theme.
            string themeName = loadedDictionary[THEME_NAME_KEY] as string;
            if (string.IsNullOrEmpty(themeName))
                return false;

            // Get the currently active theme
            var currentTheme = GetCurrentTheme();
            if (currentTheme == null)
                return false;

            var appResourceDictionary = ApplicationDictionary ?? Application.Current.Resources;
            appResourceDictionary.MergedDictionaries.Remove(currentTheme);
            appResourceDictionary.MergedDictionaries.Insert(0, loadedDictionary);
            return true;
        }

        /// <summary>
        /// Load theme's special bounded icons into an internal dictionary, 
        /// storing keys as the icon's file name without the extension, 
        /// and the values as the icon's full path. <br />
        /// The icons could be retrieved later using either 
        /// <see cref="GetIconPath"/> or <see cref="GetIcon"/> methods.
        /// </summary>
        /// <param name="i_IconsDirectoryPath">Path to the parent directory of all theme's icons.</param>
        /// <param name="i_AcceptedExtensions"></param>
        /// <returns>True if icons were loaded successfully, false otherwise.</returns>
        public static void LoadThemeIcons(string i_IconsDirectoryPath,
            IEnumerable<string> i_AcceptedExtensions)
        {
            if (string.IsNullOrEmpty(i_IconsDirectoryPath))
                throw new ArgumentException(@"Icons' directory path can't be null or empty");

            var dirInfo = new DirectoryInfo(i_IconsDirectoryPath);
            if (!dirInfo.Exists)
                throw new ArgumentException(@"Given path doesn't exist on the file system");

            // Clear the current icons dictionary and add all files in the 
            // folder that match one of the given extensions
            sm_ThemeIcons.Clear();
            var dirFiles = dirInfo.EnumerateFiles();
            // ToDo: Check if LINQ expression can be parallel
            var acceptedExtensionsCopy =
                i_AcceptedExtensions as List<string> ??
                i_AcceptedExtensions.ToList();
            var iconFiles =
                dirFiles.Where(i_Info =>
                                   acceptedExtensionsCopy
                                       .Contains(i_Info.Extension.ToLower()));
            foreach (var iconFile in iconFiles)
            {
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(iconFile.Name);
                // ToDo: Try to compress file's path
                sm_ThemeIcons.Add(fileNameWithoutExt, iconFile.FullName);
            }
        }

        #endregion

        #region Getters

        /// <summary>
        /// Searches the given icon name in the internal icons dictionary, 
        /// and returns its' full path on the file system if found.
        /// </summary>
        /// <param name="i_IconName">Icon's name - Must match the icon's name on the 
        /// file system without extension.</param>
        /// <returns>Icon's full path on the file system.</returns>
        public static string GetIconPath(string i_IconName)
        {
            if (string.IsNullOrEmpty(i_IconName))
                throw new ArgumentException(@"Icon name can't be null or empty");

            string iconPath;
            bool isIconExist = sm_ThemeIcons.TryGetValue(i_IconName, out iconPath);
            if (!isIconExist)
                throw new
                    KeyNotFoundException("Given icon name doesn't exist in the themes' icons - " +
                                         "Assert the given name is equal to the icon's file name without extension");
            return iconPath;
        }

        /// <summary>
        /// Searches the given icon name in the internal icons dictionary, 
        /// and returns it as an <see cref="ImageSource"/> object.
        /// </summary>
        /// <param name="i_IconName">Icon's name - Must match the icon's name on the 
        /// file system without extension.</param>
        /// <returns><see cref="BitmapSource"/> object representing the icon.</returns>
        public static ImageSource GetIcon(string i_IconName)
        {
            string iconFullPath = GetIconPath(i_IconName);
            return new BitmapImage(new Uri(iconFullPath));
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

            string themeName = currentTheme[THEME_NAME_KEY] as string;
            switch (themeName)
            {
                case cm_LIGHT_THEME_NAME:
                    return EWPFTheme.Light;
                case cm_DARK_THEME_NAME:
                    return EWPFTheme.Dark;
                case cm_WEB_THEME_NAME:
                    return EWPFTheme.Web;
                default:
                    throw new
                        Exception("A theme has been found but it's not an EWPF built-in theme. " +
                                  "Please use the appropriate method.");
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Finds the currently active theme in the app, 
        /// and returns it as a <see cref="ResourceDictionary"/> object.
        /// </summary>
        /// <returns>Active theme represented as a full 
        /// <see cref="ResourceDictionary"/> object.</returns>
        private static ResourceDictionary GetCurrentTheme()
        {
            var appResourceDict = ApplicationDictionary ?? Application.Current.Resources;

            // Find the theme dictionary out of all dictionaries
            foreach (var resourceDictionary in appResourceDict.MergedDictionaries)
            {
                string themeName = resourceDictionary[THEME_NAME_KEY] as string;
                if (string.IsNullOrEmpty(themeName))
                    continue;
                return resourceDictionary;
            }
            return null; // Not a single dictionary is a theme dictionary
        }

        /// <summary>
        /// Fills the <see cref="CommonIconExtensions"/> property with hard-coded values.
        /// </summary>
        private static void FillCommonExtensions()
        {
            var extensionList =
                new List<string>
                {
                    ".bmp",
                    ".jpg",
                    ".jpeg",
                    ".gif",
                    ".png",
                    ".ico"
                };
            CommonIconExtensions = extensionList.AsReadOnly();
        }

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of strings representing 
        /// all commonly used extensions for icon/image files.
        /// </summary>
        public static IEnumerable<string> CommonIconExtensions { get; private set; }

        /// <summary>
        /// Gets or sets a reference to the application's main resource dictionary.
        /// <para />
        /// This property should not be used outside Unit Testing!
        /// </summary>
        public static ResourceDictionary ApplicationDictionary { get; set; }

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