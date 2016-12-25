using EWPFLang.ELang;

namespace ELang_UnitTests
{
    /// <summary>
    /// A static factory class providing methods to make instances of EWPF-language related objects using a factory.
    /// </summary>
    public static class LanguageFactory
    {
        #region Events

        #endregion

        #region Fields



        #endregion

        #region Constructors

        #endregion

        #region Methods

        public static ELanguage MakeELanguage()
        {
            return new ELanguage();
        }

        public static ELanguage MakeELanguage(LanguageCode i_LanguageCode, IELanguageReader i_LanguageReader)
        {
            return new ELanguage(i_LanguageCode, i_LanguageReader);
        }

        #endregion

        #region Properties



        #endregion
    }
}