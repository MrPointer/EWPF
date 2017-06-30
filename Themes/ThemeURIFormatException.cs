using System;
using System.Runtime.Serialization;

namespace EWPF.Themes
{
    /// <summary>
    /// A class representing an exception raised when a <see cref="string"/> that doesn't match 
    /// the <see cref="ThemeUri.THEME_URI_REGEX_PATTERN"/> is being used for a WPF-Theme context.
    /// </summary>
    public class ThemeUriFormatException : UriFormatException
    {
        #region Events



        #endregion

        #region Fields



        #endregion

        #region Constructors

        /// <inheritdoc />
        public ThemeUriFormatException()
        {
        }

        /// <inheritdoc />
        public ThemeUriFormatException(string textString) : base(textString)
        {
        }

        /// <inheritdoc />
        public ThemeUriFormatException(string textString, Exception e) : base(textString, e)
        {
        }

        /// <inheritdoc />
        protected ThemeUriFormatException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }

        #endregion

        #region Methods



        #endregion

        #region Properties



#endregion
    }
}