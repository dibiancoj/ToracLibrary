using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Parser.Tokenizer.Tokens.OperatorTokens
{

    /// <summary>
    /// Base token for an operator
    /// </summary>
    public abstract class OperatorBaseToken : TokenBase
    {

        /// <summary>
        /// Order of which this needs to be handled. ie: addition is before multiplication
        /// </summary>
        public abstract int OrderOfPresedence { get; }

        /// <summary>
        /// Calculate the left hand and right hand side of equation
        /// </summary>
        /// <param name="LeftSide">Left hand side of equation</param>
        /// <param name="RightSide">Right hand side of the equation</param>
        /// <returns>Result of the operation</returns>
        public abstract double Calculate(double LeftSide, double RightSide);
    }

}
