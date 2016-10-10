using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Parser;
using ToracLibrary.Parser.Exceptions;
using ToracLibrary.Parser.Tokenizer;
using ToracLibrary.Parser.Tokenizer.TokenFactories;
using ToracLibrary.Parser.Tokenizer.TokenFactories.LiteralTokens;
using ToracLibrary.Parser.Tokenizer.Tokens;
using ToracLibrary.Parser.Tokenizer.Tokens.RelationalTokens;
using Xunit;

namespace ToracLibrary.UnitTest.Serialization
{

    /// <summary>
    /// Unit test for parsing values
    /// </summary>
    public class ParserTest
    {

        #region Mathematical Parser 

        /// <summary>
        /// Test a valid expression and its result
        /// </summary>
        /// <param name="ExpressionToTest">Expression to test</param>
        /// <param name="ExpectedResultOfExpression">Expected result of the expression</param>
        [InlineData("1 +   20", 1 + 20)]
        [InlineData("1 - 10", 1 - 10)]
        [InlineData("10 - 1", 10 - 1)]
        [InlineData("2+4+7", 2 + 4 + 7)]
        [InlineData("2+4+7- 2", 2 + 4 + 7 - 2)]
        [InlineData("10/2", 10 / 2)]
        [InlineData("10/2 * 3 + 2", 10 / 2 * 3 + 2)]
        [InlineData("10 * (1+2) * 5", 10 * (1 + 2) * 5)]
        [Theory]
        public void MathParserTest1(string ExpressionToTest, int ExpectedResultOfExpression)
        {
            Assert.Equal(ExpectedResultOfExpression, ExpressionLibrary.ParseAndEvaluateNumberExpression(ExpressionToTest));
        }

        /// <summary>
        /// Test a valid expression and its result
        /// </summary>
        /// <param name="ExpressionToTest">Expression to test</param>
        /// <param name="ExpectedResultOfExpression">Expected result of the expression</param>
        [InlineData("2+7*8", 2 + 7 * 8)]
        [InlineData("8*7", 56)]
        [InlineData("2+5*9+1*3", 2 + 5 * 9 + 1 * 3)]
        [InlineData("3*2+1", 3 * 2 + 1)]
        [Theory]
        public void MathParserMultiplyTest1(string ExpressionToTest, int ExpectedResultOfExpression)
        {
            Assert.Equal(ExpectedResultOfExpression, ExpressionLibrary.ParseAndEvaluateNumberExpression(ExpressionToTest));
        }

        /// <summary>
        /// Unknown character exception check
        /// </summary>
        /// <param name="ExpressionToTest">Expression To Test</param>
        [InlineData("2+b+c")]
        [InlineData("abc")]
        [Theory]
        public void MathParserExpectedExceptionTest1(string ExpressionToTest)
        {
            Assert.Throws<ParserUnknownCharacterException>(() => ExpressionLibrary.ParseAndEvaluateNumberExpression(ExpressionToTest));
        }

        /// <summary>
        /// Missing Parenthesis
        /// </summary>
        /// <param name="ExpressionToTest">Expression To Test</param>
        [InlineData("10 * (1+2 * 5")]
        [InlineData("10 * 1+2) * 5")]
        [Theory]
        public void MathParserExpectedExceptionMissingParenthesisTest1(string ExpressionToTest)
        {
            Assert.Throws<MissingParenthesisException>(() => ExpressionLibrary.ParseAndEvaluateNumberExpression(ExpressionToTest));
        }

        #endregion

        #region Logical - Relational Testing

        /// <summary>
        /// hold all the valid token factories. This is a little weird...will cleanup or remove this logic on scan lazy later on.
        /// </summary>
        private ISet<ITokenFactory> ValidLogicalTokens = new HashSet<ITokenFactory>(
            new ITokenFactory[]
            {
                new NumberLiteralTokenFactory(),
                new EqualTokenFactory(),
                new NotEqualTokenFactory(),
                new GreaterThanTokenFactory(),
                new GreaterThanOrEqualTokenFactory(),
                new LessThanOrEqualTokenFactory(),
                new LessThanTokenFactory()
            });

        private void AssertTree(IEnumerable<TokenBase> BuiltTreeToTest, double ExpectedLiteralNumberValue, Type[] ExpectedTokens)
        {
            //make sure we have the same amount of nodes
            Assert.Equal(BuiltTreeToTest.Count(), ExpectedTokens.Count());

            //current tree node
            int i = 0;

            //make sure we have the expected tokens
            foreach (var TokenFound in BuiltTreeToTest)
            {
                //make sure its the correct type
                Assert.IsAssignableFrom(ExpectedTokens[i], TokenFound);

                //increase the tally
                i++;
            }

            //go make sure the number literal token is correct.
            Assert.Equal(ExpectedLiteralNumberValue, BuiltTreeToTest.OfType<NumberLiteralToken>().Single().Value);
        }

        [InlineData("= 10256", 10256, new Type[] { typeof(EqualToToken), typeof(NumberLiteralToken) })]
        [InlineData("<> 99", 99, new Type[] { typeof(NotEqualToToken), typeof(NumberLiteralToken) })]
        [InlineData("!= 99", 99, new Type[] { typeof(NotEqualToToken), typeof(NumberLiteralToken) })]
        [InlineData(">= 10", 10, new Type[] { typeof(GreaterThanOrEqualToken), typeof(NumberLiteralToken) })]
        [InlineData("> 502", 502, new Type[] { typeof(GreaterThanToken), typeof(NumberLiteralToken) })]
        [InlineData("<= 10", 10, new Type[] { typeof(LessThanOrEqualToToken), typeof(NumberLiteralToken) })]
        [InlineData("< 502", 502, new Type[] { typeof(LessThanToken), typeof(NumberLiteralToken) })]
        [Theory]
        public void LogicalParserTest1(string ExpressionToTest, double ExpectedLiteralNumberValue, Type[] ExpectedTokens)
        {
            //go assert the tree
            AssertTree(GenericTokenizer.ScanLazy(ExpressionToTest, ValidLogicalTokens).ToArray(), ExpectedLiteralNumberValue, ExpectedTokens);
        }

        #endregion

    }

}
