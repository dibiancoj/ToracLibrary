using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Parser.Parser;
using ToracLibrary.Parser.Tokenizer;

namespace ToracLibrary.Parser
{

    /// <summary>
    /// Is a parser. Starting out to build a number parser and will expand it little by little
    /// </summary>
    public class ExpressionLibrary
    {

        /* Definition For Now */
        // Expression := Number {Operator Number}
        // Operator   := "+" | "-"
        // Number     := Digit{Digit}
        // Digit      := "0" | "1" | "2" | "3" | "4" | "5" | "6" | "7" | "8" | "9" 

        /// <summary>
        /// Parses the given expression and return the result
        /// </summary>
        /// <param name="ExpressionToParse">Expression To Parse</param>
        /// <returns>The calculated value</returns>
        public static int ParseNumberExpression(string ExpressionToParse)
        {
            //go tokenize this thing. Convert the string to tokens
            var TokensFoundInExpression = PlusMinusTokenizer.Scan(ExpressionToParse);

            //Build out the parser and return the result
            return PlusMinusParser.Parse(TokensFoundInExpression);
        }

    }

}
