using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToracLibrary.Units;

namespace ToracLibraryTest.UnitsTest
{

    /// <summary>
    /// Unit test to convert size units for computers
    /// </summary>
    [TestClass]
    public class ComputerSizeUnitConverterTest
    {

        /// <summary>
        /// Test the conversion between 2 unit types for computer sizes
        /// </summary>
        [TestMethod]
        public void UnitConversionTest1()
        {
            //run a whole bunch of tests
            Assert.AreEqual(1, ComputerSizeUnitConverter.ConvertUnitCalcuation(ComputerSizeUnitConverter.ComputerSizeUnit.Byte, ComputerSizeUnitConverter.ComputerSizeUnit.Byte, 1));
            Assert.AreEqual(250, ComputerSizeUnitConverter.ConvertUnitCalcuation(ComputerSizeUnitConverter.ComputerSizeUnit.Byte, ComputerSizeUnitConverter.ComputerSizeUnit.Byte, 250));

            Assert.AreEqual(0.0009765625, ComputerSizeUnitConverter.ConvertUnitCalcuation(ComputerSizeUnitConverter.ComputerSizeUnit.Byte, ComputerSizeUnitConverter.ComputerSizeUnit.Kilobyte, 1));
            Assert.AreEqual(9.5367431640625E-07, ComputerSizeUnitConverter.ConvertUnitCalcuation(ComputerSizeUnitConverter.ComputerSizeUnit.Byte, ComputerSizeUnitConverter.ComputerSizeUnit.Megabyte, 1));
            Assert.AreEqual(0.00000000093132257461547852, ComputerSizeUnitConverter.ConvertUnitCalcuation(ComputerSizeUnitConverter.ComputerSizeUnit.Byte, ComputerSizeUnitConverter.ComputerSizeUnit.Gigabyte, 1));
            Assert.AreEqual(0.00000000000090949470177292824, ComputerSizeUnitConverter.ConvertUnitCalcuation(ComputerSizeUnitConverter.ComputerSizeUnit.Byte, ComputerSizeUnitConverter.ComputerSizeUnit.Terabyte, 1));

            Assert.AreEqual(1024, ComputerSizeUnitConverter.ConvertUnitCalcuation(ComputerSizeUnitConverter.ComputerSizeUnit.Terabyte, ComputerSizeUnitConverter.ComputerSizeUnit.Gigabyte, 1));
            Assert.AreEqual(1048576, ComputerSizeUnitConverter.ConvertUnitCalcuation(ComputerSizeUnitConverter.ComputerSizeUnit.Terabyte, ComputerSizeUnitConverter.ComputerSizeUnit.Megabyte, 1));
            Assert.AreEqual(1073741824, ComputerSizeUnitConverter.ConvertUnitCalcuation(ComputerSizeUnitConverter.ComputerSizeUnit.Terabyte, ComputerSizeUnitConverter.ComputerSizeUnit.Kilobyte, 1));
            Assert.AreEqual(1099511627776, ComputerSizeUnitConverter.ConvertUnitCalcuation(ComputerSizeUnitConverter.ComputerSizeUnit.Terabyte, ComputerSizeUnitConverter.ComputerSizeUnit.Byte, 1));

            Assert.AreEqual(0.0048828125, ComputerSizeUnitConverter.ConvertUnitCalcuation(ComputerSizeUnitConverter.ComputerSizeUnit.Byte, ComputerSizeUnitConverter.ComputerSizeUnit.Kilobyte, 5));
            Assert.AreEqual(0.00000476837158203125, ComputerSizeUnitConverter.ConvertUnitCalcuation(ComputerSizeUnitConverter.ComputerSizeUnit.Kilobyte, ComputerSizeUnitConverter.ComputerSizeUnit.Gigabyte, 5));
            Assert.AreEqual(0.0000000000045474735088646412, ComputerSizeUnitConverter.ConvertUnitCalcuation(ComputerSizeUnitConverter.ComputerSizeUnit.Byte, ComputerSizeUnitConverter.ComputerSizeUnit.Terabyte, 5));
        }

    }

}