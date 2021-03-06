﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToracLibrary.Core.ExtensionMethods.IDictionaryExtensions;
using ToracLibrary.UnitTest.Framework;
using Xunit;

namespace ToracLibrary.UnitTest.ExtensionMethods.Core
{

    /// <summary>
    /// Unit test to IDictionary Extension Methods
    /// </summary>
    public class IDictionaryExtensionTest
    {

        #region Try Add

        /// <summary>
        /// Unit test for try add to a dictionary
        /// </summary>
        [Fact]
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
            Assert.False(TestDictionary.ContainsKey(24));
            Assert.True(TestDictionary.TryAdd(24, new DummyObject(24, "24")));
            Assert.True(TestDictionary.ContainsKey(24));

            //now check to see we get the correct result for that item we just added
            Assert.True(TestDictionary.ContainsKey(24));
            Assert.False(TestDictionary.TryAdd(24, new DummyObject(24, "24")));
            Assert.Single(TestDictionary);

            //try to add another new item
            Assert.False(TestDictionary.ContainsKey(25));
            Assert.True(TestDictionary.TryAdd(25, new DummyObject(25, "25")));
            Assert.True(TestDictionary.ContainsKey(25));

            //make sure 24 is still in the dictionary
            Assert.True(TestDictionary.ContainsKey(24));

            //make sure we have 2 items
            Assert.Equal(2, TestDictionary.Count);
        }

        #endregion

        #region Try Get

        /// <summary>
        /// Unit test for try get to a dictionary
        /// </summary>
        [Fact]
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
            Assert.Null(TestDictionary.TryGet(24));

            //let's add an item
            TestDictionary.Add(24, new DummyObject(24, "24"));

            //make sure we have this item now
            Assert.Equal(24, TestDictionary.TryGet(24).Id);

            //make sure we can't find 25
            Assert.Null(TestDictionary.TryGet(25));

            //now add 25
            TestDictionary.Add(25, new DummyObject(25, "25"));

            //make sure we can find 25 now
            Assert.Equal(25, TestDictionary.TryGet(25).Id);
        }

        #endregion

        #region Get Or Add

        /// <summary>
        /// Unit test for GetOrAdd to a dictionary.
        /// </summary>
        [Fact]
        public void GetOrAddTest1()
        {
            //how many times this has been created
            int HowManyTimesCreated = 0;

            //value to use to test
            const int UniqueId = 9999;

            //create a test dictionary which we will use
            var TestDictionary = new Dictionary<int, DummyObject>();

            //try to get it. It shouldn't be found...so we will return the creator (Xunit has the same method. namespace issues so calling it with class name)
            var Result = IDictionaryExtensionMethods.GetOrAdd(TestDictionary, UniqueId, () =>
            {
                //increase the tally
                HowManyTimesCreated++;

                //return the object
                return new DummyObject(UniqueId, UniqueId.ToString());
            });

            //test the entry
            Assert.Equal(UniqueId, TestDictionary[UniqueId].Id);

            //now make sure if we try to add the same item that we don't throw the exception
            Assert.Equal(UniqueId, TestDictionary.GetOrAdd(UniqueId, () => throw new IndexOutOfRangeException("This shouldn't Be Called")).Id);

            //make sure we only every call the method once
            Assert.Equal(1, HowManyTimesCreated);
        }

        #endregion

    }

}