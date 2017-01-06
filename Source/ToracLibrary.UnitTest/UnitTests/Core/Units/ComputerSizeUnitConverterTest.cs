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
        [InlineData(ComputerSizeUnit.Byte, ComputerSizeUnit.Byte, 1, 1)]
        [InlineData(ComputerSizeUnit.Byte, ComputerSizeUnit.Byte, 250, 250)]
        [InlineData(ComputerSizeUnit.Byte, ComputerSizeUnit.Kilobyte, 1, 0.0009765625)]
        [InlineData(ComputerSizeUnit.Byte, ComputerSizeUnit.Megabyte, 1, 9.5367431640625E-07)]
        [InlineData(ComputerSizeUnit.Byte, ComputerSizeUnit.Gigabyte, 1, 0.00000000093132257461547852)]
        [InlineData(ComputerSizeUnit.Byte, ComputerSizeUnit.Terabyte, 1, 0.00000000000090949470177292824)]
        [InlineData(ComputerSizeUnit.Terabyte, ComputerSizeUnit.Gigabyte, 1, 1024)]
        [InlineData(ComputerSizeUnit.Terabyte, ComputerSizeUnit.Megabyte, 1, 1048576)]
        [InlineData(ComputerSizeUnit.Terabyte, ComputerSizeUnit.Kilobyte, 1, 1073741824)]
        [InlineData(ComputerSizeUnit.Terabyte, ComputerSizeUnit.Byte, 1, 1099511627776)]
        [InlineData(ComputerSizeUnit.Byte, ComputerSizeUnit.Kilobyte, 5, 0.0048828125)]
        [InlineData(ComputerSizeUnit.Kilobyte, ComputerSizeUnit.Gigabyte, 5, 0.00000476837158203125)]
        [InlineData(ComputerSizeUnit.Byte, ComputerSizeUnit.Terabyte, 5, 0.0000000000045474735088646412)]
        [Theory]
        public void UnitConversionTest1(ComputerSizeUnit FromUnit, ComputerSizeUnit ToUnit, double ValueToConvert, double ExpectedResults)
        {
            Assert.Equal(ExpectedResults, ConvertUnitCalcuation(FromUnit, ToUnit, ValueToConvert));
        }

    }

}