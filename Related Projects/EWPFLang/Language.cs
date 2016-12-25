using EWPFLang.ELang;

namespace EWPFLang
{
    /// <summary>
    /// A class representing a language parsed from a user-defined file.
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



        #endregion

        #region Properties

        public LanguageCode Code { get; private set; }

        #endregion
    }
}