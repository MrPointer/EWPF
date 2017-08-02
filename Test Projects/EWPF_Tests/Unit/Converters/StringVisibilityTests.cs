using System.Globalization;
using System.Windows;
using EWPF.Converters;
using NUnit.Framework;

namespace EWPF_Tests.Unit.Converters
{
    [TestFixture]
    public class StringVisibilityTests
    {
        #region Events



        #endregion

        #region Fields



        #endregion

        #region Constructors



        #endregion

        #region Methods

        [Test]
        public void Convert_NullValue_ReturnsCollapsed()
        {
            var converter = new StringToVisibilityConverter();
            const Visibility expectedVisibility = Visibility.Collapsed;

            var visibility = converter.Convert(null, typeof(string), 
                null, CultureInfo.CurrentCulture);
            Assert.AreEqual(expectedVisibility, visibility);
        }

        [Test]
        public void Convert_EmptyStringValue_ReturnsCollapsed()
        {
            var converter = new StringToVisibilityConverter();
            const Visibility expectedVisibility = Visibility.Collapsed;

            var visibility = converter.Convert(string.Empty, typeof(string),
                null, CultureInfo.CurrentCulture);
            Assert.AreEqual(expectedVisibility, visibility);
        }

        [Test]
        public void Convert_ValidStringValue_ReturnsVisible()
        {
            var converter = new StringToVisibilityConverter();

            const Visibility expectedVisibility = Visibility.Visible;
            const string validString = "abc";

            var visibility = converter.Convert(validString, typeof(string),
                null, CultureInfo.CurrentCulture);
            Assert.AreEqual(expectedVisibility, visibility);
        }

        #endregion

        #region Properties



        #endregion
    }
}