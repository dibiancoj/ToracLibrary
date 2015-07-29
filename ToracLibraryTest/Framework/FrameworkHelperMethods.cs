using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibraryTest.Framework
{

    /// <summary>
    /// Any generic method we may need
    /// </summary>
    public static class FrameworkHelperMethods
    {

        /// <summary>
        /// Test's an array to make each element in the index is the same
        /// </summary>
        /// <param name="ExpectedResult">Expected Result To Test Against</param>
        /// <param name="ResultOfTestMethod">Result Of The Test Method</param>
        /// <remarks>Will throw an error if the elements don't match</remarks>
        public static void UnitTestArrayElements<T>(IEnumerable<T> ExpectedResult, IEnumerable<T> ResultOfTestMethod)
        {
            //push both items to an array
            var ExpectedResultArray = ExpectedResult.ToArray();
            var ResultOfTestMethodArray = ResultOfTestMethod.ToArray();

            //first make sure they are the same length
            Assert.AreEqual(ExpectedResultArray.Length, ResultOfTestMethodArray.Length);

            //loop through the results of the test method
            for (int i = 0; i < ResultOfTestMethodArray.Length; i++)
            {
                //assert each item in the array
                Assert.AreEqual(ExpectedResultArray[i], ResultOfTestMethodArray[i]);
            }
        }

    }

}
