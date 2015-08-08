using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.Mathematical.Median;

namespace ToracLibraryTest.UnitsTest.Core
{

    /// <summary>
    /// Unit test for Median calculation
    /// </summary>
    [TestClass]
    public class MedianTest
    {

        [TestCategory("Core.Mathematical.Median")]
        [TestCategory("Core.Mathematical")]
        [TestCategory("Core")]
        [TestMethod]
        public void MedianTest1()
        {
            //test when we have an odd number of elements
            int[] BaseLineData = { 1, 2, 3, 4, 5, 6, 7 };

            //go calculate median for an odd number of elements
            Assert.AreEqual(4, MedianCalculation.CalculateMedian(BaseLineData.Select(x => Convert.ToDouble(x)).ToArray()));
        }

        [TestCategory("Core.Mathematical.Median")]
        [TestCategory("Core.Mathematical")]
        [TestCategory("Core")]
        [TestMethod]
        public void MedianTest2()
        {
            //test when we have an even number of elements
            double[] BaseLineData = { 2.5, 5, 1.25, 6, 7, 10, 11, 250 };

            //go calculate median for an even amount of elements
            Assert.AreEqual(6.5, MedianCalculation.CalculateMedian(BaseLineData));
        }

    }

}
