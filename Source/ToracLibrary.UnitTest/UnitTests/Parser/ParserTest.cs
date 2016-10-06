using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Parser;
using ToracLibrary.Parser.Exceptions;
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

        #endregion

    }

}
