using System.Collections.Generic;
using EWPFLang.ELang;

namespace ELang_UnitTests.Fakes
{
    /// <summary>
    /// A fake class used to test the <see cref="IELanguageReader"/> interface.
    /// </summary>
    public class TestableELanguageReader : IELanguageReader
    {
        #region Events



        #endregion

        #region Fields



        #endregion

        #region Constructors



        #endregion

        #region Methods

        #region Implementation of IELanguageReader

        /// <summary>
        /// Loads a language from the given file path, and constructs a dictionary object from it.
        /// <para />
        /// The language dictionary has <see cref="DictionaryCode"/> codes as keys, and their translations as values.
        /// </summary>
        /// <param name="i_FilePath">Path to the language file on the local file system.</param>
        /// <returns>Language dictionary object.</returns>
        public IDictionary<DictionaryCode, string> LoadLanguageFile(string i_FilePath)
        {
            return DictionaryToReturn;
        }

        #endregion

        #endregion

        #region Properties

        public IDictionary<DictionaryCode, string> DictionaryToReturn { get; set; }

        #endregion        
    }
}