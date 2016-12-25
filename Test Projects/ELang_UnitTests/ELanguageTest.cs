using System;
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

        [Test]
        public void FillDictionary_NullInputDictionary_ThrowsArgumentNullException()
        {
            var elanguage = LanguageFactory.MakeELanguage();
            Assert.Catch<ArgumentNullException>(() => elanguage.Dictionary = null);
        }

        #endregion

        #region Properties



        #endregion
    }
}