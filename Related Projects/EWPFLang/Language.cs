using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EWPFLang
{
    /// <summary>
    /// A class representing a language.
    /// </summary>
    public class Language
    {
        #region Events



        #endregion

        #region Fields



        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public Language(LanguageCode i_Code)
        {
            Code = i_Code;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads a language from a XML file on the local file system.
        /// </summary>
        /// <param name="i_XmlParentDirectory">Path of the language file's folder.</param>
        public void LoadDictionaryFromXml(string i_XmlParentDirectory)
        {
            string fileName = string.Empty;
            switch (Code)
            {
                case LanguageCode.EnglishUs:
                    fileName = "EnglishUS";
                    break;

                case LanguageCode.EnglishUk:
                    break;

                case LanguageCode.French:
                    break;

                case LanguageCode.Italian:
                    break;

                case LanguageCode.Spanish:
                    break;

                case LanguageCode.Hebrew:
                    fileName = "Hebrew";
                    break;

                case LanguageCode.Deutch:
                    break;

                case LanguageCode.Russian:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            var loadedDictionary = LanguageXmlReader.Instance.LoadLanguageFile(
                i_XmlParentDirectory + Path.DirectorySeparatorChar + fileName + ".xml");
            FillDictionary(loadedDictionary);
        }

        /// <summary>
        /// Fills language's dictionary object with the given dictionary.
        /// </summary>
        /// <param name="i_InputDictionary">Dictionary containing all translations.</param>
        public void FillDictionary(IDictionary<DictionaryCode, string> i_InputDictionary)
        {
            if (i_InputDictionary == null)
                throw new ArgumentNullException("i_InputDictionary", @"Input dictionary can't be null");
            if (!i_InputDictionary.Any()) // Empty dictionary
                throw new ArgumentException(@"Input dictionary must contain at least one value");

            Dictionary = i_InputDictionary;
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
        public IDictionary<DictionaryCode, string> Dictionary { get; private set; }

        /// <summary>
        /// Describes the language code of this language object.
        /// </summary>
        public LanguageCode Code { get; set; }

        #endregion
    }
}