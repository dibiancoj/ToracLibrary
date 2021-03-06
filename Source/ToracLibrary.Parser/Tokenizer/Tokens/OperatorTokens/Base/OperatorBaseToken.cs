﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Parser.Tokenizer.Tokens.OrderTokens;

namespace ToracLibrary.Parser.Tokenizer.Tokens.OperatorTokens
{

    /// <summary>
    /// Base token for an operator
    /// </summary>
    public abstract class OperatorBaseToken : OrderBaseToken
    {

        /// <summary>
        /// Calculate the left hand and right hand side of equation
        /// </summary>
        /// <param name="LeftSide">Left hand side of equation</param>
        /// <param name="RightSide">Right hand side of the equation</param>
        /// <returns>Result of the operation</returns>
        public abstract double Calculate(double LeftSide, double RightSide);

    }

}
