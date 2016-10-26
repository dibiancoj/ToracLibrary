using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Parser.Tokenizer.Tokens.OrderTokens;

namespace ToracLibrary.Parser.Tokenizer.Tokens.OperatorTokens
{

    /// <summary>
    /// Token for a multiply operation. Complication is order of operation in formula. ie: 2+5*10. Need to multiple the 5*10 first. The ReversePolicyNotation solves that issue.
    /// </summary>
    public class MultiplyToken : OperatorBaseToken
    {

        /// <summary>
        /// Order of which this needs to be handled. ie: addition is before multiplication
        /// </summary>
        public override int OrderOfPresedence
        {
            get { return 13; }
        }

        /// <summary>
        /// Calculate the left hand and right hand side of equation
        /// </summary>
        /// <param name="LeftSide">Left hand side of equation</param>
        /// <param name="RightSide">Right hand side of the equation</param>
        /// <returns>Result of the operation</returns>
        public override double Calculate(double LeftSide, double RightSide)
        {
            return LeftSide * RightSide;
        }

    }

}
