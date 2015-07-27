using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToracLibrary.ExtensionMethods.ObjectExtensions;
using ToracLibraryTest.Framework.DummyObjects;

namespace ToracLibraryTest.ExtensionMethods.ObjectTest
{

    /// <summary>
    /// Unit test to Object Extension Methods
    /// </summary>
    [TestClass]
    public class ObjectExtensionTest
    {

        /// <summary>
        /// Unit test to create an IEnumerable from a single object
        /// </summary>
        [TestMethod]
        public void SingleObjectToIEnumerableTest1()
        {
            //grab a single record and push to an ienumerable
            var IEnumerableBuiltFromSingleObject = DummyObject.CreateDummyRecord().ToIEnumerable();

            //make sure we only have 1 record. This should prove it's in a form of ienumerable
            Assert.AreEqual(1, IEnumerableBuiltFromSingleObject.Count());
        }

        /// <summary>
        /// Unit test to create an IList from a single object
        /// </summary>
        [TestMethod]
        public void SingleObjectToListTest1()
        {
            //grab a single record and push to an ienumerable
            var IListBuiltFromSingleObject = DummyObject.CreateDummyRecord().ToIList();

            //make sure we only have 1 record. This should prove it's in a form of ienumerable
            Assert.AreEqual(1, IListBuiltFromSingleObject.Count);

            //add another record so we can make sure it increments
            IListBuiltFromSingleObject.Add(DummyObject.CreateDummyRecord());

            //check the count
            Assert.AreEqual(2, IListBuiltFromSingleObject.Count);
        }

    }

}