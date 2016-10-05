using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Parser.Tokenizer.Tokens
{

    /// <summary>
    /// Holds a number literal token
    /// </summary>
    public class NumberLiteralToken : TokenBase
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ValueToSet">Value to set</param>
        public NumberLiteralToken(int ValueToSet)
        {
            Value = ValueToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Value of the constant
        /// </summary>
        public int Value { get; }

        #endregion

    }

}
