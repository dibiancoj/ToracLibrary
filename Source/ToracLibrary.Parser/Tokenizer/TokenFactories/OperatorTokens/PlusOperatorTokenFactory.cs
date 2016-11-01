using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Parser.Tokenizer.Tokens;
using ToracLibrary.Parser.Tokenizer.Tokens.OperatorTokens;

namespace ToracLibrary.Parser.Tokenizer.TokenFactories.LiteralTokens
{

    /// <summary>
    /// Token factory for a plus operator token
    /// </summary>
    public class PlusOperatorTokenFactory : ITokenFactory
    {

        #region Static Readonly Properties

        /// <summary>
        /// So we don't have to keep creating instances and keep memory down for a token that doesn't hold any data
        /// </summary>
        private readonly PlusToken InstanceofToken = new PlusToken();

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
            return TokenToInspect == '+';
        }

        /// <summary>
        /// Create an instance of the specified token after IsToken returns true
        /// </summary>
        /// <param name="Reader">Reader to use to navigate the stream</param>
        /// <param name="CurrentToken">Current token we are up to</param>
        /// <returns>The create instance of the token</returns>
        public TokenBase CreateToken(StringReader Reader, char CurrentToken)
        {
            //return the plus token
            return InstanceofToken;
        }

        #endregion

    }

}
