using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToracLibrary.Core.ExtensionMethods.IEnumerableExtensions;
using ToracLibraryTest.Framework.DummyObjects;

namespace ToracLibraryTest.UnitsTest.ExtensionMethods.Core
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
        [TestCategory("Core.ExtensionMethods.IEnumerableExtensions")]
        [TestCategory("ExtensionMethods")]
        [TestCategory("Core")]
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
        [TestCategory("Core.ExtensionMethods.IEnumerableExtensions")]
        [TestCategory("ExtensionMethods")]
        [TestCategory("Core")]
        [TestMethod]
        public void AnyWithNullCheckTest1()
        {
            //create a new null list that we will use to check
            List<int> lst = null;

            //check the null list
            Assert.IsFalse(lst.AnyWithNullCheck());

            //create a new list
            lst = new List<int>();

            //check if the object instance has any items
            Assert.IsFalse(lst.AnyWithNullCheck());

            //add an item to the list
            lst.Add(1);

            //do we see the 1 number
            Assert.IsTrue(lst.AnyWithNullCheck());

            //add another item
            lst.Add(2);

            //should see the 2 items
            Assert.IsTrue(lst.AnyWithNullCheck());

            //clear all the items
            lst.Clear();

            //should resolve to false
            Assert.IsFalse(lst.AnyWithNullCheck());

        }

        /// <summary>
        /// Unit test the version with the predicate
        /// </summary>
        [TestCategory("Core.ExtensionMethods.IEnumerableExtensions")]
        [TestCategory("ExtensionMethods")]
        [TestCategory("Core")]
        [TestMethod]
        public void AnyWithNullCheckPredicateTest1()
        {
            //create a new null list that we will use to check
            List<int> lst = null;

            //should return false since we don't have an instance of an object
            Assert.IsFalse(lst.AnyWithNullCheck(x => x == 5));

            //create an instance of the list now
            lst = new List<int>();

            //we still don't have any items in the list
            Assert.IsFalse(lst.AnyWithNullCheck(x => x == 5));

            //add an item now 
            lst.Add(1);

            //we should be able to find the == 1
            Assert.IsTrue(lst.AnyWithNullCheck(x => x == 1));

            //we don't have anything greater then 5
            Assert.IsFalse(lst.AnyWithNullCheck(x => x > 5));

            //add 2
            lst.Add(2);

            //should be able to find the 2
            Assert.IsTrue(lst.AnyWithNullCheck(x => x == 2));

            //shouldn't be able to find any numbers greater then 5
            Assert.IsFalse(lst.AnyWithNullCheck(x => x > 5));

            //clear the list
            lst.Clear();

            //we have no items because we just cleared the list
            Assert.IsFalse(lst.AnyWithNullCheck(x => x <= 5));
        }

        #endregion

        #region First Index Of Element

        /// <summary>
        /// Unit test the basic functionality of the first index
        /// </summary>
        [TestCategory("Core.ExtensionMethods.IEnumerableExtensions")]
        [TestCategory("ExtensionMethods")]
        [TestCategory("Core")]
        [TestMethod]
        public void FirstIndexOfElementTest1()
        {
            //create a dummy list to test
            var DummyCreatedList = DummyObject.CreateDummyListLazy(10).ToArray();

            //run the test below
            Assert.AreEqual(0, DummyCreatedList.FirstIndexOfElement(x => x.Id == 0));
            Assert.AreEqual(1, DummyCreatedList.FirstIndexOfElement(x => x.Id == 1));
            Assert.AreEqual(8, DummyCreatedList.FirstIndexOfElement(x => x.Id == 8));
            Assert.AreEqual(8, DummyCreatedList.FirstIndexOfElement(x => x.Description == "Test_8"));
            Assert.IsNull(DummyCreatedList.FirstIndexOfElement(x => x.Id == 1000));
        }

        #endregion

        #region Last Index Of Element

        /// <summary>
        /// Unit test the basic functionality of the last index
        /// </summary>
        [TestCategory("Core.ExtensionMethods.IEnumerableExtensions")]
        [TestCategory("ExtensionMethods")]
        [TestCategory("Core")]
        [TestMethod]
        public void LastIndexOfElementTest1()
        {
            //creat the id's that will have duplicates
            var DuplicateIds = new int[] { 3, 4 };

            //duplicate text to use
            const string DuplicateTextTag = "Duplicate";

            //create a dummy list to test and modify it so we have duplicate propertu values
            var DummyCreatedList = DummyObject.CreateDummyListLazy(10).Select((x, i) => new { MainObject = x, DuplicateText = (DuplicateIds.Contains(i) ? DuplicateTextTag : null) }).ToArray();

            //check the following tests

            //check the main object that we don't have any id's with 100
            Assert.IsNull(DummyCreatedList.LastIndexOfElement(x => x.MainObject.Id == 100));

            //now check the duplicate text value and make sure the last index is 4 (or the highest number in duplicate id)
            Assert.AreEqual(DuplicateIds.Last(), DummyCreatedList.LastIndexOfElement(x => x.DuplicateText == DuplicateTextTag));

            //let's try to find something with no duplicates
            Assert.AreEqual(0, DummyCreatedList.LastIndexOfElement(x => x.MainObject.Id == 0));
        }

        #endregion

        #region For Each

        /// <summary>
        /// Unit test the basic functionality of the foreach on ienumerable
        /// </summary>
        [TestCategory("Core.ExtensionMethods.IEnumerableExtensions")]
        [TestCategory("ExtensionMethods")]
        [TestCategory("Core")]
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
            DummyCreatedList.Where(x => IdsToChange.Contains(x.Id)).ForEach(x => x.Description = ValueToSet);

            //let's loop through each item and make sure we have the correct value
            foreach (var ItemToTest in DummyCreatedList.Where(x => IdsToChange.Contains(x.Id)))
            {
                //make sure it's the value
                Assert.AreEqual(ValueToSet, ItemToTest.Description);
            }
        }

        #endregion

        #region Distinct By

        /// <summary>
        /// Unit test the basic functionality of the Distinct By
        /// </summary>
        [TestCategory("Core.ExtensionMethods.IEnumerableExtensions")]
        [TestCategory("ExtensionMethods")]
        [TestCategory("Core")]
        [TestMethod]
        public void DistintByTest1()
        {
            //creat the id's that will have duplicates
            var DuplicateIds = new int[] { 3, 4 };

            //duplicate text to use
            const string DuplicateTextTag = "Duplicate";

            //create a dummy list to test and modify it so we have duplicate propertu values
            var DummyCreatedList = DummyObject.CreateDummyListLazy(10).Select((x, i) => new { MainObject = x, DuplicateText = (DuplicateIds.Contains(i) ? DuplicateTextTag : null) }).ToArray();

            //run a distinct by on the duplicate text
            var ResultsOfDistinctBy = DummyCreatedList.DistinctByLazy(x => x.DuplicateText).OrderBy(x => x.MainObject.Id).ToArray();

            //check the results

            //make sure we have 2 distinct items
            Assert.AreEqual(2, ResultsOfDistinctBy.Length);

            //make sure we only have 1 distint value for the duplicate tag (make sure it's truly distinct)
            Assert.AreEqual(1, ResultsOfDistinctBy.Where(x => x.DuplicateText == DuplicateTextTag).Count());

            //make sure the duplicate text matches what we set
            Assert.AreEqual(DuplicateTextTag, ResultsOfDistinctBy.FirstOrDefault(x => x.DuplicateText == DuplicateTextTag).DuplicateText);
        }

        #endregion

        #region  Output Friendly Description Of IEnumerable

        #region Non Index Overload

        /// <summary>
        /// Unit test the Output friendly with non index overload
        /// </summary>
        [TestCategory("Core.ExtensionMethods.IEnumerableExtensions")]
        [TestCategory("ExtensionMethods")]
        [TestCategory("Core")]
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
        [TestCategory("Core.ExtensionMethods.IEnumerableExtensions")]
        [TestCategory("ExtensionMethods")]
        [TestCategory("Core")]
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
        [TestCategory("Core.ExtensionMethods.IEnumerableExtensions")]
        [TestCategory("ExtensionMethods")]
        [TestCategory("Core")]
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
        [TestCategory("Core.ExtensionMethods.IEnumerableExtensions")]
        [TestCategory("ExtensionMethods")]
        [TestCategory("Core")]
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
        [TestCategory("Core.ExtensionMethods.IEnumerableExtensions")]
        [TestCategory("ExtensionMethods")]
        [TestCategory("Core")]
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
        [TestCategory("Core.ExtensionMethods.IEnumerableExtensions")]
        [TestCategory("ExtensionMethods")]
        [TestCategory("Core")]
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

        #region Chunk Up list

        /// <summary>
        /// Helper method to calculate how many items should go in a bucket
        /// </summary>
        /// <param name="ItemsToBuild">how many items to build. Total data set count</param>
        /// <param name="MaxItemsInABucket">the most amount of items in a bucket</param>
        /// <returns>how many buckets there should be</returns>
        private static int HowManyGroupsInAChunkedUpList(int ItemsToBuild, int MaxItemsInABucket)
        {
            //ceiling so we "round up"
            return Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(ItemsToBuild) / MaxItemsInABucket)));
        }

        /// <summary>
        /// Unit test the basic functionality of the chunk list for ienumerable
        /// </summary>
        [TestCategory("Core.ExtensionMethods.IEnumerableExtensions")]
        [TestCategory("ExtensionMethods")]
        [TestCategory("Core")]
        [TestMethod]
        public void ChunkUpListTest1()
        {
            //the results should be 
            //bucket 1 = 5 items
            //bucket 2 = 5 items
            //bucket 3 = 5 items
            //bucket 4 = 5 items

            //how many items to build
            const int ItemsToBuild = 20;

            //how many items in a bucket
            const int MaxItemsInABucket = 5;

            //grab the dummy list
            var DummyCreatedList = DummyObject.CreateDummyListLazy(ItemsToBuild).ToArray();

            //let's chunk it up in slabs of 5
            var ChunkedInto5ElementsPerGroup = DummyCreatedList.ChunkUpListItemsLazy(MaxItemsInABucket).ToArray();

            //we should have an even 4 groups
            Assert.AreEqual(HowManyGroupsInAChunkedUpList(ItemsToBuild, MaxItemsInABucket), ChunkedInto5ElementsPerGroup.Length);

            //should be an even 5 elements per group
            ChunkedInto5ElementsPerGroup.ForEach(x => Assert.AreEqual(MaxItemsInABucket, x.Count()));

            //let's make sure, the elements in each group are correct. we will just check the first element in each group
            for (int i = 0; i < ChunkedInto5ElementsPerGroup.Length; i++)
            {
                Assert.AreEqual(i * MaxItemsInABucket, ChunkedInto5ElementsPerGroup[i].ElementAt(0).Id);
            }
        }

        /// <summary>
        /// Unit test the chunk list items, when we don't have an even aount
        /// </summary>
        [TestCategory("Core.ExtensionMethods.IEnumerableExtensions")]
        [TestCategory("ExtensionMethods")]
        [TestCategory("Core")]
        [TestMethod]
        public void ChunkUpListTest2()
        {
            //the results should be 
            //bucket 1 = 5 items
            //bucket 2 = 5 items
            //bucket 3 = 2 items

            //how many items to build
            const int ItemsToBuild = 12;

            //how many items in a bucket
            const int MaxItemsInABucket = 5;

            //grab the dummy list
            var DummyCreatedList = DummyObject.CreateDummyListLazy(12).ToArray();

            //let's chunk it up in slabs of 5 (we should have an extra 2 guys at the end)
            var ChunkedInto5ElementsPerGroup = DummyCreatedList.ChunkUpListItemsLazy(5).ToArray();

            //we should have an even 3 groups
            Assert.AreEqual(HowManyGroupsInAChunkedUpList(ItemsToBuild, MaxItemsInABucket), ChunkedInto5ElementsPerGroup.Length);

            //let's make sure, the elements in each group are correct. we will just check the first element in each group
            for (int i = 0; i < ChunkedInto5ElementsPerGroup.Length; i++)
            {
                //group should have the following number of elements
                int ShouldBeXAmountOfElementsPerGroup = MaxItemsInABucket;

                //let's make sure this group has the correct number (the last group should only have 2)
                if (i == (ChunkedInto5ElementsPerGroup.Length - 1))
                {
                    //it's the last group, there should only be 2 items
                    ShouldBeXAmountOfElementsPerGroup = 2;
                }

                //check how many elements in the group
                Assert.AreEqual(ShouldBeXAmountOfElementsPerGroup, ChunkedInto5ElementsPerGroup[i].Count());

                //now check the first element in the group
                Assert.AreEqual(i * 5, ChunkedInto5ElementsPerGroup[i].ElementAt(0).Id);
            }
        }

        #endregion

        #endregion

    }

}