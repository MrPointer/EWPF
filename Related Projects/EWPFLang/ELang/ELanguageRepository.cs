using System;
using System.Collections.Generic;

namespace EWPFLang.ELang
{
    /// <summary>
    /// A static class used to provide users a language object of their choosing.
    /// </summary>
    public static class ELanguageRepository
    {
        #region Events

        #endregion

        #region Fields

        // Internal to make it visible to unit tests.
        internal static readonly Dictionary<LanguageCode, ELanguage> Languages;

        #endregion

        #region Constructors

        static ELanguageRepository()
        {
            Languages = new Dictionary<LanguageCode, ELanguage>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the repository by setting the <see cref="LanguageReader"/> to the requested implementation.
        /// </summary>
        /// <param name="i_StorageType">Represents how the language files to be loaded are stored.</param>
        /// <param name="i_LanguageDirectoryPath">Path to the main languages directory path, 
        /// containing all language files used by the library.</param>
        public static void Initialize(LanguageStorageType i_StorageType, string i_LanguageDirectoryPath = null)
        {
            switch (i_StorageType)
            {
                case LanguageStorageType.Xml:
                    LanguageReader = LanguageXmlReader.Instance;
                    break;

                default:
                    throw new ArgumentOutOfRangeException("i_StorageType", i_StorageType, null);
            }
            LanguagesDirectoryPath = i_LanguageDirectoryPath;
        }

        /// <summary>
        /// Gets the requested language along with its' dictionary of translated words.
        /// <para/>
        /// If the language has already been requested once, it will just return a reference to its' object, 
        /// otherwise it will be loaded from an XML file.
        /// </summary>
        /// <param name="i_Code">Code of the language to get.</param>
        /// <param name="i_LanguagesDirectoryPath">Path of the parent directory of all language files.</param>
        /// <returns>Language object containing its' translated dictionary.</returns>
        public static ELanguage GetLanguageFromFile(LanguageCode i_Code, string i_LanguagesDirectoryPath = null)
        {
            ELanguage languageObject;
            bool isLanguageLoaded = Languages.TryGetValue(i_Code, out languageObject);
            if (isLanguageLoaded)
                return languageObject;

            if (LanguageReader == null)
                throw new InvalidOperationException("Language reader must be set to load a new language, " +
                                                    "please use the Initialize method.");

            languageObject = new ELanguage(i_Code, LanguageReader);
            languageObject.LoadDictionaryFromFile(i_LanguagesDirectoryPath ??
                                                  LanguagesDirectoryPath ?? ConstantValues
                                                      .DefaultELanguagesFolderPath);
            Languages.Add(i_Code, languageObject);
            return languageObject;
        }

        /// <summary>
        /// Terminates the class by resetting it's state to the one present before calling any method on it.
        /// Use this class only for unit tests!
        /// </summary>
        internal static void Terminate()
        {
            Languages.Clear();
            LanguageReader = null;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the language reader used to read new languages.
        /// </summary>
        public static IELanguageReader LanguageReader { get; set; }

        /// <summary>
        /// Gets the languages directory path set externally.
        /// </summary>
        public static string LanguagesDirectoryPath { get; private set; }

        #endregion
    }
}