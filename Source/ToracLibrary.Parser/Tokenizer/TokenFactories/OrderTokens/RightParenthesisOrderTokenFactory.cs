﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Parser.Tokenizer.Tokens;
using ToracLibrary.Parser.Tokenizer.Tokens.OrderTokens;

namespace ToracLibrary.Parser.Tokenizer.TokenFactories.LiteralTokens
{

    /// <summary>
    /// Token factory for a right Parenthesis order token
    /// </summary>
    public class RightParenthesisOrderTokenFactory : ITokenFactory
    {

        #region Static Readonly Properties

        /// <summary>
        /// So we don't have to keep creating instances and keep memory down for a token that doesn't hold any data
        /// </summary>
        private readonly RightParenthesisOrderToken InstanceofToken = new RightParenthesisOrderToken();

        #endregion

        #region Public Methods

        public bool IsToken(char TokenToInspect, char? NextTokenPeekToInspect)
        {
            return TokenToInspect == ')';
        }

        public TokenBase CreateToken(StringReader Reader, char CurrentToken)
        {
            //return the plus token
            return InstanceofToken;
        }

        #endregion

    }

}
