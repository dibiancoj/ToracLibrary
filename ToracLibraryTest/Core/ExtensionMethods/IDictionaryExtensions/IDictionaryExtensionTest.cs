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

        #region Try Add

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
            Assert.IsFalse(TestDictionary.ContainsKey(24));
            Assert.IsTrue(TestDictionary.TryAdd(24, new DummyObject(24, "24")));
            Assert.IsTrue(TestDictionary.ContainsKey(24));

            //now check to see we get the correct result for that item we just added
            Assert.IsTrue(TestDictionary.ContainsKey(24));
            Assert.IsFalse(TestDictionary.TryAdd(24, new DummyObject(24, "24")));
            Assert.AreEqual(1, TestDictionary.Count);

            //try to add another new item
            Assert.IsFalse(TestDictionary.ContainsKey(25));
            Assert.IsTrue(TestDictionary.TryAdd(25, new DummyObject(25, "25")));
            Assert.IsTrue(TestDictionary.ContainsKey(25));

            //make sure 24 is still in the dictionary
            Assert.IsTrue(TestDictionary.ContainsKey(24));

            //make sure we have 2 items
            Assert.AreEqual(2, TestDictionary.Count);
        }

        #endregion

        #region Try Get

        /// <summary>
        /// Unit test for try get to a dictionary
        /// </summary>
        [TestCategory("Core.ExtensionMethods.IDictionaryExtensions")]
        [TestCategory("ExtensionMethods")]
        [TestCategory("Core")]
        [TestMethod]
        public void TryGetTest1()
        {
            //create a test dictionary which we will use
            var TestDictionary = new Dictionary<int, DummyObject>();

            /* this saves you from declaring the out parameter which is rough with the syntax
             * if (!TestDictionary.TryGetValue(24, out...))
             * {
             * 
             * }
             */

            //let's make sure we can't find an item since we have nothing in our dictionary right now
            Assert.IsNull(TestDictionary.TryGet(24));

            //let's add an item
            TestDictionary.Add(24, new DummyObject(24, "24"));

            //make sure we have this item now
            Assert.AreEqual(24, TestDictionary.TryGet(24).Id);

            //make sure we can't find 25
            Assert.IsNull(TestDictionary.TryGet(25));

            //now add 25
            TestDictionary.Add(25, new DummyObject(25, "25"));

            //make sure we can find 25 now
            Assert.AreEqual(25, TestDictionary.TryGet(25).Id);
        }

        #endregion

    }

}