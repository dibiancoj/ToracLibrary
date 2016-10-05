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
    /// Token factory for a multiply operator token
    /// </summary>
    public class MultiplyOperatorTokenFactory : ITokenFactory
    {

        #region Public Methods

        public bool IsToken(char TokenToInspect)
        {
            return TokenToInspect == '*';
        }

        public TokenBase CreateToken(StringReader Reader, char CurrentToken)
        {
            //read the token...then return the object
            Reader.Read();

            //return the plus token
            return new MultiplyToken();
        }

        #endregion

    }

}
