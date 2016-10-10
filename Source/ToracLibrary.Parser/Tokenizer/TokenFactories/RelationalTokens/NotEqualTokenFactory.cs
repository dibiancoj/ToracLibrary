using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Parser.Tokenizer.Tokens;
using ToracLibrary.Parser.Tokenizer.Tokens.RelationalTokens;

namespace ToracLibrary.Parser.Tokenizer.TokenFactories.LiteralTokens
{

    /// <summary>
    /// Token factory for not equal
    /// </summary>
    public class NotEqualTokenFactory : ITokenFactory
    {

        #region Static Readonly Properties

        /// <summary>
        /// So we don't have to keep creating instances and keep memory down for a token that doesn't hold any data
        /// </summary>
        private readonly NotEqualToToken InstanceofToken = new NotEqualToToken();

        #endregion

        #region Public Methods

        public bool IsToken(char TokenToInspect, char? NextTokenPeekToInspect)
        {
            return (TokenToInspect == '<' && NextTokenPeekToInspect.GetValueOrDefault() == '>') ||
                   (TokenToInspect == '!' && NextTokenPeekToInspect.GetValueOrDefault() == '=');
        }

        public TokenBase CreateToken(StringReader Reader, char CurrentToken)
        {
            //since we are reading 2 characteres ">=". Then we need to read the "="
            Reader.Read();

            //return the instance
            return InstanceofToken;
        }

        #endregion

    }

}
