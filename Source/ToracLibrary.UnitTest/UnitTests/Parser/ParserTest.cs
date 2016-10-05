using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Parser;
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
        [InlineData("1 +   20", 21)]
        [Theory]
        public void PlusMinusParserTest1(string ExpressionToTest, int ExpectedResultOfExpression)
        {
            Assert.Equal(ExpectedResultOfExpression, ExpressionLibrary.ParseNumberExpression(ExpressionToTest));
        }

        #endregion

    }

}
