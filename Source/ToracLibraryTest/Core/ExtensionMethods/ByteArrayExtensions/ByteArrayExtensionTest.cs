using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ToracLibrary.Core.ExtensionMethods.ByteArrayExtensions;

namespace ToracLibraryTest.UnitsTest.ExtensionMethods.Core
{

    /// <summary>
    /// Unit test to test byte array Extension Methods
    /// </summary>
    [TestClass]
    public class ByteArrayExtensionTest
    {

        #region Constants

        /// <summary>
        /// Should be the results of the test. Just use ToUpper or ToLower to check the variation
        /// </summary>
        private const string ResultValue = "5465737456616c7565";

        #endregion

        /// <summary>
        /// Unit test to ensure a byte array can convert to a hexadecimal string in lowercase
        /// </summary>
        [TestCategory("Core.ExtensionMethods.ByteArrayExtensions")]
        [TestCategory("ExtensionMethods")]
        [TestCategory("Core")]
        [TestMethod]
        public void ByteArrayToHexadecimalLowerCaseTest1()
        {
            //declare the test value
            const string TestValue = "TestValue";

            //go grab the bytes
            var BytesToTest = new UTF8Encoding().GetBytes(TestValue);

            //now make sure nothing has changed
            Assert.AreEqual(ResultValue.ToLower(), BytesToTest.ToByteArrayToHexadecimalString(true));
        }

        /// <summary>
        /// Unit test to ensure a byte array can convert to a hexadecimal string in uppercase
        /// </summary>
        [TestCategory("Core.ExtensionMethods.ByteArrayExtensions")]
        [TestCategory("ExtensionMethods")]
        [TestCategory("Core")]
        [TestMethod]
        public void ByteArrayToHexadecimalUpperCaseTest1()
        {
            //declare the test value
            const string TestValue = "TestValue";

            //go grab the bytes
            var BytesToTest = new UTF8Encoding().GetBytes(TestValue);

            //now make sure nothing has changed
            Assert.AreEqual(ResultValue.ToUpper(), BytesToTest.ToByteArrayToHexadecimalString(false));
        }

    }

}