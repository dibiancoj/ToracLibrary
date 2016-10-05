using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Parser.Tokenizer.Tokens;

namespace ToracLibrary.Parser.Exceptions
{

    /// <summary>
    /// Expecting a specific token but got a different token
    /// </summary>
    public class ExpectingTokenException : Exception
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ExpectedTokenTypeToSet">The type of token we are expecting</param>
        /// <param name="TokenFoundToSet">The type of token we found</param>
        public ExpectingTokenException(Type ExpectedTokenTypeToSet, TokenBase TokenFoundToSet)
        {
            ExpectedTokenType = ExpectedTokenTypeToSet;
            TokenFound = TokenFoundToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The type of token we are expecting
        /// </summary>
        public Type ExpectedTokenType { get; }

        /// <summary>
        /// The type of token we found
        /// </summary>
        public TokenBase TokenFound { get; }

        #endregion

        #region Methods

        /// <summary>
        /// ToString override
        /// </summary>
        /// <returns>ToString Override Value</returns>
        public override string ToString()
        {
            return string.Format("Expecting {0} After Expression, But Got {1}", ExpectedTokenType.Name, TokenFound.GetType().Name);
        }

        #endregion

    }

}
