using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.Mathematical.Mode;

namespace ToracLibraryTest.UnitsTest.Core
{

    /// <summary>
    /// Unit test for Mode calculation
    /// </summary>
    [TestClass]
    public class ModeTest
    {

        [TestCategory("Core.Mathematical.Mode")]
        [TestCategory("Core.Mathematical")]
        [TestCategory("Core")]
        [TestMethod]
        public void ModeTest1()
        {
            //this test where all the numbers are the same (used once)

            //grab the data we are going to test with
            int[] TestData = { 1, 2, 3, 4 };

            //go run the method so we can compare the results
            var TestResult = ModeCalculation.CalculateMode(TestData);

            //compare the most used times
            Assert.AreEqual(1, TestResult.HowManyTimesUsed);

            //test how many items are the mean
            Assert.AreEqual(4, TestResult.Mean.Count);

            //now compare what the result of the most used numbers are
            Assert.IsTrue(TestResult.Mean.Contains(1));
            Assert.IsTrue(TestResult.Mean.Contains(2));
            Assert.IsTrue(TestResult.Mean.Contains(3));
            Assert.IsTrue(TestResult.Mean.Contains(4));
        }

        [TestCategory("Core.Mathematical.Mode")]
        [TestCategory("Core.Mathematical")]
        [TestCategory("Core")]
        [TestMethod]
        public void ModeTest2()
        {
            //this test where only 1 number is the mean

            //grab the data we are going to test with
            double[] TestData = { 1, 2, 3, 4, 1 };

            //go run the method so we can compare the results
            var TestResult = ModeCalculation.CalculateMode(TestData);

            //compare the most used times
            Assert.AreEqual(2, TestResult.HowManyTimesUsed);

            //test how many items are the mean
            Assert.AreEqual(1, TestResult.Mean.Count);

            //now compare what the result of the most used numbers are
            Assert.IsTrue(TestResult.Mean.Contains(1));
            Assert.IsFalse(TestResult.Mean.Contains(2));
            Assert.IsFalse(TestResult.Mean.Contains(3));
            Assert.IsFalse(TestResult.Mean.Contains(4));
        }

        [TestCategory("Core.Mathematical.Mode")]
        [TestCategory("Core.Mathematical")]
        [TestCategory("Core")]
        [TestMethod]
        public void ModeTest3()
        {
            //this test where multiple numbers (2) are the mean

            //grab the data we are going to test with
            decimal[] TestData = { 1, 1, 2, 2, 3, 4 };

            //go run the method so we can compare the results
            var TestResult = ModeCalculation.CalculateMode(TestData);

            //compare the most used times
            Assert.AreEqual(2, TestResult.HowManyTimesUsed);

            //test how many items are the mean
            Assert.AreEqual(2, TestResult.Mean.Count);

            //now compare what the result of the most used numbers are
            Assert.IsTrue(TestResult.Mean.Contains(1));
            Assert.IsTrue(TestResult.Mean.Contains(2));
            Assert.IsFalse(TestResult.Mean.Contains(3));
            Assert.IsFalse(TestResult.Mean.Contains(4));
        }

    }

}
