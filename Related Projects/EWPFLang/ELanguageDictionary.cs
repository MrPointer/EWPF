namespace EWPFLang
{
    /// <summary>
    /// A static class used as a language dictionary, providing various strings used throughout the EWPF library in different languages.
    /// </summary>
    public static class ELanguageDictionary
    {
        #region Events

        #endregion

        #region Fields



        #endregion

        #region Constructors

        #endregion

        #region Methods

        /// <summary>
        /// Loads a language file containing all language values stated as properties in this class based on the given language.
        /// </summary>
        /// <param name="i_Language">Language to load.</param>
        public static void LoadLanguage(Language i_Language)
        {

        }

        #endregion

        #region Properties

        public static string Yes { get; private set; }

        public static string No { get; private set; }

        public static string Cancel { get; private set; }

        public static string OK { get; private set; }

        #endregion
    }
}