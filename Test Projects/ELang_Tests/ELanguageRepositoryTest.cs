using System;
using System.Collections.Generic;
using ELang_UnitTests.Fakes;
using EWPFLang.ELang;
using NUnit.Framework;

namespace ELang_UnitTests
{
    [TestFixture]
    public class ELanguageRepositoryTest
    {
        #region Events



        #endregion

        #region Fields



        #endregion

        #region Constructors



        #endregion

        #region Methods

        #region Tear Down

        [TearDown]
        public void TerminateClass()
        {
            ELanguageRepository.Terminate();
        }

        #endregion

        #region Initialize

        [Test]
        public void Initialize_GoodValue_SetsLanguageReader()
        {
            ELanguageRepository.Initialize(LanguageStorageType.Xml);
            Assert.NotNull(ELanguageRepository.LanguageReader);
        }

        [Test]
        public void Initialize_BadValue_ThrowsArgumentOutOfRangeException()
        {
            Assert.Catch<ArgumentOutOfRangeException>(() => ELanguageRepository.Initialize((LanguageStorageType)(-1)));
        }

        #endregion

        #region Get Language

        [Test]
        public void GetLanguage_UnsetReader_ThrowsInvalidOperationException()
        {
            Assert.Catch<InvalidOperationException>(() => ELanguageRepository.GetLanguageFromFile(LanguageCode.EnglishUs));
        }

        [Test]
        public void GetLanguage_NewLanguage_AddedToDictionary()
        {
            ELanguageRepository.LanguageReader = new TestableELanguageReader
            {
                DictionaryToReturn = LanguageFactory.MakeLanguageDictionary(1)
            };
            ELanguageRepository.GetLanguageFromFile(LanguageCode.EnglishUs);
            ELanguageRepository.GetLanguageFromFile(LanguageCode.French);
            Assert.AreEqual(2, ELanguageRepository.Languages.Count);
        }

        [Test]
        public void GetLanguage_SameLanguage_NotAddedToDictionary()
        {
            ELanguageRepository.LanguageReader = new TestableELanguageReader
            {
                DictionaryToReturn = LanguageFactory.MakeLanguageDictionary(1)
            };
            ELanguageRepository.GetLanguageFromFile(LanguageCode.EnglishUs);
            ELanguageRepository.GetLanguageFromFile(LanguageCode.EnglishUs);
            Assert.AreEqual(1, ELanguageRepository.Languages.Count);
        }

        [Test]
        public void GetLanguage_SameLanguage_SameReference()
        {
            ELanguageRepository.LanguageReader = new TestableELanguageReader
            {
                DictionaryToReturn = LanguageFactory.MakeLanguageDictionary(1)
            };
            var originalELang = ELanguageRepository.GetLanguageFromFile(LanguageCode.EnglishUs);
            var newELang = ELanguageRepository.GetLanguageFromFile(LanguageCode.EnglishUs);
            Assert.AreSame(originalELang, newELang);
        }

        #endregion

        #region Terminate

        [Test]
        public void Terminate_ResetsState()
        {
            ELanguageRepository.LanguageReader = new TestableELanguageReader
            {
                DictionaryToReturn = LanguageFactory.MakeLanguageDictionary(1)
            };
            ELanguageRepository.GetLanguageFromFile(LanguageCode.EnglishUs);
            ELanguageRepository.Terminate();
            Assert.Zero(ELanguageRepository.Languages.Count);
            Assert.Null(ELanguageRepository.LanguageReader);
        }

        #endregion

        #endregion

        #region Properties



        #endregion
    }
}