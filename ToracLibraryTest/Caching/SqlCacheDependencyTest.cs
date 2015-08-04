using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ToracLibrary.Caching;
using ToracLibrary.Core.DataProviders.ADO;
using ToracLibraryTest.Framework;
using ToracLibraryTest.Framework.DummyObjects;
using ToracLibraryTest.UnitsTest.Core.DataProviders;

namespace ToracLibraryTest.UnitsTest.Caching
{

    /// <summary>
    /// Unit test to test sql cache dep.
    /// </summary>
    [TestClass]
    public class SqlCacheDependencyTest
    {

        //#region IDependency Injection Methods

        ///// <summary>
        ///// Configure the DI container for this unit test. Get's called because the class has IDependencyInject - DIUnitTestContainer.ConfigureDIContainer
        ///// </summary>
        ///// <param name="DIContainer">container to modify</param>
        //public void ConfigureDIContainer(UnityContainer DIContainer)
        //{
        //    //let's register my dummy cache container
        //    DIContainer.RegisterType<IDepInjectUnitTestCache<IEnumerable<DummyObject>>, DummyCacheWithDI<IEnumerable<DummyObject>>>(
        //        DIFactoryName,
        //        new ContainerControlledLifetimeManager(),
        //        new InjectionConstructor(CacheKeyToUse,
        //        new Func<IEnumerable<DummyObject>>(() => DummyObjectCacheNoDI.BuildCacheDataSourceLazy())));
        //}

        //#endregion

        #region Constants

        /// <summary>
        /// declare the cache key so we have it for the tests
        /// </summary>
        private const string CacheKeyToUse = "DISqlCachTestKey";

        /// <summary>
        /// di factory name for this specific cache
        /// </summary>
        private const string DIFactoryName = "DISqlFactoryInMemoryTest";

        #endregion

        /// <summary>
        /// Dummy Cache For Unit Test
        /// </summary>
        public static class DummySqlCacheObjectCacheNoDI
        {

            /// <summary>
            /// Common method so we can have 1 method that creates the in memory cache for "DummyObjectCache"
            /// </summary>
            /// <returns>In memory cache</returns>
            public static InMemoryCache<IEnumerable<DummyObject>> BuildCache()
            {
                return new SqlCacheDependency<IEnumerable<DummyObject>>("DummySqlCacheObjectCache",
                    InMemoryCacheTest.DummyObjectCacheNoDI.BuildCacheDataSourceLazy,
                    SqlDataProviderTest.ConnectionStringToUse(),
                    "dbo",
                    "select * from dbo.Ref_Test");
            }

            /// <summary>
            /// Get the cache item. From cache, otherwise goes back to the data source
            /// </summary>
            /// <returns>IEnumerable of dummy object</returns>
            public static IEnumerable<DummyObject> GetCacheItem()
            {
                return BuildCache().GetCacheItem();
            }

        }

        #region Test Methods

        /// <summary>
        /// Test sql cache dep.
        /// </summary>
        [TestMethod]
        public void SqlCacheDependencyNoDITest1()
        {
            //tear down and build up
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            //make sure we have 10 items
            Assert.AreEqual(DataProviderSetupTearDown.DefaultRecordsToInsert, DummySqlCacheObjectCacheNoDI.GetCacheItem().Count());

            //now insert 4 more records
            DataProviderSetupTearDown.AddRows(4);

            //we need to try to wait until sql cache dep event is raised...otherwise we will get false blowups.
            //because it will raise for every record inserted. so just try to wait a second then go grab the data and check
            Thread.SpinWait(10000000);

            //make sure we have 14 records
            Assert.AreEqual(14, DummySqlCacheObjectCacheNoDI.GetCacheItem().Count());

            //now insert 4 more records
            DataProviderSetupTearDown.AddRows(4);

            //we need to try to wait until sql cache dep event is raised...otherwise we will get false blowups.
            //because it will raise for every record inserted. so just try to wait a second then go grab the data and check
            Thread.SpinWait(10000000);

            //make sure we have 18 records
            Assert.AreEqual(18, DummySqlCacheObjectCacheNoDI.GetCacheItem().Count());

            //call refresh and let's check to make sure it works
            DummySqlCacheObjectCacheNoDI.BuildCache().RemoveCacheItem();

            //we need to try to wait until sql cache dep event is raised...otherwise we will get false blowups.
            //because it will raise for every record inserted. so just try to wait a second then go grab the data and check
            Thread.SpinWait(10000000);

            //make sure we have 18 records
            Assert.AreEqual(18, DummySqlCacheObjectCacheNoDI.GetCacheItem().Count());

            //now insert 2 more records
            DataProviderSetupTearDown.AddRows(2);

            //we need to try to wait until sql cache dep event is raised...otherwise we will get false blowups.
            //because it will raise for every record inserted. so just try to wait a second then go grab the data and check
            Thread.SpinWait(10000000);

            //make sure we have 20 records
            Assert.AreEqual(20, DummySqlCacheObjectCacheNoDI.GetCacheItem().Count());
        }

        #endregion

    }

}