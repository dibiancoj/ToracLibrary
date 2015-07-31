using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToracLibrary.ExtensionMethods.ISetExtensions;
using ToracLibraryTest.Framework.DummyObjects;

namespace ToracLibraryTest.UnitsTest.ExtensionMethods
{

    /// <summary>
    /// Unit test to ISet Extension Methods
    /// </summary>
    [TestClass]
    public class ISetExtensionTest
    {

        /// <summary>
        /// Unit test for add range for a hash set. This test has no duplicates so everything should evaulate to true (Test 1)
        /// </summary>
        [TestMethod]
        public void AddRangeTest1()
        {
            //create a new instance which should contain 0 duplicate values

            //let's loop through and make all of them insert correctly (everything is a new item in the set)
            foreach (var ResultOfAdd in new HashSet<int>().AddRangeLazy(new int[] { 1, 2, 3, 4, 5 }))
            {
                //there should be no duplicates, so everything should evaulate to true
                Assert.AreEqual(true, ResultOfAdd.SuccesfullyAddedToHashSet);
            }
        }

        /// <summary>
        /// Unit test for add range for a hash set. This test has duplicate values where we need to check for true and false that get returned from the method call (Test 2)
        /// </summary>
        [TestMethod]
        public void AddRangeTest2()
        {
            //hold the duplicate values so we can assert them below
            var DuplicateValues = new int[] { 1, 2 };

            //hashset to build up to test
            var TestHashSet = new HashSet<int>(DuplicateValues);

            //let's loop through and make all of them insert correctly
            foreach (var ResultOfAdd in TestHashSet.AddRangeLazy(new int[] { 1, 2, 3, 4, 5 }))
            {
                //the result should be if it's in the duplicate list.
                Assert.AreEqual(!DuplicateValues.Contains(ResultOfAdd.AttemptedItemToBeAdded), ResultOfAdd.SuccesfullyAddedToHashSet);
            }
        }

    }

}