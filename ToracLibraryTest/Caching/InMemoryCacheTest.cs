using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using ToracLibrary.Caching;
using ToracLibraryTest.Framework;
using ToracLibraryTest.Framework.DummyObjects;

namespace ToracLibraryTest.UnitsTest.Caching
{

    /// <summary>
    /// Unit test to test the in memory cache
    /// </summary>
    [TestClass]
    public class InMemoryCacheTest : IDependencyInject
    {

        #region IDependency Injection Methods

        /// <summary>
        /// Configure the DI container for this unit test. Get's called because the class has IDependencyInject - DIUnitTestContainer.ConfigureDIContainer
        /// </summary>
        /// <param name="DIContainer">container to modify</param>
        public void ConfigureDIContainer(UnityContainer DIContainer)
        {
            //let's register my dummy cache container
            DIContainer.RegisterType<ICacheImplementation<IEnumerable<DummyObject>>, InMemoryCache<IEnumerable<DummyObject>>>(
                DIFactoryName,
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(CacheKeyToUse, new Func<IEnumerable<DummyObject>>(() => DummyObjectCacheNoDI.BuildCacheDataSource())));
        }

        #endregion

        #region Constants

        /// <summary>
        /// declare the cache key so we have it for the tests
        /// </summary>
        private const string CacheKeyToUse = "DICachTestKey";

        /// <summary>
        /// di factory name for this specific cache
        /// </summary>
        private const string DIFactoryName = "DIFactoryInMemoryTest";

        #endregion

        #region Framework

        /// <summary>
        /// Dummy Cache For Unit Test
        /// </summary>
        public static class DummyObjectCacheNoDI
        {

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
                DataSourceForCache.Add(new DummyObject { Id = 1 });
                DataSourceForCache.Add(new DummyObject { Id = 2 });

                //return the list now
                return DataSourceForCache;
            }

        }

        #endregion

        #region Unit Tests

        /// <summary>
        /// Test the in memory cache
        /// </summary>
        [TestMethod]
        public void InMemoryCacheTestWithNoDependencyInjection1()
        {
            //we will make sure nothing is in the cache
            Assert.AreEqual(0, InMemoryCache.GetAllItemsInCacheLazy().Count());

            //grab the first item that we will test against. This should be the record "it should be"
            var RecordToCheckAgainst = DummyObjectCacheNoDI.BuildCacheDataSource().First();

            //let's try to grab the first item...it should go get the item from the cache and return it
            Assert.AreEqual(RecordToCheckAgainst.Id, DummyObjectCacheNoDI.GetCacheItem().ElementAt(0).Id);

            //just make sure we have that 1 item in the cache
            Assert.AreEqual(1, InMemoryCache.GetAllItemsInCacheLazy().Count());

            //let's try to clear the cache now
            DummyObjectCacheNoDI.BuildCache().RemoveCacheItem();

            //make sure we have 0 records
            Assert.AreEqual(0, InMemoryCache.GetAllItemsInCacheLazy().Count());

            //let's test the refresh now (we currently don't have an item in the cache, so it should handle if it's not there!)
            DummyObjectCacheNoDI.BuildCache().RefreshCacheItem();

            //that method should put the item back in... (1 element, because its only 1 cache we are using)
            Assert.AreEqual(1, InMemoryCache.GetAllItemsInCacheLazy().Count());

            //let's just make sure we have 2 elements in the array
            Assert.AreEqual(DummyObjectCacheNoDI.BuildCacheDataSource().Count(), DummyObjectCacheNoDI.GetCacheItem().Count());
        }

        /// <summary>
        /// Test the in memory cache using a DI container
        /// </summary>
        [TestMethod]
        public void InMemoryCacheTestWithDependencyInjection1()
        {
            //declare a func so we can just count how many items we have for just this cache (other cache unit tests might get in the way)
            Func<KeyValuePair<string, object>, bool> OnlyThisCache = x => x.Key == CacheKeyToUse;

            //let's go get my factory from my DI Container
            var CacheFromDIContainer = DIUnitTestContainer.DIContainer.Resolve<ICacheImplementation<IEnumerable<DummyObject>>>(DIFactoryName);

            //we will make sure nothing is in the cache
            Assert.AreEqual(0, InMemoryCache.GetAllItemsInCacheLazy().Count(OnlyThisCache));

            //grab the first item that we will test against. This should be the record "it should be"
            var RecordToCheckAgainst = CacheFromDIContainer.FunctionToGetDataFromSource.Invoke().First();

            //let's try to grab the first item...it should go get the item from the cache and return it
            Assert.AreEqual(RecordToCheckAgainst.Id, CacheFromDIContainer.GetCacheItem().ElementAt(0).Id);

            //just make sure we have that 1 item in the cache
            Assert.AreEqual(1, InMemoryCache.GetAllItemsInCacheLazy().Count(OnlyThisCache));

            //let's try to clear the cache now
            CacheFromDIContainer.RemoveCacheItem();

            //make sure we have 0 records
            Assert.AreEqual(0, InMemoryCache.GetAllItemsInCacheLazy().Count(OnlyThisCache));

            //let's test the refresh now (we currently don't have an item in the cache, so it should handle if it's not there!)
            CacheFromDIContainer.RefreshCacheItem();

            //that method should put the item back in... (1 element, because its only 1 cache we are using)
            Assert.AreEqual(1, InMemoryCache.GetAllItemsInCacheLazy().Count(OnlyThisCache));

            //let's just make sure we have 2 elements in the array
            Assert.AreEqual(CacheFromDIContainer.FunctionToGetDataFromSource.Invoke().Count(), CacheFromDIContainer.GetCacheItem().Count());
        }

        #endregion

    }

}