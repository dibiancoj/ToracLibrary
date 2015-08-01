using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using ToracLibrary.Caching;
using ToracLibraryTest.Framework.DummyObjects;

namespace ToracLibraryTest.UnitsTest.Caching
{

    /// <summary>
    /// Unit test to test the in memory cache
    /// </summary>
    [TestClass]
    public class InMemoryCacheTest
    {

        #region Framework

        /// <summary>
        /// Dummy Cache For Unit Test
        /// </summary>
        public static class DummyObjectCache
        {

            /// <summary>
            /// Common method so we can have 1 method that creates the in memory cache for "DummyObjectCache"
            /// </summary>
            /// <returns>In memory cache</returns>
            public static InMemoryCache<IEnumerable<DummyObject>> BuildCache()
            {
                return new InMemoryCache<IEnumerable<DummyObject>>("DummyObjectCache", BuildCacheDataSourceLazy);
            }

            /// <summary>
            /// Get the cache item. From cache, otherwise goes back to the data source
            /// </summary>
            /// <returns>IEnumerable of dummy object</returns>
            public static IEnumerable<DummyObject> GetCacheItem()
            {
                return BuildCache().GetCacheItem();
            }

            /// <summary>
            /// Build the data source that we will put in the cache. Seperate static method so we can test this.
            /// </summary>
            /// <returns>IEnumerable of dummy objects</returns>
            public static IEnumerable<DummyObject> BuildCacheDataSourceLazy()
            {
                yield return new DummyObject { Id = 1 };
                yield return new DummyObject { Id = 2 };
            }

        }

        #endregion

        #region Unit Tests

        /// <summary>
        /// Test the in memory cache
        /// </summary>
        [TestMethod]
        public void InMemoryCacheTest1()
        {
            //we will make sure nothing is in the cache
            Assert.AreEqual(0, InMemoryCache.GetAllItemsInCacheLazy().Count());

            //grab the first item that we will test against. This should be the record "it should be"
            var RecordToCheckAgainst = DummyObjectCache.BuildCacheDataSourceLazy().First();

            //let's try to grab the first item...it should go get the item from the cache and return it
            Assert.AreEqual(RecordToCheckAgainst.Id, DummyObjectCache.GetCacheItem().ElementAt(0).Id);

            //just make sure we have that 1 item in the cache
            Assert.AreEqual(1, InMemoryCache.GetAllItemsInCacheLazy().Count());

            //let's try to clear the cache now
            DummyObjectCache.BuildCache().RemoveCacheItem();

            //make sure we have 0 records
            Assert.AreEqual(0, InMemoryCache.GetAllItemsInCacheLazy().Count());

            //let's test the refresh now (we currently don't have an item in the cache, so it should handle if it's not there!)
            DummyObjectCache.BuildCache().RefreshCacheItem();

            //that method should put the item back in... (1 element, because its only 1 cache we are using)
            Assert.AreEqual(1, InMemoryCache.GetAllItemsInCacheLazy().Count());

            //let's just make sure we have 2 elements in the array
            Assert.AreEqual(DummyObjectCache.BuildCacheDataSourceLazy().Count(), DummyObjectCache.GetCacheItem().Count());
        }

        #endregion

    }

}