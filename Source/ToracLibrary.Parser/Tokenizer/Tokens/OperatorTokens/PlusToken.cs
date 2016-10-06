using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Parser.Tokenizer.Tokens.OrderTokens;

namespace ToracLibrary.Parser.Tokenizer.Tokens.OperatorTokens
{

    /// <summary>
    /// Token for the an addition operation
    /// </summary>
    public class PlusToken : OperatorBaseToken
    {

        /// <summary>
        /// Order of which this needs to be handled. ie: addition is before multiplication
        /// </summary>
        public override int OrderOfPresedence
        {
            get { return 12; }
        }

        /// <summary>
        /// Calculate the left hand and right hand side of equation
        /// </summary>
        /// <param name="LeftSide">Left hand side of equation</param>
        /// <param name="RightSide">Right hand side of the equation</param>
        /// <returns>Result of the operation</returns>
        public override double Calculate(double LeftSide, double RightSide)
        {
            return LeftSide + RightSide;
        }

    }

}
