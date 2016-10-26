using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Parser.Tokenizer.Tokens;

namespace ToracLibrary.Parser.Tokenizer.TokenFactories
{

    /// <summary>
    /// Interface so we can abstract how tokens are collected so we don't need to hard code everything
    /// </summary>
    public interface ITokenFactory
    {

        /// <summary>
        /// Is this an instance of the current token?
        /// </summary>
        /// <param name="TokenToInspect">Token to inspect if this is the instance of that token</param>
        /// <param name="NextTokenPeekToInspect">The 2nd character to inspect. Could be null if we don't have any more characters</param>
        /// <returns>If its an instance of this token</returns>
        bool IsToken(char TokenToInspect, char? NextTokenPeekToInspect);

        /// <summary>
        /// Create an instance of the specified token after IsToken returns true
        /// </summary>
        /// <param name="Reader">Reader to use to navigate the stream</param>
        /// <param name="CurrentToken">Current token we are up to</param>
        /// <returns>The create instance of the token</returns>
        TokenBase CreateToken(StringReader Reader, char CurrentToken);

    }

}
