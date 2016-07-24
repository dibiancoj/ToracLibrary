using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.Mathematical.Mode;
using Xunit;

namespace ToracLibrary.UnitTest.Core
{

    /// <summary>
    /// Unit test for Mode calculation
    /// </summary>
    public class ModeTest
    {

        [Fact]
        public void ModeTest1()
        {
            //this test where all the numbers are the same (used once)

            //grab the data we are going to test with
            int[] TestData = { 1, 2, 3, 4 };

            //go run the method so we can compare the results
            var TestResult = ModeCalculation.CalculateMode(TestData);

            //compare the most used times
            Assert.Equal(1, TestResult.HowManyTimesUsed);

            //test how many items are the mean
            Assert.Equal(4, TestResult.Mean.Count);

            //now compare what the result of the most used numbers are
            Assert.True(TestResult.Mean.Contains(1));
            Assert.True(TestResult.Mean.Contains(2));
            Assert.True(TestResult.Mean.Contains(3));
            Assert.True(TestResult.Mean.Contains(4));
        }

        [Fact]
        public void ModeTest2()
        {
            //this test where only 1 number is the mean

            //grab the data we are going to test with
            double[] TestData = { 1, 2, 3, 4, 1 };

            //go run the method so we can compare the results
            var TestResult = ModeCalculation.CalculateMode(TestData);

            //compare the most used times
            Assert.Equal(2, TestResult.HowManyTimesUsed);

            //test how many items are the mean
            Assert.Equal(1, TestResult.Mean.Count);

            //now compare what the result of the most used numbers are
            Assert.True(TestResult.Mean.Contains(1));
            Assert.False(TestResult.Mean.Contains(2));
            Assert.False(TestResult.Mean.Contains(3));
            Assert.False(TestResult.Mean.Contains(4));
        }

        [Fact]
        public void ModeTest3()
        {
            //this test where multiple numbers (2) are the mean

            //grab the data we are going to test with
            decimal[] TestData = { 1, 1, 2, 2, 3, 4 };

            //go run the method so we can compare the results
            var TestResult = ModeCalculation.CalculateMode(TestData);

            //compare the most used times
            Assert.Equal(2, TestResult.HowManyTimesUsed);

            //test how many items are the mean
            Assert.Equal(2, TestResult.Mean.Count);

            //now compare what the result of the most used numbers are
            Assert.True(TestResult.Mean.Contains(1));
            Assert.True(TestResult.Mean.Contains(2));
            Assert.False(TestResult.Mean.Contains(3));
            Assert.False(TestResult.Mean.Contains(4));
        }

    }

}
