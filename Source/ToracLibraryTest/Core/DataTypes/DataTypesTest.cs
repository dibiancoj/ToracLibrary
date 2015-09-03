using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using ToracLibrary.Core.DataTypes;
using ToracLibrary.Core.DateTimeHelpers;
using ToracLibrary.Core.DateTimeHelpers.BusinessHours;

namespace ToracLibraryTest.UnitsTest.Core
{

    /// <summary>
    /// Unit test to test data types
    /// </summary>
    [TestClass]
    public class DataTypesTest
    {

        #region Primitive Types

        /// <summary>
        /// Test Primitive types 
        /// </summary>
        [TestCategory("Core.DataTypes")]
        [TestCategory("Core")]
        [TestMethod]
        public void PrimitiveTypesTest1()
        {
            //grab all the types from the method
            var DataTypesToCheck = PrimitiveTypes.PrimitiveTypesSelect();

            //string test
            Assert.IsTrue(DataTypesToCheck.Contains(typeof(string)));

            //booleans
            Assert.IsTrue(DataTypesToCheck.Contains(typeof(bool)));
            Assert.IsTrue(DataTypesToCheck.Contains(typeof(bool?)));

            //int 16s
            Assert.IsTrue(DataTypesToCheck.Contains(typeof(Int16)));
            Assert.IsTrue(DataTypesToCheck.Contains(typeof(Int16?)));

            //int 32s
            Assert.IsTrue(DataTypesToCheck.Contains(typeof(int)));
            Assert.IsTrue(DataTypesToCheck.Contains(typeof(int?)));

            //int 64s
            Assert.IsTrue(DataTypesToCheck.Contains(typeof(Int64)));
            Assert.IsTrue(DataTypesToCheck.Contains(typeof(Int64?)));

            //singles
            Assert.IsTrue(DataTypesToCheck.Contains(typeof(Single)));
            Assert.IsTrue(DataTypesToCheck.Contains(typeof(Single?)));

            //doubles
            Assert.IsTrue(DataTypesToCheck.Contains(typeof(double)));
            Assert.IsTrue(DataTypesToCheck.Contains(typeof(double?)));

            //floats
            Assert.IsTrue(DataTypesToCheck.Contains(typeof(float)));
            Assert.IsTrue(DataTypesToCheck.Contains(typeof(float?)));

            //decimals
            Assert.IsTrue(DataTypesToCheck.Contains(typeof(decimal)));
            Assert.IsTrue(DataTypesToCheck.Contains(typeof(decimal?)));

            //check items that should be false
            Assert.IsFalse(DataTypesToCheck.Contains(typeof(IEnumerable<double>)));
            Assert.IsFalse(DataTypesToCheck.Contains(typeof(object)));
            Assert.IsFalse(DataTypesToCheck.Contains(typeof(List<double>)));
        }

        #endregion

    }

}