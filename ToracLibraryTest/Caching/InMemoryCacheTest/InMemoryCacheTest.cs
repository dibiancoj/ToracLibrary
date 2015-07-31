using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using ToracLibrary.Caching;
using ToracLibrary.Caching.BaseClass;
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
        /// Dummy Cache Record
        /// </summary>
        private class DummyObjectCache : InMemoryCache<IEnumerable<DummyObject>>
        {

            #region Constructor

            /// <summary>
            /// Constructor
            /// </summary>
            public DummyObjectCache() : base("DummyObjectCache")
            {
            }

            #endregion

            #region Implementation Methods

            /// <summary>
            /// Grab the record from the data source
            /// </summary>
            /// <returns></returns>
            protected override IEnumerable<DummyObject> GetDataFromDataSource()
            {
                return BuildCacheDataSourceLazy().ToArray();
            }

            #endregion

            #region Static Methods So We Can Test This

            /// <summary>
            /// Build the data source that we will put in the cache. Seperate static method so we can test this.
            /// </summary>
            /// <returns>IEnumerable of dummy objects</returns>
            internal static IEnumerable<DummyObject> BuildCacheDataSourceLazy()
            {
                yield return new DummyObject { Id = 1 };
                yield return new DummyObject { Id = 2 };
            }

            #endregion

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
            Assert.AreEqual(0, DummyObjectCache.GetAllItemsInCacheLazy().Count());

            //let's create a new cache object for this specific cache
            var CacheToUse = new DummyObjectCache();

            //grab the first item that we will test against. This should be the record "it should be"
            var RecordToCheckAgainst = DummyObjectCache.BuildCacheDataSourceLazy().First();

            //let's try to grab the first item...it should go get the item from the cache and return it
            Assert.AreEqual(RecordToCheckAgainst.Id, CacheToUse.GetCacheItem().ElementAt(0).Id);

            //just make sure we have that 1 item in the cache
            Assert.AreEqual(1, DummyObjectCache.GetAllItemsInCacheLazy().Count());

            //let's try to clear the cache now
            CacheToUse.RemoveCacheItem();

            //make sure we have 0 records
            Assert.AreEqual(0, DummyObjectCache.GetAllItemsInCacheLazy().Count());

            //let's test the refresh now (we currently don't have an item in the cache, so it should handle if it's not there!)
            CacheToUse.RefreshCacheItem();

            //that method should put the item back in... (1 element, because its only 1 cache we are using)
            Assert.AreEqual(1, DummyObjectCache.GetAllItemsInCacheLazy().Count());

            //let's just make sure we have 2 elements in the array
            Assert.AreEqual(DummyObjectCache.BuildCacheDataSourceLazy().Count(), CacheToUse.GetCacheItem().Count());
        }

        #endregion

    }

}