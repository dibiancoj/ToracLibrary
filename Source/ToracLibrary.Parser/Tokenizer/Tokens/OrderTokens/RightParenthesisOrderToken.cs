using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Parser.Tokenizer.Tokens.OrderTokens
{

    /// <summary>
    /// Token for a right Parenthesis
    /// </summary>
    public class RightParenthesisOrderToken : OrderBaseToken
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
