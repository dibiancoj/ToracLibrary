using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Parser.Tokenizer.Tokens.OrderTokens
{

    /// <summary>
    /// Base class for order based tokens
    /// </summary>
    public abstract class OrderBaseToken : TokenBase
    {

        /// <summary>
        /// Interface method
        /// </summary>
        public abstract int OrderOfPresedence { get; }

    }

}
