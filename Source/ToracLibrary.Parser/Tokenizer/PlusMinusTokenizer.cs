using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Parser.Tokenizer.Tokens;
using ToracLibrary.Parser.Tokenizer.Tokens.OperatorTokens;

namespace ToracLibrary.Parser.Tokenizer
{

    /// <summary>
    /// Converts a raw string into tokens which can be parsed
    /// </summary>
    public class PlusMinusTokenizer
    {

        /// <summary>
        /// Converts the expression to all the tokens that make up the expression
        /// </summary>
        /// <param name="ExpressionToScan">Expression to scan and return all the tokens</param>
        /// <returns>list of tokens that make up the expression</returns>
        public IEnumerable<TokenBase> Scan(string ExpressionToScan)
        {
            //since we are going to dispose of the reader lets build up a list and return it (for now)
            var TokensFound = new List<TokenBase>();

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
                        TokensFound.Add(new NumberConstantToken(ParseNumber(ExpressionReader)));
                    }
                    else if (CharacterPeekedAt == '-')
                    {
                        //return the minus token
                        TokensFound.Add(new MinusToken());

                        //move the reader to the next character
                        ExpressionReader.Read();
                    }
                    else if (CharacterPeekedAt == '+')
                    {
                        //return the plus token
                        TokensFound.Add(new PlusToken());

                        //move the reader
                        ExpressionReader.Read();
                    }
                    else
                    {
                        throw new Exception("Unknown character in expression: " + CharacterPeekedAt);
                    }
                }
            }

            //return the list of tokens
            return TokensFound;
        }

        /// <summary>
        /// Grab the entire number and parse id
        /// </summary>
        /// <param name="Reader">Reader To Continue Reading</param>
        /// <returns>The Parsed Number</returns>
        private int ParseNumber(StringReader Reader)
        {
            //digits found
            var DigitsFound = new List<int>();

            //keep reading until we don't have a digit
            while (Char.IsDigit((char)Reader.Peek()))
            {
                //grab the digit we just read
                var DigitRead = (char)Reader.Read();

                //try to parse that number
                int TryParseNumber;

                //can we parse this number
                if (int.TryParse(Char.ToString(DigitRead), out TryParseNumber))
                {
                    //add the number to the list because we were able to parse it
                    DigitsFound.Add(TryParseNumber);
                }
                else
                {
                    //why would we get here?
                    throw new Exception("Could not parse integer number when parsing digit: " + DigitRead);
                }
            }


            Add this to a extension method for ienumerable<int>...could be reused...don't use reverse as it alters the list...

            //this magical math just makes everything in the array 
            //ie: [0] = 2, [1] = 3 => 23
            var RunningTally = 0;

            //what we multiple with
            var MultiplyValue = 1;

            //reverse it and then loop through it
            DigitsFound.Reverse();

            //loop through all of them and multiple everything
            foreach (var Digit in DigitsFound)
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
