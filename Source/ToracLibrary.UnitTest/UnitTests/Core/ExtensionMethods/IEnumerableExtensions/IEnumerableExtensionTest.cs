﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.IEnumerableExtensions;
using ToracLibrary.UnitTest.Framework;
using Xunit;

namespace ToracLibrary.UnitTest.ExtensionMethods.Core
{

    /// <summary>
    /// Unit test to IEnumerable Extension Methods
    /// </summary>
    public class IEnumerableExtensionTest
    {

        #region UnTyped IEnumerable

        #region Count

        /// <summary>
        /// Unit test the basic functionality of count on an untyped IEnumerable
        /// </summary>
        [Fact]
        public void IEnumerableCountTest1()
        {
            //go check the counts
            Assert.Equal(10, ((IEnumerable)DummyObject.CreateDummyListLazy(10).ToArray()).Count());
            Assert.Equal(21, ((IEnumerable)DummyObject.CreateDummyListLazy(21).ToArray()).Count());
        }

        #endregion

        #endregion

        #region Typed IEnumerable

        #region Any With Null Check Tests

        /// <summary>
        /// Unit test the no predicate version
        /// </summary>
        [Fact]
        public void AnyWithNullCheckTest1()
        {
            //create a new null list that we will use to check
            List<int> ListToTestWith = null;

            //check the null list
            Assert.False(ListToTestWith.AnyWithNullCheck());

            //create a new list
            ListToTestWith = new List<int>();

            //check if the object instance has any items
            Assert.False(ListToTestWith.AnyWithNullCheck());

            //add an item to the list
            ListToTestWith.Add(1);

            //do we see the 1 number
            Assert.True(ListToTestWith.AnyWithNullCheck());

            //add another item
            ListToTestWith.Add(2);

            //should see the 2 items
            Assert.True(ListToTestWith.AnyWithNullCheck());

            //clear all the items
            ListToTestWith.Clear();

            //should resolve to false
            Assert.False(ListToTestWith.AnyWithNullCheck());
        }

        /// <summary>
        /// Unit test the version with the predicate
        /// </summary>
        [Fact]
        public void AnyWithNullCheckPredicateTest1()
        {
            //create a new null list that we will use to check
            List<int> ListToTestWith = null;

            //should return false since we don't have an instance of an object
            Assert.False(ListToTestWith.AnyWithNullCheck(x => x == 5));

            //create an instance of the list now
            ListToTestWith = new List<int>();

            //we still don't have any items in the list
            Assert.False(ListToTestWith.AnyWithNullCheck(x => x == 5));

            //add an item now 
            ListToTestWith.Add(1);

            //we should be able to find the == 1
            Assert.True(ListToTestWith.AnyWithNullCheck(x => x == 1));

            //we don't have anything greater then 5
            Assert.False(ListToTestWith.AnyWithNullCheck(x => x > 5));

            //add 2
            ListToTestWith.Add(2);

            //should be able to find the 2
            Assert.True(ListToTestWith.AnyWithNullCheck(x => x == 2));

            //shouldn't be able to find any numbers greater then 5
            Assert.False(ListToTestWith.AnyWithNullCheck(x => x > 5));

            //clear the list
            ListToTestWith.Clear();

            //we have no items because we just cleared the list
            Assert.False(ListToTestWith.AnyWithNullCheck(x => x <= 5));
        }

        #endregion

        #region Empty If Null Tests

        /// <summary>
        /// Test an enumerable that is not empty. Should pass back the original enumerable
        /// </summary>
        [Fact]
        public void EmptyIfNullWithEnumerableThatIsNotNullTest1()
        {
            //original item to test
            var OriginalEnumerable = new List<string> { "1", "2", "3" };

            //go use the helper to check the result
            Assert.Equal(OriginalEnumerable, OriginalEnumerable.EmptyIfNull());
        }

        /// <summary>
        /// Test an enumerable that is not empty. Should pass back the original enumerable
        /// </summary>
        [Fact]
        public void EmptyIfNullWithEnumerableThatIsNullTest1()
        {
            //original item to test
            List<string> OriginalEnumerable = null;

            //go grab the result. (pass in empty enumerable...because the result should be empty)
            Assert.Equal(Enumerable.Empty<string>(), OriginalEnumerable.EmptyIfNull());
        }

        #endregion

        #region Any With Null Check Tests

        /// <summary>
        /// Unit test the Coalesce for a class
        /// </summary>
        [Fact]
        public void CoalesceWithClassTest1()
        {
            DateTime? Date1 = null;
            DateTime? Date2 = new DateTime(2017, 1, 1);
            DateTime? Date3 = new DateTime(2017, 12, 1);

            //date 2 is the first non null date
            Assert.Equal(Date2, new DateTime?[] { Date1, Date2, Date3 }.Coalesce());

            //3rd element is first non null
            Assert.Equal(Date3, new DateTime?[] { Date1, Date1, Date3 }.Coalesce());

            //all items are null...this should return null
            Assert.Null(new DateTime?[] { Date1, Date1, null }.Coalesce());
        }

        /// <summary>
        /// Unit test the Coalesce for a struct
        /// </summary>
        [Fact]
        public void CoalesceWithStructTest1()
        {
            DateTime Date1 = new DateTime(2017, 1, 1);
            DateTime Date2 = DateTime.MinValue;
            DateTime Date3 = new DateTime(2017, 12, 1);

            //date 2 is the first non default date
            Assert.Equal(Date1, new DateTime[] { Date1, Date2, Date3 }.Coalesce());

            //switch the order and make sure 2 is the returned value
            Assert.Equal(Date3, new DateTime[] { Date2, Date3, Date1 }.Coalesce());

            //all items are null...this should return null
            Assert.Equal(DateTime.MinValue, new DateTime[] { Date2, Date2, DateTime.MinValue }.Coalesce());
        }

        #endregion

        #region First Index Of Element

        /// <summary>
        /// Unit test the basic functionality of the first index
        /// </summary>
        [Fact]
        public void FirstIndexOfElementTest1()
        {
            //create a dummy list to test
            var DummyCreatedList = DummyObject.CreateDummyListLazy(10).ToArray();

            //run the test below
            Assert.Equal(0, DummyCreatedList.FirstIndexOfElement(x => x.Id == 0));
            Assert.Equal(1, DummyCreatedList.FirstIndexOfElement(x => x.Id == 1));
            Assert.Equal(8, DummyCreatedList.FirstIndexOfElement(x => x.Id == 8));
            Assert.Equal(8, DummyCreatedList.FirstIndexOfElement(x => x.Description == "Test_8"));
            Assert.Null(DummyCreatedList.FirstIndexOfElement(x => x.Id == 1000));
        }

        #endregion

        #region Last Index Of Element

        /// <summary>
        /// Unit test the basic functionality of the last index
        /// </summary>
        [Fact]
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
            Assert.Null(DummyCreatedList.LastIndexOfElement(x => x.MainObject.Id == 100));

            //now check the duplicate text value and make sure the last index is 4 (or the highest number in duplicate id)
            Assert.Equal(DuplicateIds.Last(), DummyCreatedList.LastIndexOfElement(x => x.DuplicateText == DuplicateTextTag));

            //let's try to find something with no duplicates
            Assert.Equal(0, DummyCreatedList.LastIndexOfElement(x => x.MainObject.Id == 0));
        }

        #endregion

        #region For Each

        /// <summary>
        /// Unit test the basic functionality of the foreach on ienumerable
        /// </summary>
        [Fact]
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
                Assert.Equal(ValueToSet, ItemToTest.Description);
            }
        }

        #endregion

        #region Distinct By

        /// <summary>
        /// Unit test the basic functionality of the Distinct By
        /// </summary>
        [Fact]
        public void DistinctByTest1()
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
            Assert.Equal(2, ResultsOfDistinctBy.Length);

            //make sure we only have 1 distint value for the duplicate tag (make sure it's truly distinct)
            Assert.Single(ResultsOfDistinctBy.Where(x => x.DuplicateText == DuplicateTextTag));

            //make sure the duplicate text matches what we set
            Assert.Equal(DuplicateTextTag, ResultsOfDistinctBy.FirstOrDefault(x => x.DuplicateText == DuplicateTextTag).DuplicateText);
        }

        #endregion

        #region Chunk Up List

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
        [Fact]
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
            Assert.Equal(HowManyGroupsInAChunkedUpList(ItemsToBuild, MaxItemsInABucket), ChunkedInto5ElementsPerGroup.Length);

            //should be an even 5 elements per group
            ChunkedInto5ElementsPerGroup.ForEach(x => Assert.Equal(MaxItemsInABucket, x.Count()));

            //let's make sure, the elements in each group are correct. we will just check the first element in each group
            for (int i = 0; i < ChunkedInto5ElementsPerGroup.Length; i++)
            {
                Assert.Equal(i * MaxItemsInABucket, ChunkedInto5ElementsPerGroup[i].ElementAt(0).Id);
            }
        }

        /// <summary>
        /// Unit test the chunk list items when we have an even count
        /// </summary>
        [Fact]
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
            Assert.Equal(HowManyGroupsInAChunkedUpList(ItemsToBuild, MaxItemsInABucket), ChunkedInto5ElementsPerGroup.Length);

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
                Assert.Equal(ShouldBeXAmountOfElementsPerGroup, ChunkedInto5ElementsPerGroup[i].Count());

                //now check the first element in the group
                Assert.Equal(i * 5, ChunkedInto5ElementsPerGroup[i].ElementAt(0).Id);
            }
        }

        [Fact]
        public void ChunkUpListMaxNumberZeroRaiseException()
        {
            //grab the dummy list
            var DummyCreatedList = DummyObject.CreateDummyListLazy(2).ToArray();

            //these should throw an exception
            Assert.Throws<ArgumentOutOfRangeException>(() => DummyCreatedList.ChunkUpListItemsLazy(0).ToArray());
            Assert.Throws<ArgumentOutOfRangeException>(() => DummyCreatedList.ChunkUpListItemsLazy(-1).ToArray());

            //positive test so it shouldn't throw
            Assert.Equal(2, DummyCreatedList.ChunkUpListItemsLazy(1).SelectMany(x => x).Count());
        }

        #endregion

        #region Prepend and Append With Single Item

        /// <summary>
        /// Unit test for Prepend - single item first
        /// </summary>
        [Fact]
        public void PrependSingleItemFirstTest1()
        {
            //go build the list
            var TestList = DummyObject.CreateDummyListLazy(2).ToList();

            //go grab the single item
            var ItemToAppend = DummyObject.CreateDummyRecord();

            //go build the result
            var Result = TestList.PrependItemLazy(ItemToAppend).ToList();

            //make sure we have 3 items
            Assert.Equal(TestList.Count + 1, Result.Count);

            //use the concat operator to make sure we have the same results
            var ShouldBeResult = new DummyObject[] { ItemToAppend }.Concat(TestList).ToList();

            //now go test all the items
            for (int i = 0; i < ShouldBeResult.Count; i++)
            {
                //test the id
                Assert.Equal(ShouldBeResult[i].Id, Result[i].Id);

                //test the description
                Assert.Equal(ShouldBeResult[i].Description, Result[i].Description);
            }
        }

        /// <summary>
        /// Unit test for Append - list first
        /// </summary>
        [Fact]
        public void AppendSingleItemFirstTest1()
        {
            //go build the list
            var TestList = DummyObject.CreateDummyListLazy(2).ToList();

            //go grab the single item
            var ItemToAppend = DummyObject.CreateDummyRecord();

            //go build the result
            var Result = TestList.AppendItemLazy(ItemToAppend).ToList();

            //make sure we have 3 items
            Assert.Equal(TestList.Count + 1, Result.Count);

            //use the concat operator to make sure we have the same results
            var ShouldBeResult = TestList.Concat(new DummyObject[] { ItemToAppend }).ToList();

            //now go test all the items
            for (int i = 0; i < ShouldBeResult.Count; i++)
            {
                //test the id
                Assert.Equal(ShouldBeResult[i].Id, Result[i].Id);

                //test the description
                Assert.Equal(ShouldBeResult[i].Description, Result[i].Description);
            }
        }

        #endregion

        #region Parallel For Each Async

        [Fact]
        public async Task ParallelForEachAsyncTest1()
        {
            var TestSource = Enumerable.Range(0, 5).Select(x => x);
            var Bag = new ConcurrentBag<int>();

            async Task MockedAsyncMethod(int i)
            {
                await Task.Delay(10);

                var Calc = i + 100;

                Bag.Add(Calc);
            }

            await TestSource.ParallelForEachAsync(MockedAsyncMethod);

            Assert.Equal(5, Bag.Count);
            Assert.Contains(100, Bag);
            Assert.Contains(101, Bag);
            Assert.Contains(102, Bag);
            Assert.Contains(103, Bag);
            Assert.Contains(104, Bag);
        }

        [Fact]
        public async Task ParallelForEachAsyncWithMaxDegreeTest1()
        {
            var TestSource = Enumerable.Range(0, 5).Select(x => x);
            var Bag = new ConcurrentBag<int>();

            async Task MockedAsyncMethod(int i)
            {
                await Task.Delay(10);

                var Calc = i + 100;

                Bag.Add(Calc);
            }

            await TestSource.ParallelForEachAsync(MockedAsyncMethod, 20);

            Assert.Equal(5, Bag.Count);
            Assert.Contains(100, Bag);
            Assert.Contains(101, Bag);
            Assert.Contains(102, Bag);
            Assert.Contains(103, Bag);
            Assert.Contains(104, Bag);
        }

        #endregion

        #endregion

    }

}