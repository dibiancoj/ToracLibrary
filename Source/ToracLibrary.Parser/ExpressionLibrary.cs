using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Parser.Parser;
using ToracLibrary.Parser.Tokenizer;
using ToracLibrary.Parser.Tokenizer.TokenFactories;
using ToracLibrary.Parser.Tokenizer.TokenFactories.LiteralTokens;

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
        public static double ParseNumberExpression(string ExpressionToParse)
        {
            //supported tokens
            var SupportedTokens = new HashSet<ITokenFactory>(new ITokenFactory[] { new NumberLiteralTokenFactory(), new PlusOperatorTokenFactory(), new MinusOperatorTokenFactory(), new MultiplyOperatorTokenFactory() });

            //go tokenize this thing. Convert the string to tokens
            var TokensFoundInExpression = GenericTokenizer.ScanLazy(ExpressionToParse, SupportedTokens);

            //Build out the parser and return the result
            //return PlusMinusParser.Parse(TokensFoundInExpression, SupportedTokens);
            var ReversePolishTokenResult = ReversePolishNotationParser.ConvertToReversePolishNotationLazy(TokensFoundInExpression);

            return -1;
        }

    }

}
