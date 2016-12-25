using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EWPFLang.ELang
{
    /// <summary>
    /// A class representing a language.
    /// </summary>
    public class ELanguage
    {
        private IDictionary<DictionaryCode, string> m_Dictionary;

        #region Events



        #endregion

        #region Fields



        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public ELanguage()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public ELanguage(LanguageCode i_Code, IELanguageReader i_Reader)
        {
            Code = i_Code;
            LanguageReader = i_Reader;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads a language from a file on the local file system.
        /// </summary>
        /// <param name="i_LanguageFileParentDirectory">Path of the language file's folder.</param>
        public void LoadDictionaryFromFile(string i_LanguageFileParentDirectory)
        {
            if (string.IsNullOrEmpty(i_LanguageFileParentDirectory))
                throw new ArgumentException(@"Language file's path can't be null or empty", i_LanguageFileParentDirectory);
            if (Code == LanguageCode.None)
                throw new InvalidOperationException("Language code must be set to load a dictionary");
            if (LanguageReader == null)
                throw new InvalidOperationException("Language reader must be set to load a language");

            string fileName = Code.ToString();

            Dictionary = LanguageReader.LoadLanguageFile(i_LanguageFileParentDirectory +
                Path.DirectorySeparatorChar + fileName);
        }

        /// <summary>
        /// Searches the language's dictionary for the given word and returns its' value.
        /// </summary>
        /// <param name="i_WordCode">Code of the word to get.</param>
        /// <returns>Word's value translated to the object's language.</returns>
        public string GetWord(DictionaryCode i_WordCode)
        {
            if (Dictionary == null || !Dictionary.Any()) // Dictionary unavailable
                throw new InvalidOperationException("Cant get a word from a language that has no dictionary assigned to it");

            string wordValue;
            bool isWordFound = Dictionary.TryGetValue(i_WordCode, out wordValue);
            if (!isWordFound) // Word not found in dictionary
                throw new WordNotFoundExcpetion("Current dictionary doesn't contain the given word", i_WordCode);
            return wordValue;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Represents language's dictionary, containing words translated to the requested language.
        /// </summary>
        public IDictionary<DictionaryCode, string> Dictionary
        {
            get { return m_Dictionary; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value), @"Input dictionary can't be null");
                if (!value.Any()) // Empty dictionary
                    throw new ArgumentException(@"Input dictionary must contain at least one value");

                m_Dictionary = value;
            }
        }

        /// <summary>
        /// Describes the language code of this language object.
        /// </summary>
        public LanguageCode Code { get; set; }

        /// <summary>
        /// Gets a reference to language reader object, used to read a language file to memory.
        /// </summary>
        public IELanguageReader LanguageReader { get; }

        #endregion
    }
}