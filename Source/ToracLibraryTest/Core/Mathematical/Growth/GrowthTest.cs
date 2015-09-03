using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.Mathematical.Growth;

namespace ToracLibraryTest.UnitsTest.Core
{

    /// <summary>
    /// Unit test for Growth calculation
    /// </summary>
    [TestClass]
    public class GrowthTest
    {

        [TestCategory("Core.Mathematical.Growth")]
        [TestCategory("Core.Mathematical")]
        [TestCategory("Core")]
        [TestMethod]
        public void GrowthTest1()
        {
            Assert.AreEqual(0M, GrowthCalculation.CalculateGrowth(0, 0));

            Assert.AreEqual(1M, GrowthCalculation.CalculateGrowth(0, 1));

            Assert.AreEqual(-1M, GrowthCalculation.CalculateGrowth(50, 0));

            Assert.AreEqual(1M, GrowthCalculation.CalculateGrowth(50, 100));

            Assert.AreEqual(.50M, GrowthCalculation.CalculateGrowth(50, 75));

            Assert.AreEqual(-0.3333333333333333333333333333M, GrowthCalculation.CalculateGrowth(75, 50));

            Assert.AreEqual(0.6M, GrowthCalculation.CalculateGrowth(50, 80));

            Assert.AreEqual(1M, GrowthCalculation.CalculateGrowth(50, 100));

            //test 0's for both
            Assert.AreEqual(1M, GrowthCalculation.CalculateGrowth(0, 100));
            Assert.AreEqual(-1M, GrowthCalculation.CalculateGrowth(100, 0));

            //test for 0% when both numbers are the same
            Assert.AreEqual(0M, GrowthCalculation.CalculateGrowth(100, 100));
            Assert.AreEqual(0M, GrowthCalculation.CalculateGrowth(50.5M, 50.5M));

            //test for -% when both numbers are the same and negative
            Assert.AreEqual(0M, GrowthCalculation.CalculateGrowth(-100, -100));
            Assert.AreEqual(0M, GrowthCalculation.CalculateGrowth(-50.5M, -50.5M));

            //previous greater than 0 and current is higher
            Assert.AreEqual(1M, GrowthCalculation.CalculateGrowth(100, 200));

            //same test but have it equal something inbetween 0 and 1
            Assert.AreEqual(.5M, GrowthCalculation.CalculateGrowth(100, 150));

            //flip the signs...
            Assert.AreEqual(-3M, GrowthCalculation.CalculateGrowth(100, -200));

            //check for previous is -..and the current is 0
            Assert.AreEqual(1M, GrowthCalculation.CalculateGrowth(-100, 0));
            Assert.AreEqual(1M, GrowthCalculation.CalculateGrowth(-50, 0));
        }

    }

}
