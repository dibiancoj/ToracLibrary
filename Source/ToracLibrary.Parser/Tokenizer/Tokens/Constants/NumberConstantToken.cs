using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Parser.Tokenizer.Tokens
{

    /// <summary>
    /// Holds a number constant token
    /// </summary>
    public class NumberConstantToken : TokenBase
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ValueToSet">Value to set</param>
        public NumberConstantToken(int ValueToSet)
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
