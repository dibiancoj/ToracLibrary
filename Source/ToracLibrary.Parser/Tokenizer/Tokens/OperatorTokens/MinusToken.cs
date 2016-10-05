﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Parser.Tokenizer.Tokens.OperatorTokens
{

    /// <summary>
    ///  Token for the a minus operation
    /// </summary>
    public class MinusToken : OperatorBaseToken
    {

        /// <summary>
        /// Calculate the left hand and right hand side of equation
        /// </summary>
        /// <param name="LeftSide">Left hand side of equation</param>
        /// <param name="RightSide">Right hand side of the equation</param>
        /// <returns>Result of the operation</returns>
        public override int Calculate(int LeftSide, int RightSide)
        {
            return LeftSide - RightSide;
        }

    }

}
