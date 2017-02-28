using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using ToracLibrary.Caching;
using ToracLibrary.Core.DataProviders.ADO;
using ToracLibrary.UnitTest.Core.DataProviders;
using ToracLibrary.UnitTest.Core.DataProviders.EntityFrameworkDP;
using ToracLibrary.UnitTest.Framework;
using Xunit;

namespace ToracLibrary.UnitTest.Caching
{

    /// <summary>
    /// Unit test to test sql cache dep.
    /// </summary>
    [Collection("DatabaseUnitTests")]
    public class SqlCacheDependencyTest
    {

        #region Constants

        /// <summary>
        /// declare the cache key so we have it for the tests
        /// </summary>
        internal const string CacheKeyToUse = "DISqlCacheTestKey";

        /// <summary>
        /// di factory name for this specific cache
        /// </summary>
        internal const string DIFactoryName = "DISqlFactoryInMemoryTest";

        /// <summary>
        /// Database schema use to trigger a refresh of the cache
        /// </summary>
        internal const string DatabaseSchemaUsedForCacheRefresh = "dbo";

        /// <summary>
        /// Sql to use to refresh the cache
        /// </summary>
        internal const string CacheSqlToUseToTriggerRefresh = "select * from dbo.Ref_SqlCacheTrigger";

        #endregion

        #region Static Helper Methods

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
            /// insert into the sql cache record
            /// </summary>
            public static void UpdateSqlCache()
            {
                //create the data provider
                using (var DP = DIUnitTestContainer.DIContainer.Resolve<IDataProvider>())
                {
                    DP.ExecuteNonQuery($"Insert into dbo.Ref_SqlCacheTrigger(LastUpdatedDate) values('{DateTime.Now}')", CommandType.Text);
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
                        DataSourceInCache.Add(new DummyObject(Convert.ToInt32(DataRowFound["Id"]), null));
                    }
                }

                //return the list
                return DataSourceInCache;
            }

        }

        #endregion

        #region Test Methods

        /// <summary>
        /// Test sql cache dep.
        /// </summary>
        [Fact(Skip = DisableSpecificUnitTestAreas.DatabaseAvailableForUnitTestFlag)]
        public void SqlCacheDependencyNoDITest1()
        {
            //cleanup the table
            CleanupTable();

            //how many records to add
            const int RecordsToAdd = 1;

            //tear down and build up
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            //make sure we have 10 items in the table
            Assert.Equal(DataProviderSetupTearDown.DefaultRecordsToInsert, DummySqlCacheObjectCacheNoDI.Cache.GetCacheItem().Count());

            //insert some rows
            DataProviderSetupTearDown.AddRows(RecordsToAdd, false);

            //now we just added some rows in the table (but we have not updated the trigger table...so we should still have the same amount of records)
            Assert.Equal(DataProviderSetupTearDown.DefaultRecordsToInsert, DummySqlCacheObjectCacheNoDI.Cache.GetCacheItem().Count());

            //now we want to trigger the cache and grab the changes
            DummySqlCacheObjectCacheNoDI.UpdateSqlCache();

            //we need to try to wait until sql cache dep event is raised...otherwise we will get false blowups.
            //because it will raise for every record inserted. so just try to wait a second then go grab the data and check
            Thread.SpinWait(10000000);

            //cache should be reset now...should be 14
            Assert.Equal(DataProviderSetupTearDown.DefaultRecordsToInsert + RecordsToAdd, DummySqlCacheObjectCacheNoDI.Cache.GetCacheItem().Count());
        }

        /// <summary>
        /// Test the sql cache dep using a DI container
        /// </summary>
        [Fact(Skip = DisableSpecificUnitTestAreas.DatabaseAvailableForUnitTestFlag)]
        public void SqlCacheDependencyWithDependencyInjectionTest1()
        {
            //cleanup the table
            CleanupTable();

            //let's go get my factory from my DI Container
            var CacheFromDIContainer = DIUnitTestContainer.DIContainer.Resolve<ICacheImplementation<IEnumerable<DummyObject>>>(DIFactoryName);

            //how many records to add
            const int RecordsToAdd = 1;

            //tear down and build up
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            //make sure we have 10 items in the table
            Assert.Equal(DataProviderSetupTearDown.DefaultRecordsToInsert, CacheFromDIContainer.GetCacheItem().Count());

            //insert some rows
            DataProviderSetupTearDown.AddRows(RecordsToAdd, false);

            //now we just added some rows in the table (but we have not updated the trigger table...so we should still have the same amount of records)
            Assert.Equal(DataProviderSetupTearDown.DefaultRecordsToInsert, CacheFromDIContainer.GetCacheItem().Count());

            //now we want to trigger the cache and grab the changes
            DummySqlCacheObjectCacheNoDI.UpdateSqlCache();

            //we need to try to wait until sql cache dep event is raised...otherwise we will get false blowups.
            //because it will raise for every record inserted. so just try to wait a second then go grab the data and check
            Thread.SpinWait(10000000);

            //cache should be reset now...should be 14
            Assert.Equal(DataProviderSetupTearDown.DefaultRecordsToInsert + RecordsToAdd, CacheFromDIContainer.GetCacheItem().Count());
        }

        #endregion

        #region Helper Methods

        private void CleanupTable()
        {
            //just cleanup the table.
            using (var DP = DIUnitTestContainer.DIContainer.Resolve<IDataProvider>())
            {
                DP.ExecuteNonQuery("Truncate Table dbo.Ref_SqlCacheTrigger", CommandType.Text);
            }
        }

        #endregion

    }

}