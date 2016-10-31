using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace EWPFLang
{
    /// <summary>
    /// A singleton class used to read language xml files to memory.
    /// </summary>
    public class LanguageXmlReader
    {
        #region Events



        #endregion

        #region Fields

        #region Singleton

        private static readonly Lazy<LanguageXmlReader> m_Instance = new Lazy<LanguageXmlReader>(() => new LanguageXmlReader());

        #endregion

        #endregion

        #region Constructors

        private LanguageXmlReader()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reads a language's dictionary from the given XML file.
        /// </summary>
        /// <param name="i_FilePath">Language file's path.</param>
        /// <returns>Dictionary of words' codes as keys and words' translations as values.</returns>
        internal IDictionary<DictionaryCode, string> LoadLanguageFile(string i_FilePath)
        {
            if (string.IsNullOrEmpty(i_FilePath))
                throw new ArgumentException(@"Language file's path cant be null or empty", i_FilePath);

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

        #region Properties

        #region Singleton

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static LanguageXmlReader Instance
        {
            get { return m_Instance.Value; }
        }

        #endregion

        #endregion
    }
}