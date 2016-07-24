using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ToracLibrary.UnitTest.Framework
{

    /// <summary>
    /// Any generic method we may need
    /// </summary>
    internal static class FrameworkHelperMethods
    {

        /// <summary>
        /// Test's an array to make each element in the index is the same
        /// </summary>
        /// <param name="ExpectedResult">Expected Result To Test Against</param>
        /// <param name="ResultOfFact">Result Of The Test Method</param>
        /// <remarks>Will throw an error if the elements don't match</remarks>
        internal static void UnitTestArrayElements<T>(IEnumerable<T> ExpectedResult, IEnumerable<T> ResultOfFact)
        {
            //push both items to an array
            var ExpectedResultArray = ExpectedResult.ToArray();
            var ResultOfFactArray = ResultOfFact.ToArray();

            //first make sure they are the same length
            Assert.Equal(ExpectedResultArray.Length, ResultOfFactArray.Length);

            //loop through the results of the test method
            for (int i = 0; i < ResultOfFactArray.Length; i++)
            {
                //assert each item in the array
                Assert.Equal(ExpectedResultArray[i], ResultOfFactArray[i]);
            }
        }

        /// <summary>
        /// Helper method to grab the field for a specified name
        /// </summary>
        /// <param name="ClassType">Class which has the type we need to retrieve</param>
        /// <param name="FieldName">field name to retrieve</param>
        /// <returns>format string to use</returns>
        internal static string GetPrivateFieldValue(Type ClassType, string FieldName)
        {
            return ClassType.GetField(FieldName, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).GetValue(null).ToString();
        }

    }

}
