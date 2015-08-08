using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.Mathematical.GeometricAverage;

namespace ToracLibraryTest.UnitsTest.Core
{

    /// <summary>
    /// Unit test for geometric average calculation
    /// </summary>
    [TestClass]
    public class GeometricAverageTest
    {

        [TestCategory("Core.Mathematical.GeometricAverage")]
        [TestCategory("Core.Mathematical")]
        [TestCategory("Core")]
        [TestMethod]
        public void GeometricAverageTest1()
        {
            //build the base data set to calculate
            double[] BaseLineData = { 1, 2, 3, 4, 5 };

            //go calculate the results and compare it to what we expect
            Assert.AreEqual(2.6051710846973521, GeometricAverageCalculation.CalculateGeometricAverage(BaseLineData));
        }

        [TestCategory("Core.Mathematical.GeometricAverage")]
        [TestCategory("Core.Mathematical")]
        [TestCategory("Core")]
        [TestMethod]
        public void GeometricAverageTest2()
        {
            //build the base data set to calculate
            double[] BaseLineData = { 1, 10, 11100, 2, 5, 3.45, 5.56 };
            
            //go calculate the results and compare it to what we expect
            Assert.AreEqual(11.140075228833647, GeometricAverageCalculation.CalculateGeometricAverage(BaseLineData));
        }

    }

}
