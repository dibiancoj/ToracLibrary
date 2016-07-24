using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.Mathematical.Median;
using Xunit;

namespace ToracLibrary.UnitTest.Core
{

    /// <summary>
    /// Unit test for Median calculation
    public class MedianTest
    {

        [Fact]
        public void MedianTest1()
        {
            //test when we have an odd number of elements
            int[] BaseLineData = { 1, 2, 3, 4, 5, 6, 7 };

            //go calculate median for an odd number of elements
            Assert.Equal(4, MedianCalculation.CalculateMedian(BaseLineData.Select(x => Convert.ToDouble(x)).ToArray()));
        }

        [Fact]
        public void MedianTest2()
        {
            //test when we have an even number of elements
            double[] BaseLineData = { 2.5, 5, 1.25, 6, 7, 10, 11, 250 };

            //go calculate median for an even amount of elements
            Assert.Equal(6.5, MedianCalculation.CalculateMedian(BaseLineData));
        }

    }

}
