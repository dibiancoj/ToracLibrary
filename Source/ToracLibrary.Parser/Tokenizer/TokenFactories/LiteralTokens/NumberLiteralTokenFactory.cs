using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Parser.Tokenizer.Tokens;

namespace ToracLibrary.Parser.Tokenizer.TokenFactories.LiteralTokens
{

    /// <summary>
    /// Token factory for a number literal
    /// </summary>
    public class NumberLiteralTokenFactory : ITokenFactory
    {

        #region Constants

        /// <summary>
        /// Holds a decimal operator
        /// </summary>
        private const char DecimalOperator = '.';

        #endregion

        #region Public Methods

        /// <summary>
        /// Is this an instance of the current token?
        /// </summary>
        /// <param name="TokenToInspect">Token to inspect if this is the instance of that token</param>
        /// <param name="NextTokenPeekToInspect">The 2nd character to inspect. Could be null if we don't have any more characters</param>
        /// <returns>If its an instance of this token</returns>
        public bool IsToken(char TokenToInspect, char? NextTokenPeekToInspect)
        {
            //allow decimals - use the method so we can share it with parse number
            return IsValidLiteralNumber(TokenToInspect);
        }

        /// <summary>
        /// Create an instance of the specified token after IsToken returns true
        /// </summary>
        /// <param name="Reader">Reader to use to navigate the stream</param>
        /// <param name="CurrentToken">Current token we are up to</param>
        /// <returns>The create instance of the token</returns>
        public TokenBase CreateToken(StringReader Reader, char CurrentToken)
        {
            //go parse the number and return the literal
            return new NumberLiteralToken(ParseNumber(Reader, CurrentToken));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Grab the entire number and parse id
        /// </summary>
        /// <param name="Reader">Reader To Continue Reading</param>
        /// <param name="CurrentToken">Current token which was read</param>
        /// <returns>The Parsed Number</returns>
        private static double ParseNumber(StringReader Reader, char CurrentToken)
        {
            //digits found
            var DigitsFound = new StringBuilder();

            //add the current token
            DigitsFound.Append(char.ToString(CurrentToken));

            //keep reading until we don't have a digit
            while (IsValidLiteralNumber((char)Reader.Peek()))
            {
                //grab the digit we just read
                var DigitRead = (char)Reader.Read();

                //add the item to the string builder
                DigitsFound.Append(DigitRead);
            }

            //now let's try to parse the string value ("123.567");
            double TryParseAttempt;

            //try to parse this
            if (double.TryParse(DigitsFound.ToString(), out TryParseAttempt))
            {
                //we were able to parse this, return it
                return TryParseAttempt;
            }

            //can't parse into a double...throw an exception
            throw new InvalidCastException("Can't Parse The Following Into A Double: " + DigitsFound.ToString());
        }

        /// <summary>
        /// This this a valid literal character
        /// </summary>
        /// <param name="CharacterToTest">Character to test</param>
        /// <returns>Yes if this is valid</returns>
        private static bool IsValidLiteralNumber(char CharacterToTest)
        {
            //either a decimal or a digit
            return CharacterToTest == DecimalOperator || char.IsDigit(CharacterToTest);
        }

        #endregion

    }

}
