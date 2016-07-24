using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToracLibrary.Core.ExtensionMethods.ObjectExtensions;
using ToracLibrary.UnitTest.Framework;
using Xunit;

namespace ToracLibraryTest.UnitsTest.ExtensionMethods.Core
{

    /// <summary>
    /// Unit test to Object Extension Methods
    /// </summary>
    public class ObjectExtensionTest
    {

        /// <summary>
        /// Unit test to create an IEnumerable from a single object
        /// </summary>
        [Fact]
        public void SingleObjectToIEnumerableTest1()
        {
            //make sure we only have 1 record. This should prove it's in a form of ienumerable
            Assert.Equal(1, DummyObject.CreateDummyRecord().ToIEnumerableLazy().Count());
        }

        /// <summary>
        /// Unit test to create an IList from a single object
        /// </summary>
        [Fact]
        public void SingleObjectToListTest1()
        {
            //grab a single record and push to an ienumerable
            var IListBuiltFromSingleObject = DummyObject.CreateDummyRecord().ToIList();

            //make sure we only have 1 record. This should prove it's in a form of ienumerable
            Assert.Equal(1, IListBuiltFromSingleObject.Count);

            //add another record so we can make sure it increments
            IListBuiltFromSingleObject.Add(DummyObject.CreateDummyRecord());

            //check the count
            Assert.Equal(2, IListBuiltFromSingleObject.Count);
        }

    }

}