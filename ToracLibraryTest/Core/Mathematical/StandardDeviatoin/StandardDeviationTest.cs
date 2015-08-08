using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.Mathematical.StandardDeviation;

namespace ToracLibraryTest.UnitsTest.Core
{

    /// <summary>
    /// Unit test for Standard Deviation calculation
    /// </summary>
    [TestClass]
    public class StandardDeviationTest
    {

        [TestCategory("Core.Mathematical.StandardDeviation")]
        [TestCategory("Core.Mathematical")]
        [TestCategory("Core")]
        [TestMethod]
        public void StandardDeviationTest1()
        {
            //declare our base line data set
            double[] BaseLineData = { 4, 8, 100 };

            //go check the results now
            Assert.AreEqual(44.34210439550904, StandardDeviationCalculation.CalculateStandardDeviationHelper(BaseLineData));
        }

        [TestCategory("Core.Mathematical.StandardDeviation")]
        [TestCategory("Core.Mathematical")]
        [TestCategory("Core")]
        [TestMethod]
        public void StandardDeviationTest2()
        {
            //declare our base line data set
            double[] BaseLineData = { 1.25, 2.7856, 5.5536, 100.01, 250.05, 4.03, 3 };

            //go check the results now
            Assert.AreEqual(87.329217656649419, StandardDeviationCalculation.CalculateStandardDeviationHelper(BaseLineData));
        }

    }

}
