﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Parser.Tokenizer.Tokens.OrderTokens
{

    /// <summary>
    /// Token for a left Parenthesis
    /// </summary>
    public class LeftParenthesisOrderToken : OrderBaseToken
    {
        public override int OrderOfPresedence
        {
            get
            {
                return 0;
            }
        }
    }

}
