using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.Mathematical;
using Xunit;

namespace ToracLibrary.UnitTest.Core
{

    /// <summary>
    /// Unit test for general mathematical helpers
    /// </summary>
    public class GeneralTest
    {

        #region Helper Method

        /// <summary>
        /// Going to use the string append then convert to test everything. The mathematical way is better so we will do it that way
        /// </summary>
        /// <param name="NumbersToTest">Numbers to test</param>
        /// <returns>Expected Result</returns>
        private static int ExpectedResult(IEnumerable<int> NumbersToTest)
        {
            //append all the numbers
            var Builder = new StringBuilder();

            //loop through all the numbers
            foreach (var Number in NumbersToTest)
            {
                //append the numbers
                Builder.Append(Number);
            }

            //Convert the number now
            return Convert.ToInt32(Builder.ToString());
        }

        #endregion

        /// <summary>
        /// Test a single number
        /// </summary>
        [Fact]
        public void ArrayOfNumbersToNumberTest1()
        {
            //build the base data set to calculate
            int[] ArrayToTest = new[] { 1 };

            //go calculate the results and compare it to what we expect
            Assert.Equal(ExpectedResult(ArrayToTest), MathematicalHelpers.ArrayOfNumbersToNumber(ArrayToTest));
        }

        /// <summary>
        /// Test an array with 2 numbers
        /// </summary>
        [Fact]
        public void ArrayOfNumbersToNumberTest2()
        {
            //build the base data set to calculate
            int[] ArrayToTest = new[] { 2, 0, 1 };

            //go calculate the results and compare it to what we expect
            Assert.Equal(ExpectedResult(ArrayToTest), MathematicalHelpers.ArrayOfNumbersToNumber(ArrayToTest));
        }

    }

}
