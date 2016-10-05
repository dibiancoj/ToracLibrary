using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Parser.Tokenizer.Tokens.OperatorTokens;

namespace ToracLibrary.Parser.Exceptions
{

    /// <summary>
    /// Exception when we don't know the specified operator
    /// </summary>
    public class UnsupportedOperatorException : Exception
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="InvalidOperatorTokenToSet">Operator token that is not supported</param>
        /// <param name="SupportedOperatorTokensToSet">Supported operator tokens</param>
        public UnsupportedOperatorException(OperatorBaseToken InvalidOperatorTokenToSet, IEnumerable<OperatorBaseToken> SupportedOperatorTokensToSet)
        {
            InvalidOperatorToken = InvalidOperatorTokenToSet;
            SupportedOperatorTokens = SupportedOperatorTokensToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Operator token that is not supported
        /// </summary>
        public OperatorBaseToken InvalidOperatorToken { get; }

        /// <summary>
        /// Supported Operator TOkens
        /// </summary>
        public IEnumerable<OperatorBaseToken> SupportedOperatorTokens { get; }

        #endregion

    }

}
