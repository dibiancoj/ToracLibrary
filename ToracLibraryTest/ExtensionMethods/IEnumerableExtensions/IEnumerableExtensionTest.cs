using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToracLibrary.ExtensionMethods.IEnumerableExtensions;
using ToracLibraryTest.Framework.DummyObjects;

namespace ToracLibraryTest.ExtensionMethods.IEnumerableTest
{

    /// <summary>
    /// Unit test to IEnumerable Extension Methods
    /// </summary>
    [TestClass]
    public class IEnumerableExtensionTest
    {

        #region UnTyped IEnumerable

        #region For Each

        /// <summary>
        /// Unit test the basic functionality of count on IEnumerable
        /// </summary>
        [TestMethod]
        public void IEnumerableCountTest1()
        {
            //go check the counts
            Assert.AreEqual(10, ((IEnumerable)DummyObject.CreateDummyListLazy(10).ToArray()).Count());
            Assert.AreEqual(21, ((IEnumerable)DummyObject.CreateDummyListLazy(21).ToArray()).Count());
        }

        #endregion

        #endregion

        #region Typed IEnumerable

        #region Any With Null Check Tests

        /// <summary>
        /// Unit test the no predicate version
        /// </summary>
        [TestMethod]
        public void AnyWithNullCheckTest1()
        {
            //create a new null list that we will use to check
            List<int> lst = null;

            //check the null list
            Assert.AreEqual(false, lst.AnyWithNullCheck());

            //create a new list
            lst = new List<int>();

            //check if the object instance has any items
            Assert.AreEqual(false, lst.AnyWithNullCheck());

            //add an item to the list
            lst.Add(1);

            //do we see the 1 number
            Assert.AreEqual(true, lst.AnyWithNullCheck());

            //add another item
            lst.Add(2);

            //should see the 2 items
            Assert.AreEqual(true, lst.AnyWithNullCheck());

            //clear all the items
            lst.Clear();

            //should resolve to false
            Assert.AreEqual(false, lst.AnyWithNullCheck());

        }

        /// <summary>
        /// Unit test the version with the predicate
        /// </summary>
        [TestMethod]
        public void AnyWithNullCheckPredicateTest1()
        {
            //create a new null list that we will use to check
            List<int> lst = null;

            //should return false since we don't have an instance of an object
            Assert.AreEqual(false, lst.AnyWithNullCheck(x => x == 5));

            //create an instance of the list now
            lst = new List<int>();

            //we still don't have any items in the list
            Assert.AreEqual(false, lst.AnyWithNullCheck(x => x == 5));

            //add an item now 
            lst.Add(1);

            //we should be able to find the == 1
            Assert.AreEqual(true, lst.AnyWithNullCheck(x => x == 1));

            //we don't have anything greater then 5
            Assert.AreEqual(false, lst.AnyWithNullCheck(x => x > 5));

            //add 2
            lst.Add(2);

            //should be able to find the 2
            Assert.AreEqual(true, lst.AnyWithNullCheck(x => x == 2));

            //shouldn't be able to find any numbers greater then 5
            Assert.AreEqual(false, lst.AnyWithNullCheck(x => x > 5));

            //clear the list
            lst.Clear();

            //we have no items because we just cleared the list
            Assert.AreEqual(false, lst.AnyWithNullCheck(x => x <= 5));
        }

        #endregion

        #region First Index Of Element

        /// <summary>
        /// Unit test the basic functionality of the first index
        /// </summary>
        [TestMethod]
        public void FirstIndexOfElementTest1()
        {
            //create a dummy list to test
            var DummyCreatedList = DummyObject.CreateDummyListLazy(10).ToArray();

            //run the test below
            Assert.AreEqual(0, DummyCreatedList.FirstIndexOfElement(x => x.Id == 0));
            Assert.AreEqual(1, DummyCreatedList.FirstIndexOfElement(x => x.Id == 1));
            Assert.AreEqual(8, DummyCreatedList.FirstIndexOfElement(x => x.Id == 8));
            Assert.AreEqual(8, DummyCreatedList.FirstIndexOfElement(x => x.txt == "Test_8"));
            Assert.IsNull(DummyCreatedList.FirstIndexOfElement(x => x.Id == 1000));
        }

        #endregion

        #region Last Index Of Element

        /// <summary>
        /// Unit test the basic functionality of the last index
        /// </summary>
        [TestMethod]
        public void LastIndexOfElementTest1()
        {
            //create a dummy list to test
            var DummyCreatedList = DummyObject.CreateDummyListLazy(10).ToArray();

            //check the following tests
            Assert.IsNull(DummyCreatedList.LastIndexOfElement(x => x.Id == 100));
            Assert.AreEqual(DummyCreatedList.Count() - 1, DummyCreatedList.LastIndexOfElement(x => string.Equals(x.duplicateTxt, "Dup", StringComparison.OrdinalIgnoreCase)));
            Assert.AreEqual(4, DummyCreatedList.LastIndexOfElement(x => x.LastIndexTest == 5));

            //add this to the unit test to test a item found at the 0 index
            Assert.AreEqual(0, DummyCreatedList.LastIndexOfElement(x => x.Id == 0));
        }

        #endregion

        #region For Each

        /// <summary>
        /// Unit test the basic functionality of the foreach on ienumerable
        /// </summary>
        [TestMethod]
        public void ForEachTest1()
        {
            //set the value we want to change too
            const string ValueToSet = "Changed Value";

            //id values we want to change
            var IdsToChange = new int[] { 1, 2 };

            //grab the dummy list
            var DummyCreatedList = DummyObject.CreateDummyListLazy(10).ToArray();

            //go change the value for the 2 items using foreach
            DummyCreatedList.Where(x => IdsToChange.Contains(x.Id)).ForEach(x => x.txt = ValueToSet);

            //let's loop through each item and make sure we have the correct value
            foreach (var ItemToTest in DummyCreatedList.Where(x => IdsToChange.Contains(x.Id)))
            {
                //make sure it's the value
                Assert.AreEqual(ValueToSet, ItemToTest.txt);
            }
        }

        #endregion

        #region Distinct By

        /// <summary>
        /// Unit test the basic functionality of the Distinct By
        /// </summary>
        [TestMethod]
        public void DistintByTest1()
        {
            //create a dummy list
            var DummyCreatedList = DummyObject.CreateDummyListLazy(10).ToArray();

            //run a distinct by on the duplicate text
            var ResultsOfDistinctBy = DummyCreatedList.DistinctByLazy(x => x.duplicateTxt).OrderBy(x => x.Id).ToArray();

            //check the results
            Assert.AreEqual(2, ResultsOfDistinctBy.Count());
            Assert.AreEqual("Dup", ResultsOfDistinctBy.ElementAt(0).duplicateTxt);
            Assert.AreEqual("Dup = 1", ResultsOfDistinctBy.ElementAt(1).duplicateTxt);
        }

        #endregion

        #region  Output Friendly Description Of IEnumerable

        #region Non Index Overload

        /// <summary>
        /// Unit test the Output friendly with non index overload
        /// </summary>
        [TestMethod]
        public void OutputFriendlyDescriptionNonIndexOverloadTest1()
        {
            //create a new list to output
            var OutputList = new string[] { "Item 1", "Item 2", "Item 3" };

            //go run the output string
            var result = OutputList.ToOutputString(x => x, ", ");

            //check the result
            Assert.AreEqual("Item 1, Item 2, Item 3", result);
        }

        /// <summary>
        /// Unit test the Output friendly with non index overload
        /// </summary>
        [TestMethod]
        public void OutputFriendlyDescriptionNonIndexOverloadTest2()
        {
            //create a new list to output
            var OutputList = new List<string> { "Item 1" };

            //go run the output string
            var result = OutputList.ToOutputString(x => x, ", ");

            //check the result
            Assert.AreEqual("Item 1", result);
        }

        /// <summary>
        /// Unit test the Output friendly with non index overload
        /// </summary>
        [TestMethod]
        public void OutputFriendlyDescriptionNonIndexOverloadTest3()
        {
            //create a new list to output
            var OutputList = new List<string>();

            //go run the output string
            var result = OutputList.ToOutputString(x => x, ", ");

            //check the result
            Assert.AreEqual(string.Empty, result);
        }

        #endregion

        #region Index Overload

        /// <summary>
        /// Unit test the Output friendly with index overload
        /// </summary>
        [TestMethod]
        public void OutputFriendlyDescriptionWithIndexOverloadTest1()
        {
            //create a new list to output
            var OutputList = new List<string> { "Item 1", "Item 2", "Item 3" };

            //go run the output string
            var result = OutputList.ToOutputString((thisItem, thisIndex) => string.Format("{0}. {1}", thisIndex + 1, thisItem), ", ");

            //check the result
            Assert.AreEqual("1. Item 1, 2. Item 2, 3. Item 3", result);
        }

        /// <summary>
        /// Unit test the Output friendly with index overload
        /// </summary>
        [TestMethod]
        public void OutputFriendlyDescriptionWithIndexOverloadTest2()
        {
            //create a new list to output
            var OutputList = new List<string> { "Item 1" };

            //go run the output string
            var result = OutputList.ToOutputString((thisItem, thisIndex) => string.Format("{0}. {1}", thisIndex + 1, thisItem), ", ");

            //check the result
            Assert.AreEqual("1. Item 1", result);
        }

        /// <summary>
        /// Unit test the Output friendly with index overload
        /// </summary>
        [TestMethod]
        public void OutputFriendlyDescriptionWithIndexOverloadTest3()
        {
            //go run the output string on a blank string
            var result = Array.Empty<string>().ToOutputString((thisItem, thisIndex) => string.Format("{0}. {1}", thisIndex + 1, thisItem), ", ");

            //check the result
            Assert.AreEqual(string.Empty, result);
        }

        #endregion

        #endregion

        #region Paginate Results

        /// <summary>
        /// Unit test the basic functionality of the Paginate
        /// </summary>
        [TestMethod]
        public void PaginateResultsWithExtensionMethodsTest1()
        {
            //create a dummy list
            var DummyCreatedList = DummyObject.CreateDummyListLazy(100).ToArray();

            //grab the paged data
            var PagedData = DummyCreatedList.PaginateResults(2, 10).ToArray();

            //go check the results
            Assert.AreEqual(10, PagedData.Count());
            Assert.AreEqual(10, PagedData[0].Id);
            Assert.AreEqual(11, PagedData[1].Id);
            Assert.AreEqual(12, PagedData[2].Id);
        }

        #endregion

        #endregion

    }

}