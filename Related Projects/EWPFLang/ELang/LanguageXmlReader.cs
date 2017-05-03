using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace EWPFLang.ELang
{
    /// <summary>
    /// A singleton class used to read language XML files to memory.
    /// </summary>
    public class LanguageXmlReader : IELanguageReader
    {
        #region Events



        #endregion

        #region Fields

        #region Singleton

        private static readonly Lazy<LanguageXmlReader> sm_Instance = new Lazy<LanguageXmlReader>(() => new LanguageXmlReader());

        #endregion

        #endregion

        #region Constructors

        private LanguageXmlReader()
        {
        }

        #endregion

        #region Methods

        #region Implementation of ILanguageReader

        /// <summary>
        /// Loads a language from the given file path, and constructs a dictionary object from it.
        /// <para />
        /// The language dictionary has <see cref="DictionaryCode"/> codes as keys, and their translations as values.
        /// </summary>
        /// <param name="i_FilePath">Path to the language file on the local file system.</param>
        /// <returns>Language dictionary object.</returns>
        public IDictionary<DictionaryCode, string> LoadLanguageFile(string i_FilePath)
        {
            if (string.IsNullOrEmpty(i_FilePath))
                throw new ArgumentException(@"Language file's path cant be null or empty", "i_FilePath");

            bool hasExtension = Regex.IsMatch(i_FilePath, @"\.xml$");
            if (!hasExtension) // File path should be appended with the '.xml' extension
                i_FilePath += ".xml";

            var dictionary = new Dictionary<DictionaryCode, string>();

            var rootElement = XElement.Load(i_FilePath);
            foreach (var wordElement in rootElement.Elements())
            {
                string elementName = wordElement.Name.ToString();
                string elementValue = wordElement.Value;

                DictionaryCode matchingDictionaryCode;
                bool isParsingSuccessful = Enum.TryParse(elementName, true, out matchingDictionaryCode);
                if (!isParsingSuccessful)
                    throw new LanguageParseException("Couldn't parse element to a valid dictionary code", elementName);

                dictionary.Add(matchingDictionaryCode, elementValue);
            }
            return dictionary;
        }

        #endregion

        #endregion

        #region Properties

        #region Singleton

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static LanguageXmlReader Instance
        {
            get { return sm_Instance.Value; }
        }

        #endregion

        #endregion
    }
}