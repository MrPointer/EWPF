using System;
using System.Collections.Generic;

namespace EWPFLang.ELang
{
    /// <summary>
    /// A static class used to provide users a language object of their choosing.
    /// </summary>
    public static class LanguageRepository
    {
        #region Events

        #endregion

        #region Fields

        private static readonly Dictionary<LanguageCode, ELanguage> m_Languages;

        #endregion

        #region Constructors

        static LanguageRepository()
        {
            m_Languages = new Dictionary<LanguageCode, ELanguage>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the repository by setting the <see cref="LanguageReader"/> to the requested implementation.
        /// </summary>
        /// <param name="i_StorageType">Represents how the language files to be loaded are stored.</param>
        public static void Initialize(LanguageStorageType i_StorageType)
        {
            switch (i_StorageType)
            {
                case LanguageStorageType.Xml:
                    LanguageReader = LanguageXmlReader.Instance;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(i_StorageType), i_StorageType, null);
            }
        }

        /// <summary>
        /// Gets the requested language along with its' dictionary of translated words.
        /// <para/>
        /// If the language has already been requested once, it will just return a reference to its' object, 
        /// otherwise it will be loaded from an XML file.
        /// </summary>
        /// <param name="i_Code">Code of the language to get.</param>
        /// <returns>Language object containing its' translated dictionary.</returns>
        public static ELanguage GetLanguage(LanguageCode i_Code)
        {
            ELanguage languageObject;

            bool isLanguageLoaded = m_Languages.TryGetValue(i_Code, out languageObject);
            if (isLanguageLoaded)
                return languageObject;

            if (LanguageReader == null)
                throw new InvalidOperationException("Language reader must be set to load a new language, " +
                                                    "please use the " + nameof(Initialize) + " method.");

            languageObject = new ELanguage(i_Code, LanguageReader);
            languageObject.LoadDictionaryFromFile(ConstantValues.DefaultELanguagesFolderPath);
            m_Languages.Add(i_Code, languageObject);
            return languageObject;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the language reader used to read new languages.
        /// </summary>
        public static IELanguageReader LanguageReader { get; set; }

        #endregion
    }
}