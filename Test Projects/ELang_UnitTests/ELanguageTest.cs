using System;
using System.Collections.Generic;
using ELang_UnitTests.Fakes;
using EWPFLang.ELang;
using NUnit.Framework;

namespace ELang_UnitTests
{
    [TestFixture]
    public class ELanguageTest
    {
        #region Events



        #endregion

        #region Fields



        #endregion

        #region Constructors



        #endregion

        #region Methods

        #region Load Dictionary From File

        [Test]
        public void LoadDictionaryFromFile_NullOrEmptyFileName_ThrowsArgumentException()
        {
            var elang = LanguageFactory.MakeELanguage();
            Assert.Catch<ArgumentException>(() => elang.LoadDictionaryFromFile(null));
            Assert.Catch<ArgumentException>(() => elang.LoadDictionaryFromFile(string.Empty));
        }

        [Test]
        public void LoadDictionaryFromFile_UnsetCode_ThrowsInvalidOperationException()
        {
            var elang = LanguageFactory.MakeELanguage();
            Assert.Catch<InvalidOperationException>(() => elang.LoadDictionaryFromFile("test"));
        }

        [Test]
        public void LoadDictionaryFromFile_UnsetReader_ThrowsInvalidOperationException()
        {
            var elang = LanguageFactory.MakeELanguage(LanguageCode.EnglishUk, null);
            Assert.Catch<InvalidOperationException>(() => elang.LoadDictionaryFromFile("test"));
        }

        [Test]
        public void LoadDictionaryFromFile_SomeDictionary_ChangesState()
        {
            var testableReader = new TestableELanguageReader
            {
                DictionaryToReturn = new Dictionary<DictionaryCode, string>
                {
                    {DictionaryCode.OK, "OK"},
                    {DictionaryCode.Cancel, "Cancel"}
                }
            };
            var elang = LanguageFactory.MakeELanguage(LanguageCode.EnglishUs, testableReader);
            elang.LoadDictionaryFromFile("Cool");
            Assert.AreSame(elang.Dictionary, testableReader.DictionaryToReturn);
        }

        #endregion

        #region Get Word



        #endregion

        #region Dictionary Setter

        [Test]
        public void DictionarySetter_NullInputDictionary_ThrowsArgumentNullException()
        {
            var elanguage = LanguageFactory.MakeELanguage();
            Assert.Catch<ArgumentNullException>(() => elanguage.Dictionary = null);
        }

        [Test]
        public void DictionarySetter_EmptyDictionary_ThrowsArgumentException()
        {
            var elanguage = LanguageFactory.MakeELanguage();
            Assert.Catch<ArgumentException>(() => elanguage.Dictionary = new Dictionary<DictionaryCode, string>());
        }

        #endregion

        #endregion

        #region Properties



        #endregion
    }
}