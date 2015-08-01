using Microsoft.Practices.Unity;
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
        public static class DummyObjectCacheNoDI
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

        /// <summary>
        /// DI Interface for cache
        /// </summary>
        public interface IDepInjectUnitTestCache<T>
        {
            InMemoryCache<T> Cache { get; }
            Func<T> BuildDataSource { get; }
        }

        /// <summary>
        /// Dummy Cache For Unit Test With Dep. Injection
        /// </summary>
        public class DummyCacheWithDI<T> : IDepInjectUnitTestCache<T>
        {

            #region Constructor

            public DummyCacheWithDI(string KeyForCache, Func<T> BuildDataSourceForCache)
            {
                //set the factory for the cache
                Cache = new InMemoryCache<T>(KeyForCache, BuildDataSourceForCache);

                //set the func that will populate this data key
                BuildDataSource = BuildDataSourceForCache;
            }

            #endregion

            #region Properties

            public InMemoryCache<T> Cache { get; }
            public Func<T> BuildDataSource { get; }

            #endregion

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
            var RecordToCheckAgainst = DummyObjectCacheNoDI.BuildCacheDataSourceLazy().First();

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
            Assert.AreEqual(DummyObjectCacheNoDI.BuildCacheDataSourceLazy().Count(), DummyObjectCacheNoDI.GetCacheItem().Count());
        }

        /// <summary>
        /// Test the in memory cache using a DI container
        /// </summary>
        [TestMethod]
        public void InMemoryCacheTestWithDependencyInjection1()
        {
            //let's go create unity DI container
            using (var DIContainer = new UnityContainer())
            {
                //declare the cache key so we have it for the tests
                const string CacheKeyToUse = "DICachTestKey";

                //di factory name for this specific cache
                const string DIFactoryName = "DIFactoryInMemoryTest";

                //declare a func so we can just count how many items we have for just this cache (other cache unit tests might get in the way)
                Func<KeyValuePair<string, object>, bool> OnlyThisCache = x => x.Key == CacheKeyToUse;

                //let's register my dummy cache container
                DIContainer.RegisterType<IDepInjectUnitTestCache<IEnumerable<DummyObject>>, DummyCacheWithDI<IEnumerable<DummyObject>>>(
                    DIFactoryName,
                    new ContainerControlledLifetimeManager(),
                    new InjectionConstructor(CacheKeyToUse,
                    new Func<IEnumerable<DummyObject>>(() => DummyObjectCacheNoDI.BuildCacheDataSourceLazy())));

                //let's go get my factory from my DI Container
                var CacheFromDIContainer = DIContainer.Resolve<DummyCacheWithDI<IEnumerable<DummyObject>>>(DIFactoryName);

                //we will make sure nothing is in the cache
                Assert.AreEqual(0, InMemoryCache.GetAllItemsInCacheLazy().Count(OnlyThisCache));

                //grab the first item that we will test against. This should be the record "it should be"
                var RecordToCheckAgainst = CacheFromDIContainer.BuildDataSource().First();

                //let's try to grab the first item...it should go get the item from the cache and return it
                Assert.AreEqual(RecordToCheckAgainst.Id, CacheFromDIContainer.Cache.GetCacheItem().ElementAt(0).Id);

                //just make sure we have that 1 item in the cache
                Assert.AreEqual(1, InMemoryCache.GetAllItemsInCacheLazy().Count(OnlyThisCache));

                //let's try to clear the cache now
                CacheFromDIContainer.Cache.RemoveCacheItem();

                //make sure we have 0 records
                Assert.AreEqual(0, InMemoryCache.GetAllItemsInCacheLazy().Count(OnlyThisCache));

                //let's test the refresh now (we currently don't have an item in the cache, so it should handle if it's not there!)
                CacheFromDIContainer.Cache.RefreshCacheItem();

                //that method should put the item back in... (1 element, because its only 1 cache we are using)
                Assert.AreEqual(1, InMemoryCache.GetAllItemsInCacheLazy().Count(OnlyThisCache));

                //let's just make sure we have 2 elements in the array
                Assert.AreEqual(CacheFromDIContainer.BuildDataSource().Count(), CacheFromDIContainer.Cache.GetCacheItem().Count());
            }
        }

        #endregion

    }

}