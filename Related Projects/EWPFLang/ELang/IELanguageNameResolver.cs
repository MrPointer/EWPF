namespace EWPFLang.ELang
{
    /// <summary>
    /// An interface declaring methods to resolve the string representation of a language, meaning its' name.
    /// </summary>
    public interface IELanguageNameResolver
    {
        #region Fields

        #endregion

        #region Methods

        /// <summary>
        /// Resolves the given language code to a full language name.
        /// </summary>
        /// <param name="i_LanguageCode">Code of the language to resolve.</param>
        /// <returns>Full string representation of the given language code.</returns>
        string ResolveName(LanguageCode i_LanguageCode);

        #endregion

        #region Properties

        #endregion
    }
}