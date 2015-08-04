using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
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
    public class SqlCacheDependencyTest : IDependencyInject
    {

        #region IDependency Injection Methods

        /// <summary>
        /// Configure the DI container for this unit test. Get's called because the class has IDependencyInject - DIUnitTestContainer.ConfigureDIContainer
        /// </summary>
        /// <param name="DIContainer">container to modify</param>
        public void ConfigureDIContainer(UnityContainer DIContainer)
        {
            //let's register my dummy cache container
            DIContainer.RegisterType<ICacheImplementation<IEnumerable<DummyObject>>, SqlCacheDependency<IEnumerable<DummyObject>>>(
                DIFactoryName,
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(CacheKeyToUse, new Func<IEnumerable<DummyObject>>(() => DummySqlCacheObjectCacheNoDI.BuildCacheDataSource()), SqlDataProviderTest.ConnectionStringToUse(), DatabaseSchemaUsedForCacheRefresh, CacheSqlToUseToTriggerRefresh));
        }

        #endregion

        #region Constants

        /// <summary>
        /// declare the cache key so we have it for the tests
        /// </summary>
        private const string CacheKeyToUse = "DISqlCachTestKey";

        /// <summary>
        /// di factory name for this specific cache
        /// </summary>
        private const string DIFactoryName = "DISqlFactoryInMemoryTest";

        /// <summary>
        /// Database schema use to trigger a refresh of the cache
        /// </summary>
        private const string DatabaseSchemaUsedForCacheRefresh = "dbo";

        /// <summary>
        /// Sql to use to refresh the cache
        /// </summary>
        private const string CacheSqlToUseToTriggerRefresh = "select * from dbo.Ref_SqlCachTrigger";

        #endregion

        /// <summary>
        /// Dummy Cache For Unit Test
        /// </summary>
        public static class DummySqlCacheObjectCacheNoDI
        {

            #region Static Constructor

            static DummySqlCacheObjectCacheNoDI()
            {
                //create the new cache object
                Cache = new SqlCacheDependency<IEnumerable<DummyObject>>("DummySqlCacheObjectCache",
                        BuildCacheDataSource,
                        SqlDataProviderTest.ConnectionStringToUse(),
                        DatabaseSchemaUsedForCacheRefresh,
                        CacheSqlToUseToTriggerRefresh);
            }

            #endregion

            #region Static Properties

            /// <summary>
            /// Holds the cache in a static object so we don't have to keep creating the cache
            /// </summary>
            public static InMemoryCache<IEnumerable<DummyObject>> Cache { get; }

            #endregion

            /// <summary>
            /// Get the cache item. From cache, otherwise goes back to the data source
            /// </summary>
            /// <returns>IEnumerable of dummy object</returns>
            public static IEnumerable<DummyObject> GetCacheItem()
            {
                return Cache.GetCacheItem();
            }

            /// <summary>
            /// insert into the sql cache record
            /// </summary>
            public static void UpdateSqlCache()
            {
                //create the data provider
                using (var DP = DIUnitTestContainer.DIContainer.Resolve<IDataProvider>())
                {
                    DP.ExecuteNonQuery($"Insert into dbo.Ref_SqlCachTrigger(LastUpdatedDate) values('{DateTime.Now}')", CommandType.Text);
                }
            }

            /// <summary>
            /// Build the data source that we will put in the cache. Seperate static method so we can test this.
            /// </summary>
            /// <returns>IEnumerable of dummy objects</returns>
            public static IEnumerable<DummyObject> BuildCacheDataSource()
            {
                //i don't want an iterator in the cache, not using yield return
                var DataSourceInCache = new List<DummyObject>();

                //create the data provider
                using (var DP = DIUnitTestContainer.DIContainer.Resolve<IDataProvider>())
                {
                    //go grab the data set
                    var DataSetToTest = DP.GetDataSet("SELECT * FROM Ref_Test", CommandType.Text);

                    //loop through each row and return it
                    foreach (DataRow DataRowFound in DataSetToTest.Tables[0].Rows)
                    {
                        DataSourceInCache.Add(new DummyObject { Id = Convert.ToInt32(DataRowFound["Id"]) });
                    }
                }

                //return the list
                return DataSourceInCache;
            }

        }

        #region Test Methods

        /// <summary>
        /// Test sql cache dep.
        /// </summary>
        [TestMethod]
        public void SqlCacheDependencyNoDITest1()
        {
            //how many records to add
            const int RecordsToAdd = 1;

            //tear down and build up
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            //make sure we have 10 items in the table
            Assert.AreEqual(DataProviderSetupTearDown.DefaultRecordsToInsert, DummySqlCacheObjectCacheNoDI.GetCacheItem().Count());

            //insert some rows
            DataProviderSetupTearDown.AddRows(RecordsToAdd, false);

            //now we just added some rows in the table (but we have not updated the trigger table...so we should still have the same amount of records)
            Assert.AreEqual(DataProviderSetupTearDown.DefaultRecordsToInsert, DummySqlCacheObjectCacheNoDI.GetCacheItem().Count());

            //now we want to trigger the cache and grab the changes
            DummySqlCacheObjectCacheNoDI.UpdateSqlCache();

            //we need to try to wait until sql cache dep event is raised...otherwise we will get false blowups.
            //because it will raise for every record inserted. so just try to wait a second then go grab the data and check
            Thread.SpinWait(10000000);

            //cache should be reset now...should be 14
            Assert.AreEqual(DataProviderSetupTearDown.DefaultRecordsToInsert + RecordsToAdd, DummySqlCacheObjectCacheNoDI.GetCacheItem().Count());
        }

        /// <summary>
        /// Test the sql cache dep using a DI container
        /// </summary>
        [TestMethod]
        public void SqlCacheDependencyWithDependencyInjection1()
        {
            //let's go get my factory from my DI Container
            var CacheFromDIContainer = DIUnitTestContainer.DIContainer.Resolve<ICacheImplementation<IEnumerable<DummyObject>>>(DIFactoryName);

            //how many records to add
            const int RecordsToAdd = 1;

            //tear down and build up
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            //make sure we have 10 items in the table
            Assert.AreEqual(DataProviderSetupTearDown.DefaultRecordsToInsert, CacheFromDIContainer.GetCacheItem().Count());

            //insert some rows
            DataProviderSetupTearDown.AddRows(RecordsToAdd, false);

            //now we just added some rows in the table (but we have not updated the trigger table...so we should still have the same amount of records)
            Assert.AreEqual(DataProviderSetupTearDown.DefaultRecordsToInsert, CacheFromDIContainer.GetCacheItem().Count());

            //now we want to trigger the cache and grab the changes
            DummySqlCacheObjectCacheNoDI.UpdateSqlCache();

            //we need to try to wait until sql cache dep event is raised...otherwise we will get false blowups.
            //because it will raise for every record inserted. so just try to wait a second then go grab the data and check
            Thread.SpinWait(10000000);

            //cache should be reset now...should be 14
            Assert.AreEqual(DataProviderSetupTearDown.DefaultRecordsToInsert + RecordsToAdd, CacheFromDIContainer.GetCacheItem().Count());
        }

        #endregion

    }

}