using System;

namespace EWPFLang
{
    /// <summary>
    /// A class representing an exception that's thrown when an error occurs during the parsing of a language file.
    /// </summary>
    public class LanguageParseException : Exception
    {
        #region Events



        #endregion

        #region Fields



        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="T:System.Exception" /> class with a specified error message.</summary>
        /// <param name="i_Message">The message that describes the error. </param>
        public LanguageParseException(string i_Message, string i_ExpectedCode)
            : base(i_Message)
        {
            ExpectedDictionaryCode = i_ExpectedCode;
        }

        #endregion

        #region Methods



        #endregion

        #region Properties

        /// <summary>
        /// Gets the expected dictionary code that caused the parsing error as a string, as it appears in the file.
        /// </summary>
        public string ExpectedDictionaryCode { get; private set; }

        #endregion
    }
}