using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Parser.Exceptions
{

    /// <summary>
    /// Expecting a specific Parenthesis missing
    /// </summary>
    public class MissingParenthesisException : Exception
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="TypeOfParenthesisMissingToSet">The type of parenthesis missing</param>
        public MissingParenthesisException(string TypeOfParenthesisMissingToSet)
        {
            TypeOfParenthesisMissing = TypeOfParenthesisMissingToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The type of token we are expecting
        /// </summary>
        public string TypeOfParenthesisMissing { get; }

        #endregion

        #region Methods

        /// <summary>
        /// ToString override
        /// </summary>
        /// <returns>ToString Override Value</returns>
        public override string ToString()
        {
            return "Missing Parenthesis. Please Verify Syntax: " + TypeOfParenthesisMissing;
        }

        #endregion

    }

}
