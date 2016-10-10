using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Parser.Exceptions;
using ToracLibrary.Parser.Tokenizer.TokenFactories;
using ToracLibrary.Parser.Tokenizer.Tokens;

namespace ToracLibrary.Parser.Tokenizer
{

    /// <summary>
    /// Converts a raw string into tokens which can be parsed
    /// </summary>
    public static class GenericTokenizer
    {

        /// <summary>
        /// Converts the expression to all the tokens that make up the expression
        /// </summary>
        /// <param name="ExpressionToScan">Expression to scan and return all the tokens</param>
        /// <param name="ValidTokens">Valid tokens for this expression type</param>
        /// <returns>list of tokens that make up the expression</returns>
        public static IEnumerable<TokenBase> ScanLazy(string ExpressionToScan, ISet<ITokenFactory> ValidTokens)
        {
            //declare the string reader
            using (var ExpressionReader = new StringReader(ExpressionToScan))
            {
                //keep reading until we are done
                while (HaveMoreCharacters(ExpressionReader))
                {
                    //what character is this?
                    var CharacterPeekedAt = (char)ExpressionReader.Read();

                    //is this a whitespace character
                    if (char.IsWhiteSpace(CharacterPeekedAt))
                    {
                        //this is a space...go to the next character
                        continue;
                    }

                    //next character for items that are multi character (we only support 2 characters now such as >=)
                    char? NextCharacterPeek = null; //closest thing to a null character

                    //do we have another character
                    if (HaveMoreCharacters(ExpressionReader))
                    {
                        //we have a character
                        NextCharacterPeek = (char)ExpressionReader.Peek();
                    }

                    //grab the first token this is valid for
                    var FirstValidToken = ValidTokens.FirstOrDefault(x => x.IsToken(CharacterPeekedAt, NextCharacterPeek));

                    //did we find a valid token
                    if (FirstValidToken == null)
                    {
                        //can't find a token for this
                        throw new ParserUnknownCharacterException(CharacterPeekedAt);
                    }

                    //we have a valid token
                    yield return FirstValidToken.CreateToken(ExpressionReader, CharacterPeekedAt);
                }
            }
        }

        /// <summary>
        /// Do we have more characters to consume
        /// </summary>
        /// <param name="Reader">Reader to check</param>
        /// <returns>if we have more characters</returns>
        private static bool HaveMoreCharacters(StringReader Reader)
        {
            //what is the peek result. Is it negative 1
            return Reader.Peek() != -1;
        }

    }

}
