using System.Collections.Generic;

namespace EWPFLang
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
            languageObject = new ELanguage(i_Code);
            languageObject.LoadDictionaryFromXml(ConstantValues.DefaultELanguagesFolderPath);
            m_Languages.Add(i_Code, languageObject);
            return languageObject;
        }

        #endregion

        #region Properties



        #endregion
    }
}