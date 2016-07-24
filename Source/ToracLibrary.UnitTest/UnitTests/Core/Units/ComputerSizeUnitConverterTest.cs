using System;
using Xunit;
using static ToracLibrary.Core.Units.ComputerSizeUnitConverter;

namespace ToracLibrary.UnitTest.Core
{

    /// <summary>
    /// Unit test to convert size units for computers
    /// </summary>
    public class ComputerSizeUnitConverterTest
    {

        /// <summary>
        /// Test the conversion between 2 unit types for computer sizes
        /// </summary>
        [Fact]
        public void UnitConversionTest1()
        {
            //run a whole bunch of tests
            Assert.Equal(1, ConvertUnitCalcuation(ComputerSizeUnit.Byte, ComputerSizeUnit.Byte, 1));
            Assert.Equal(250, ConvertUnitCalcuation(ComputerSizeUnit.Byte, ComputerSizeUnit.Byte, 250));

            Assert.Equal(0.0009765625, ConvertUnitCalcuation(ComputerSizeUnit.Byte, ComputerSizeUnit.Kilobyte, 1));
            Assert.Equal(9.5367431640625E-07, ConvertUnitCalcuation(ComputerSizeUnit.Byte, ComputerSizeUnit.Megabyte, 1));
            Assert.Equal(0.00000000093132257461547852, ConvertUnitCalcuation(ComputerSizeUnit.Byte, ComputerSizeUnit.Gigabyte, 1));
            Assert.Equal(0.00000000000090949470177292824, ConvertUnitCalcuation(ComputerSizeUnit.Byte, ComputerSizeUnit.Terabyte, 1));

            Assert.Equal(1024, ConvertUnitCalcuation(ComputerSizeUnit.Terabyte, ComputerSizeUnit.Gigabyte, 1));
            Assert.Equal(1048576, ConvertUnitCalcuation(ComputerSizeUnit.Terabyte, ComputerSizeUnit.Megabyte, 1));
            Assert.Equal(1073741824, ConvertUnitCalcuation(ComputerSizeUnit.Terabyte, ComputerSizeUnit.Kilobyte, 1));
            Assert.Equal(1099511627776, ConvertUnitCalcuation(ComputerSizeUnit.Terabyte, ComputerSizeUnit.Byte, 1));

            Assert.Equal(0.0048828125, ConvertUnitCalcuation(ComputerSizeUnit.Byte, ComputerSizeUnit.Kilobyte, 5));
            Assert.Equal(0.00000476837158203125, ConvertUnitCalcuation(ComputerSizeUnit.Kilobyte, ComputerSizeUnit.Gigabyte, 5));
            Assert.Equal(0.0000000000045474735088646412, ConvertUnitCalcuation(ComputerSizeUnit.Byte, ComputerSizeUnit.Terabyte, 5));
        }

    }

}