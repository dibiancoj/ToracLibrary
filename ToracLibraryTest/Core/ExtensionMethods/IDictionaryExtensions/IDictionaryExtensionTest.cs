using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToracLibrary.Core.ExtensionMethods.IDictionaryExtensions;
using ToracLibraryTest.Framework.DummyObjects;

namespace ToracLibraryTest.UnitsTest.ExtensionMethods.Core
{

    /// <summary>
    /// Unit test to IDictionary Extension Methods
    /// </summary>
    [TestClass]
    public class IDictionaryExtensionTest
    {

        /// <summary>
        /// Unit test for try add to a dictionary
        /// </summary>
        [TestCategory("Core.ExtensionMethods.IDictionaryExtensions")]
        [TestCategory("ExtensionMethods")]
        [TestCategory("Core")]
        [TestMethod]
        public void TryAddTest1()
        {
            //create a test dictionary which we will use
            var TestDictionary = new Dictionary<int, DummyObject>();

            /* this saves you from writing the following. It's now 1 line. Just helps clean up redundant code
             * if (!TestDictionary.ContainsKey(24))
             * {
             * TestDictionary.Add(......)
             * }
             */

            //add an item we don't have in the dictionary
            Assert.AreEqual(false, TestDictionary.ContainsKey(24));
            Assert.AreEqual(true, TestDictionary.TryAdd(24, new DummyObject(24, "24")));
            Assert.AreEqual(true, TestDictionary.ContainsKey(24));

            //now check to see we get the correct result for that item we just added
            Assert.AreEqual(true, TestDictionary.ContainsKey(24));
            Assert.AreEqual(false, TestDictionary.TryAdd(24, new DummyObject(24, "24")));
            Assert.AreEqual(1, TestDictionary.Count);

            //try to add another new item
            Assert.AreEqual(false, TestDictionary.ContainsKey(25));
            Assert.AreEqual(true, TestDictionary.TryAdd(25, new DummyObject(25, "25")));
            Assert.AreEqual(true, TestDictionary.ContainsKey(25));

            //make sure 24 is still in the dictionary
            Assert.AreEqual(true, TestDictionary.ContainsKey(24));

            //make sure we have 2 items
            Assert.AreEqual(2, TestDictionary.Count);
        }

    }

}