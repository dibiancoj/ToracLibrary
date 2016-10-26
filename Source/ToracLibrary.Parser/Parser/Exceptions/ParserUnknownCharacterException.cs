using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Parser.Exceptions
{

    /// <summary>
    /// Exception when the character passed in is not supported
    /// </summary>
    public class ParserUnknownCharacterException : Exception
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="CharacterThatIsUnknown">Character that the parser found that is not known</param>
        public ParserUnknownCharacterException(char CharacterThatIsUnknown)
        {
            UnknownCharacter = CharacterThatIsUnknown;
        }

        #endregion

        #region Properties

        /// <summary>
        /// What is the unknown character
        /// </summary>
        public char UnknownCharacter { get; }

        #endregion

        #region Methods

        /// <summary>
        /// ToString Override
        /// </summary>
        /// <returns>ToString Override Value</returns>
        public override string ToString()
        {
            return "Unknown Character In Expression: " + UnknownCharacter;
        }

        #endregion

    }

}
