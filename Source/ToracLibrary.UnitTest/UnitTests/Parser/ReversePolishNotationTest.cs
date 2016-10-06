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
using ToracLibrary.Parser.Tokenizer.Tokens.OperatorTokens;
using Xunit;

namespace ToracLibrary.UnitTest.Serialization
{

    /// <summary>
    /// Unit test for the reverse polish notation parser
    /// </summary>
    public class ReversePolishNotationTest
    {

        #region Framework

        /// <summary>
        /// Convert the tokens back to compare it to a string
        /// </summary>
        /// <param name="TokensToValidate">Tokens to validate</param>
        /// <returns>string that represents the tokens</returns>
        private static string ConvertToString(IEnumerable<TokenBase> TokensToValidate)
        {
            var Result = new StringBuilder();

            foreach (var Token in TokensToValidate)
            {
                if (Token is NumberLiteralToken)
                {
                    Result.Append(((NumberLiteralToken)Token).Value);
                }
                else if (Token is MinusToken)
                {
                    Result.Append("-");
                }
                else if (Token is PlusToken)
                {
                    Result.Append("+");
                }
                else if (Token is MultiplyToken)
                {
                    Result.Append("*");
                }

                Result.Append(" ");
            }

            Result.Remove(Result.Length - 1, 1);

            return Result.ToString();
        }

        /// <summary>
        /// Convert the given expression to a token set
        /// </summary>
        /// <param name="Expression">Expression to convert</param>
        /// <returns>Tokenized</returns>
        private static IEnumerable<TokenBase> ConvertToToken(string Expression)
        {
            //supported tokens
            var SupportedTokens = new HashSet<ITokenFactory>(new ITokenFactory[] { new NumberLiteralTokenFactory(), new PlusOperatorTokenFactory(), new MinusOperatorTokenFactory(), new MultiplyOperatorTokenFactory() });

            //go tokenize this thing. Convert the string to tokens
            return GenericTokenizer.ScanLazy(Expression, SupportedTokens);
        }

        #endregion

        #region Unit Tests

        [InlineData("2 + 3 - 2 * 10", "2 3 + 2 10 * -")]
        [InlineData("2 + 3", "2 3 +")]
        [InlineData("2 + 3 * 7", "2 3 7 * +")]
        [InlineData("2+5*9+1*3", "2 5 9 * + 1 3 * +")]
        [Theory]
        public void ReversePolishNotationTest1(string ExpressionToTest, string ExpectedResultOfExpression)
        {
           Assert.Equal(ExpectedResultOfExpression, ConvertToString(ReversePolishMathNotationParser.ConvertToReversePolishNotationLazy(ConvertToToken(ExpressionToTest).ToArray()).ToArray()));
        }

        #endregion

    }

}
