using System;
using System.Collections.Generic;
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

        public static IDictionary<DictionaryCode, string> MakeLanguageDictionary(int i_NumberOfWords)
        {
            var dict = new Dictionary<DictionaryCode, string>();
            switch (i_NumberOfWords)
            {
                case 0:
                    return dict;

                case 1:
                    dict.Add(DictionaryCode.Yes, "Yes");
                    break;

                case 2:
                    dict.Add(DictionaryCode.Yes, "Yes");
                    dict.Add(DictionaryCode.No, "No");
                    break;

                default:
                    throw new NotImplementedException();
            }
            return dict;
        }

        #endregion

        #region Properties



        #endregion
    }
}