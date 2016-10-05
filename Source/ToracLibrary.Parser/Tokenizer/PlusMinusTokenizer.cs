using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Parser.Exceptions;
using ToracLibrary.Parser.Tokenizer.Tokens;
using ToracLibrary.Parser.Tokenizer.Tokens.OperatorTokens;

namespace ToracLibrary.Parser.Tokenizer
{

    /// <summary>
    /// Converts a raw string into tokens which can be parsed
    /// </summary>
    public static class PlusMinusTokenizer
    {

        /// <summary>
        /// Converts the expression to all the tokens that make up the expression
        /// </summary>
        /// <param name="ExpressionToScan">Expression to scan and return all the tokens</param>
        /// <returns>list of tokens that make up the expression</returns>
        public static IEnumerable<TokenBase> ScanLazy(string ExpressionToScan)
        {
            //declare the string reader
            using (var ExpressionReader = new StringReader(ExpressionToScan))
            {
                //keep reading until we are done
                while (ExpressionReader.Peek() != -1)
                {
                    //what character is this?
                    var CharacterPeekedAt = (char)ExpressionReader.Peek();

                    //is this a whitespace character
                    if (char.IsWhiteSpace(CharacterPeekedAt))
                    {
                        //we don't care about white spaces...keep going
                        ExpressionReader.Read();

                        //go to the next character
                        continue;
                    }

                    //is this a digit?
                    if (char.IsDigit(CharacterPeekedAt))
                    {
                        //it's a digit...try to parse the number
                       yield return new NumberConstantToken(ParseNumber(ExpressionReader));
                    }
                    else if (CharacterPeekedAt == '-')
                    {
                        //return the minus token
                        yield return new MinusToken();

                        //move the reader to the next character
                        ExpressionReader.Read();
                    }
                    else if (CharacterPeekedAt == '+')
                    {
                        //return the plus token
                        yield return new PlusToken();

                        //move the reader
                        ExpressionReader.Read();
                    }
                    else
                    {
                        throw new ParserUnknownCharacterException(CharacterPeekedAt);
                    }
                }
            }
        }

        /// <summary>
        /// Grab the entire number and parse id
        /// </summary>
        /// <param name="Reader">Reader To Continue Reading</param>
        /// <returns>The Parsed Number</returns>
        private static int ParseNumber(StringReader Reader)
        {
            //digits found
            var DigitsFound = new List<int>();

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

    }

}
