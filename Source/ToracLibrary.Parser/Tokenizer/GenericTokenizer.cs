using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Parser.Exceptions;
using ToracLibrary.Parser.Tokenizer.TokenFactories;
using ToracLibrary.Parser.Tokenizer.Tokens;
using ToracLibrary.Parser.Tokenizer.Tokens.OperatorTokens;

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

                    //grab the first token this is valid for
                    var FirstValidToken = ValidTokens.FirstOrDefault(x => x.IsToken(CharacterPeekedAt));

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

    }

}
