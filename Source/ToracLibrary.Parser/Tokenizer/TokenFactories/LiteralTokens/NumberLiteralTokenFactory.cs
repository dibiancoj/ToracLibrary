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

        #region Public Methods

        public bool IsToken(char TokenToInspect, char? NextTokenPeekToInspect)
        {
            return char.IsDigit(TokenToInspect);
        }

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
        private static int ParseNumber(StringReader Reader, char CurrentToken)
        {
            //digits found
            var DigitsFound = new List<int>();

            //add the current token
            DigitsFound.Add(int.Parse(char.ToString(CurrentToken)));

            //keep reading until we don't have a digit
            while (char.IsDigit((char)Reader.Peek()))
            {
                //grab the digit we just read
                var DigitRead = (char)Reader.Read();

                //try to parse that number
                int TryParseNumber;

                //can we parse this number
                if (int.TryParse(char.ToString(DigitRead), out TryParseNumber))
                {
                    //add the number to the list because we were able to parse it
                    DigitsFound.Add(TryParseNumber);
                }
                else
                {
                    //was not able to parse this value for some reason. Not really sure how we got here but we will leave it
                    throw new InvalidCastException("Could Not Parse Integer Number When Parsing Digit: " + DigitRead);
                }
            }

            //at this point we have an array of char's (single digit numbers). we want to combine them to form a number
            //ie:
            //[0] = 3
            //[1] = 5
            //[2] = 7
            // ==> 357
            return ArrayOfSingleDigitsToNumber(DigitsFound);
        }

        /// <summary>
        /// Convert a list of digits into a number.
        /// </summary>
        /// <param name="DigitsToConvertToANumber">Digits to convert to a number</param>
        /// <returns>The combined number</returns>
        private static int ArrayOfSingleDigitsToNumber(IEnumerable<int> DigitsToConvertToANumber)
        {
            //ie:
            //[0] = 3
            //[1] = 5
            //[2] = 7
            // ==> 357

            //tally
            var RunningTally = 0;

            //what we multiple with
            var MultiplyValue = 1;

            //loop through all of them and multiple everything
            foreach (var Digit in DigitsToConvertToANumber.Reverse())
            {
                //multiply and add it
                RunningTally += Digit * MultiplyValue;

                //now multiple by 10
                MultiplyValue *= 10;
            }

            //return the result
            return RunningTally;
        }

        #endregion

    }

}
