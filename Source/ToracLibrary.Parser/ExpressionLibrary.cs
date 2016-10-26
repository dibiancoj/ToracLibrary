using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Parser.Parser;
using ToracLibrary.Parser.Tokenizer;
using ToracLibrary.Parser.Tokenizer.TokenFactories;
using ToracLibrary.Parser.Tokenizer.TokenFactories.LiteralTokens;
using ToracLibrary.Parser.Tokenizer.Tokens;

namespace ToracLibrary.Parser
{

    /// <summary>
    /// Is a parser. Starting out to build a number parser and will expand it little by little
    /// </summary>
    public class ExpressionLibrary
    {

        #region Math Parser

        /* Definition For Now */
        // Expression := Number {Operator Number}
        // Operator   := "+" | "-"
        // Number     := Digit{Digit}
        // Digit      := "0" | "1" | "2" | "3" | "4" | "5" | "6" | "7" | "8" | "9" 

        /// <summary>
        /// Holds the valid tokens for a number expression
        /// </summary>
        /// <remarks>Is set to internal so unit test can grab the values</remarks>
        internal static readonly ISet<ITokenFactory> ValidTokensForNumberExpression = new HashSet<ITokenFactory>(new ITokenFactory[]
        {
            new NumberLiteralTokenFactory(),
            new PlusOperatorTokenFactory(),
            new MinusOperatorTokenFactory(),
            new MultiplyOperatorTokenFactory(),
            new DivisionOperatorTokenFactory(),
            new LeftParenthesisOrderTokenFactory(),
            new RightParenthesisOrderTokenFactory()
        });

        /// <summary>
        /// Parses the given expression and return the result
        /// </summary>
        /// <param name="ExpressionToParse">Expression To Parse</param>
        /// <returns>The calculated value</returns>
        public static double ParseAndEvaluateNumberExpression(string ExpressionToParse)
        {
            //go tokenize this thing. Convert the string to tokens
            var TokensFoundInExpression = GenericTokenizer.ScanLazy(ExpressionToParse, ValidTokensForNumberExpression);

            //Build out the parser and return the result
            //return PlusMinusParser.Parse(TokensFoundInExpression, SupportedTokens);
            var ReversePolishTokenResult = ReversePolishMathNotationParser.ConvertToReversePolishNotationLazy(TokensFoundInExpression);

            //go grab the result and return it
            return ReversePolishMathNotationParser.EvaluateExpression(ReversePolishTokenResult);
        }

        #endregion

        #region Logical Or Relational Parser

        /// <summary>
        /// hold all the valid token factories for relational - logical parsers
        /// </summary>
        /// <remarks>Is set to internal so unit test can grab the values</remarks>
        internal static readonly ISet<ITokenFactory> ValidRelationalTokens = new HashSet<ITokenFactory>(new ITokenFactory[]
            {
                new NumberLiteralTokenFactory(),
                new EqualTokenFactory(),
                new NotEqualTokenFactory(),
                new GreaterThanTokenFactory(),
                new GreaterThanOrEqualTokenFactory(),
                new LessThanOrEqualTokenFactory(),
                new LessThanTokenFactory()
            });

        /// <summary>
        /// Turn the given expression into a set of tokens for this relational - logical parser
        /// </summary>
        /// <param name="ExpressionToTokenize">Expression to turn into a list of tokens</param>
        /// <returns>Tokenized for the given expression</returns>
        public static IEnumerable<TokenBase> TokenizedRelationalExpressionLazy(string ExpressionToTokenize)
        {
            return GenericTokenizer.ScanLazy(ExpressionToTokenize, ValidRelationalTokens);
        }

        #endregion

    }

}
