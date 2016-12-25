namespace EWPFLang.ELang
{
    /// <summary>
    /// An enum listing all languages supported by the EWPF library.
    /// </summary>
    public enum LanguageCode
    {
        None,
        EnglishUs,
        EnglishUk,
        French,
        Italian,
        Spanish,
        Hebrew,
        Deutch,
        Russian
    }

    /// <summary>
    /// An enum listing all words available for translation by the EWPF library as numeral codes.
    /// </summary>
    public enum DictionaryCode
    {
        Yes,
        No,
        Cancel,
        OK
    }

    /// <summary>
    /// An enum representing all available types for language storage, mostly on the local file system.
    /// </summary>
    public enum LanguageStorageType
    {
        /// <summary>
        /// XML files.
        /// </summary>
        Xml
    }
}