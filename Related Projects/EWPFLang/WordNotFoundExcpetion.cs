﻿using System;

namespace EWPFLang
{
    /// <summary>
    /// A class representing an exception thrown when trying to get a word that doesn't exist in a dictionary for some reason.
    /// </summary>
    public class WordNotFoundExcpetion : Exception
    {
        #region Events



        #endregion

        #region Fields



        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="T:System.Exception" /> class with a specified error message.</summary>
        /// <param name="i_Message">The message that describes the error. </param>
        /// <param name="i_RequestedWord">Word that has been requested to find.</param>
        public WordNotFoundExcpetion(string i_Message, DictionaryCode i_RequestedWord)
            : base(i_Message)
        {
            RequestedWord = i_RequestedWord;
        }

        #endregion

        #region Methods



        #endregion

        #region Properties

        /// <summary>
        /// Gets the word that has been requested to find but not found.
        /// </summary>
        public DictionaryCode RequestedWord { get; private set; }

        #endregion
    }
}