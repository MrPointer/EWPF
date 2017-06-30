using System;

namespace EWPF.Themes
{
    /// <summary>
    /// A static class listing the <see cref="Uri"/> of all EWPF's built-in themes 
    /// as strings.
    /// </summary>
    public static class ThemeUri
    {
        #region Events

        #endregion

        #region Fields

        internal const string THEME_URI_REGEX_PATTERN = @"^pack:\/{2}application:,{3}.+\.xaml$";

        #region Built-in Theme Paths

        /// <summary>
        /// Uri format of the light theme.
        /// </summary>
        public const string LIGHT_THEME = "pack://application:,,,/EWPF;component/Themes/LightTheme.xaml";

        /// <summary>
        /// Uri format of the dark theme.
        /// </summary>
        public const string DARK_THEME = "pack://application:,,,/EWPF;component/Themes/DarkTheme.xaml";

        /// <summary>
        /// Uri format of the web theme.
        /// </summary>
        public const string WEB_THEME = "pack://application:,,,/EWPF;component/Themes/WebTheme.xaml"; 

        #endregion

        #endregion

        #region Constructors

        #endregion

        #region Methods



        #endregion

        #region Properties



        #endregion
    }
}