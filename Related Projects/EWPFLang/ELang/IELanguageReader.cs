using System.Collections.Generic;

namespace EWPFLang.ELang
{
    /// <summary>
    /// An interface declaring methods to read a language file.
    /// </summary>
    public interface IELanguageReader
    {
        #region Fields

        #endregion

        #region Methods

        /// <summary>
        /// Loads a language from the given file path, and constructs a dictionary object from it.
        /// <para />
        /// The language dictionary has <see cref="DictionaryCode"/> codes as keys, and their translations as values.
        /// </summary>
        /// <param name="i_FilePath">Path to the language file on the local file system.</param>
        /// <returns>Language dictionary object.</returns>
        IDictionary<DictionaryCode, string> LoadLanguageFile(string i_FilePath);

        #endregion

        #region Properties

        #endregion
    }
}