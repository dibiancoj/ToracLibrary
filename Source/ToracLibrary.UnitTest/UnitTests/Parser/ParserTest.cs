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

        #region Plus Minus Parser 

        /// <summary>
        /// Test a valid expression and its result
        /// </summary>
        /// <param name="ExpressionToTest">Expression to test</param>
        /// <param name="ExpectedResultOfExpression">Expected result of the expression</param>
        [InlineData("1 +   20", 1 + 20)]
        [InlineData("1 - 10", 1 - 10)]
        [InlineData("2+4+7", 2 + 4 + 7)]
        [InlineData("2+4+7- 2", 2 + 4 + 7 - 2)]
        [Theory]
        public void PlusMinusParserTest1(string ExpressionToTest, int ExpectedResultOfExpression)
        {
            Assert.Equal(ExpectedResultOfExpression, ExpressionLibrary.ParseNumberExpression(ExpressionToTest));
        }

        /// <summary>
        /// Unknown character exception check
        /// </summary>
        /// <param name="ExpressionToTest">Expression To Test</param>
        [InlineData("2+b+c")]
        [InlineData("abc")]
        [Theory]
        public void PlusMinusExpectedExceptionTest1(string ExpressionToTest)
        {
            Assert.Throws<ParserUnknownCharacterException>(() => ExpressionLibrary.ParseNumberExpression(ExpressionToTest));
        }

        /// <summary>
        /// Expecting character exception check
        /// </summary>
        /// <param name="ExpressionToTest">Expression To Test</param>
        [InlineData("1+2 3")]
        [Theory]
        public void ExpectingCharacterExceptionTest1(string ExpressionToTest)
        {
            Assert.Throws<ExpectingTokenException>(() => ExpressionLibrary.ParseNumberExpression(ExpressionToTest));
        }


        #endregion

    }

}
