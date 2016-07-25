using System;
using System.Collections.Generic;
using System.Linq;
using ToracLibrary.Caching;
using ToracLibrary.UnitTest.Framework;
using Xunit;

namespace ToracLibrary.UnitTest.Caching
{

    /// <summary>
    /// Unit test to test the in memory cache
    /// </summary>
    public class InMemoryCacheTest
    {

        #region Constants

        /// <summary>
        /// declare the cache key so we have it for the tests
        /// </summary>
        internal const string CacheKeyToUse = "DICachTestKey";

        /// <summary>
        /// di factory name for this specific cache
        /// </summary>
        internal const string DIFactoryName = "DIFactoryInMemoryTest";

        #endregion

        #region Framework

        /// <summary>
        /// Dummy Cache For Unit Test
        /// </summary>
        public static class DummyObjectCacheNoDI
        {

            #region Constants

            /// <summary>
            /// cache key to use
            /// </summary>
            internal const string DummyObjectNoDiCacheKey = "DummyObjectCache";

            #endregion

            #region Static Methods

            /// <summary>
            /// Common method so we can have 1 method that creates the in memory cache for "DummyObjectCache"
            /// </summary>
            /// <returns>In memory cache</returns>
            public static InMemoryCache<IEnumerable<DummyObject>> BuildCache()
            {
                return new InMemoryCache<IEnumerable<DummyObject>>("DummyObjectCache", BuildCacheDataSource);
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
            public static IEnumerable<DummyObject> BuildCacheDataSource()
            {
                //using a list because i don't want an iterator in the cache
                var DataSourceForCache = new List<DummyObject>();

                //add the cache items
                DataSourceForCache.Add(new DummyObject(1, null));
                DataSourceForCache.Add(new DummyObject(2, null));

                //return the list now
                return DataSourceForCache;
            }

            #endregion

        }

        /// <summary>
        /// More of an example of how to implement the cache with a different style then above
        /// </summary>
        public class DummyObjectCacheDifferentStyleSyntax : InMemoryCache<IEnumerable<DummyObject>>
        {

            #region Constructor

            /// <summary>
            /// Constructor
            /// </summary>
            public DummyObjectCacheDifferentStyleSyntax()
                : base(DifferentStyleSyntaxCacheKey, BuildCacheFromDataSource)
            {
            }

            #endregion

            #region Constants

            /// <summary>
            /// Key for this cache
            /// </summary>
            internal const string DifferentStyleSyntaxCacheKey = "DummyObjectCache2";

            #endregion

            #region Get From Data Source Implementation

            /// <summary>
            /// Grabs the data from the source and returns it
            /// </summary>
            /// <returns>list of dummy object</returns>
            public static IEnumerable<DummyObject> BuildCacheFromDataSource()
            {
                //using a list because i don't want an iterator in the cache
                var DataSourceForCache = new List<DummyObject>();

                //add the cache items
                DataSourceForCache.Add(new DummyObject(1, null));
                DataSourceForCache.Add(new DummyObject(2, null));

                //return the list now
                return DataSourceForCache;
            }

            #endregion

        }

        #endregion

        #region Unit Tests

        /// <summary>
        /// Test the in memory cache
        /// </summary>
        [Fact]
        public void InMemoryCacheTestWithNoDependencyInjectionTest1()
        {
            //func to limit just the data in this cache
            Func<KeyValuePair<string, object>, bool> OnlyThisCache = x => x.Key == DummyObjectCacheNoDI.DummyObjectNoDiCacheKey;

            //we will make sure nothing is in the cache
            Assert.Equal(0, InMemoryCache.GetAllItemsInCacheLazy().Count(OnlyThisCache));

            //grab the first item that we will test against. This should be the record "it should be"
            var RecordToCheckAgainst = DummyObjectCacheNoDI.BuildCacheDataSource().First();

            //let's try to grab the first item...it should go get the item from the cache and return it
            Assert.Equal(RecordToCheckAgainst.Id, DummyObjectCacheNoDI.GetCacheItem().ElementAt(0).Id);

            //just make sure we have that 1 item in the cache
            Assert.Equal(1, InMemoryCache.GetAllItemsInCacheLazy().Count(OnlyThisCache));

            //let's try to clear the cache now
            DummyObjectCacheNoDI.BuildCache().RemoveCacheItem();

            //make sure we have 0 records
            Assert.Equal(0, InMemoryCache.GetAllItemsInCacheLazy().Count(OnlyThisCache));

            //let's test the refresh now (we currently don't have an item in the cache, so it should handle if it's not there!)
            DummyObjectCacheNoDI.BuildCache().RefreshCacheItem();

            //that method should put the item back in... (1 element, because its only 1 cache we are using)
            Assert.Equal(1, InMemoryCache.GetAllItemsInCacheLazy().Count(OnlyThisCache));

            //let's just make sure we have 2 elements in the array
            Assert.Equal(DummyObjectCacheNoDI.BuildCacheDataSource().Count(), DummyObjectCacheNoDI.GetCacheItem().Count());
        }

        /// <summary>
        /// Test the in memory cache using the different sytanx apporac
        /// </summary>
        [Fact]
        public void InMemoryCacheTestWithDifferentSytnaxApproachTest1()
        {
            //let's grab an instance to the cache using the static method
            var CacheBuilder = new DummyObjectCacheDifferentStyleSyntax();

            //func to limit just the data in this cache
            Func<KeyValuePair<string, object>, bool> OnlyThisCache = x => x.Key == DummyObjectCacheDifferentStyleSyntax.DifferentStyleSyntaxCacheKey;

            //we will make sure nothing is in the cache
            Assert.Equal(0, InMemoryCache.GetAllItemsInCacheLazy().Count(OnlyThisCache));

            //grab the first item that we will test against. This should be the record "it should be"
            var RecordToCheckAgainst = DummyObjectCacheDifferentStyleSyntax.BuildCacheFromDataSource().First();

            //let's try to grab the first item...it should go get the item from the cache and return it
            Assert.Equal(RecordToCheckAgainst.Id, CacheBuilder.GetCacheItem().ElementAt(0).Id);

            //just make sure we have that 1 item in the cache
            Assert.Equal(1, InMemoryCache.GetAllItemsInCacheLazy().Count(OnlyThisCache));

            //let's try to clear the cache now
            CacheBuilder.RemoveCacheItem();

            //make sure we have 0 records
            Assert.Equal(0, InMemoryCache.GetAllItemsInCacheLazy().Count(OnlyThisCache));

            //let's test the refresh now (we currently don't have an item in the cache, so it should handle if it's not there!)
            CacheBuilder.RefreshCacheItem();

            //that method should put the item back in... (1 element, because its only 1 cache we are using)
            Assert.Equal(1, InMemoryCache.GetAllItemsInCacheLazy().Count(OnlyThisCache));

            //let's just make sure we have 2 elements in the array
            Assert.Equal(DummyObjectCacheDifferentStyleSyntax.BuildCacheFromDataSource().Count(), CacheBuilder.GetCacheItem().Count());
        }

        /// <summary>
        /// Test the in memory cache using a DI container
        /// </summary>
        [Fact]
        public void InMemoryCacheTestWithDependencyInjectionTest1()
        {
            //declare a func so we can just count how many items we have for just this cache (other cache unit tests might get in the way)
            Func<KeyValuePair<string, object>, bool> OnlyThisCache = x => x.Key == CacheKeyToUse;

            //let's go get my factory from my DI Container
            var CacheFromDIContainer = DIUnitTestContainer.DIContainer.Resolve<ICacheImplementation<IEnumerable<DummyObject>>>(DIFactoryName);

            //we will make sure nothing is in the cache
            Assert.Equal(0, InMemoryCache.GetAllItemsInCacheLazy().Count(OnlyThisCache));

            //grab the first item that we will test against. This should be the record "it should be"
            var RecordToCheckAgainst = CacheFromDIContainer.FunctionToGetDataFromSource.Invoke().First();

            //let's try to grab the first item...it should go get the item from the cache and return it
            Assert.Equal(RecordToCheckAgainst.Id, CacheFromDIContainer.GetCacheItem().ElementAt(0).Id);

            //just make sure we have that 1 item in the cache
            Assert.Equal(1, InMemoryCache.GetAllItemsInCacheLazy().Count(OnlyThisCache));

            //let's try to clear the cache now
            CacheFromDIContainer.RemoveCacheItem();

            //make sure we have 0 records
            Assert.Equal(0, InMemoryCache.GetAllItemsInCacheLazy().Count(OnlyThisCache));

            //let's test the refresh now (we currently don't have an item in the cache, so it should handle if it's not there!)
            CacheFromDIContainer.RefreshCacheItem();

            //that method should put the item back in... (1 element, because its only 1 cache we are using)
            Assert.Equal(1, InMemoryCache.GetAllItemsInCacheLazy().Count(OnlyThisCache));

            //let's just make sure we have 2 elements in the array
            Assert.Equal(CacheFromDIContainer.FunctionToGetDataFromSource.Invoke().Count(), CacheFromDIContainer.GetCacheItem().Count());
        }

        #endregion

    }

}