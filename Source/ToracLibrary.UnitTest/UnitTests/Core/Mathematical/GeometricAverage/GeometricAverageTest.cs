using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.Mathematical.GeometricAverage;
using Xunit;

namespace ToracLibrary.UnitTest.Core
{

    /// <summary>
    /// Unit test for geometric average calculation
    /// </summary>
    public class GeometricAverageTest
    {

        [Fact]
        public void GeometricAverageTest1()
        {
            //build the base data set to calculate
            double[] BaseLineData = { 1, 2, 3, 4, 5 };

            //go calculate the results and compare it to what we expect
            Assert.Equal(2.6051710846973521, GeometricAverageCalculation.CalculateGeometricAverage(BaseLineData));
        }

        [Fact]
        public void GeometricAverageTest2()
        {
            //build the base data set to calculate
            double[] BaseLineData = { 1, 10, 11100, 2, 5, 3.45, 5.56 };
            
            //go calculate the results and compare it to what we expect
            Assert.Equal(11.140075228833647, GeometricAverageCalculation.CalculateGeometricAverage(BaseLineData));
        }

    }

}
