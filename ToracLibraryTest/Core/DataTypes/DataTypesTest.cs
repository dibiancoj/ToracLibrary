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
            Assert.AreEqual(true, DataTypesToCheck.Contains(typeof(string)));

            //booleans
            Assert.AreEqual(true, DataTypesToCheck.Contains(typeof(bool)));
            Assert.AreEqual(true, DataTypesToCheck.Contains(typeof(bool?)));

            //int 16s
            Assert.AreEqual(true, DataTypesToCheck.Contains(typeof(Int16)));
            Assert.AreEqual(true, DataTypesToCheck.Contains(typeof(Int16?)));

            //int 32s
            Assert.AreEqual(true, DataTypesToCheck.Contains(typeof(int)));
            Assert.AreEqual(true, DataTypesToCheck.Contains(typeof(int?)));

            //int 64s
            Assert.AreEqual(true, DataTypesToCheck.Contains(typeof(Int64)));
            Assert.AreEqual(true, DataTypesToCheck.Contains(typeof(Int64?)));

            //singles
            Assert.AreEqual(true, DataTypesToCheck.Contains(typeof(Single)));
            Assert.AreEqual(true, DataTypesToCheck.Contains(typeof(Single?)));

            //doubles
            Assert.AreEqual(true, DataTypesToCheck.Contains(typeof(double)));
            Assert.AreEqual(true, DataTypesToCheck.Contains(typeof(double?)));

            //floats
            Assert.AreEqual(true, DataTypesToCheck.Contains(typeof(float)));
            Assert.AreEqual(true, DataTypesToCheck.Contains(typeof(float?)));

            //decimals
            Assert.AreEqual(true, DataTypesToCheck.Contains(typeof(decimal)));
            Assert.AreEqual(true, DataTypesToCheck.Contains(typeof(decimal?)));

            //check items that should be false
            Assert.AreEqual(false, DataTypesToCheck.Contains(typeof(IEnumerable<double>)));
            Assert.AreEqual(false, DataTypesToCheck.Contains(typeof(object)));
            Assert.AreEqual(false, DataTypesToCheck.Contains(typeof(List<double>)));
        }

        #endregion

    }

}