using System;
using System.Globalization;
using EWPF.Converters;
using NUnit.Framework;

namespace EWPF_Tests.Unit.Converters
{
    [TestFixture]
    public class InverseBoolTests
    {
        #region Events



        #endregion

        #region Fields



        #endregion

        #region Constructors



        #endregion

        #region Methods

        [Test]
        public void Convert_NullValue_ThrowsArgumentNullException()
        {
            var inverseBoolConverter = new InverseBoolConverter();
            Assert.Catch<ArgumentNullException>(
                () => inverseBoolConverter.Convert(null, typeof(bool), 
                null, CultureInfo.CurrentUICulture));
        }

        [Test]
        public void Convert_TrueValue_ReturnsFalse()
        {
            var inverseBoolConverter = new InverseBoolConverter();
            var convertedObject = inverseBoolConverter.Convert(true, typeof(bool),
                null, CultureInfo.CurrentCulture);
            bool convertedValue = convertedObject != null && (bool) convertedObject;
            Assert.False(convertedValue);
        }

        #endregion

        #region Properties



#endregion
    }
}