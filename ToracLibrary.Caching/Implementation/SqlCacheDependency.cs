using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.DataProviders.ADO;
using ToracLibrary.Core.ExtensionMethods.StringExtensions;

namespace ToracLibrary.Caching
{

    //Documentation

    //first you must enable the broker for sql cache dependency on the database level
    //ALTER DATABASE DatabaseName SET NEW_BROKER WITH ROLLBACK IMMEDIATE;
    //ALTER DATABASE DatabaseName SET ENABLE_BROKER;

    //    --ALTER DATABASE [UNDERWRITING_WHSE_DAILY] SET NEW_BROKER WITH ROLLBACK AFTER 5 SECONDS;
    //--ALTER DATABASE [UNDERWRITING_WHSE_DAILY] SET ENABLE_BROKER WITH ROLLBACK AFTER 5 SECONDS;

    //--check it's enabled
    //SELECT database_id, name, is_broker_enabled FROM sys.databases WHERE name = 'MyDatabaseName'

    //add a record..when that record get's updated, then we will go refresh the cache
    //UPDATE Ref_PrismSqlDependency SET LastUpdatedDate = GETDATE();

    //after you add the sql dependency in sqlserverDp
    //select * from sys.dm_qn_subscriptions
    //will show you the subscription. First time the event is ran it will clear the table.
    //the event will re-add the subscription and a new record should be in that table too

    //*****Note in your sql you must specify database schema. ie: select lastupdatedate from dbo.tablename. 
    //if you just use select lastupdatedatefrom tablename it will never raise the event after the initial call is ran

    //Example
    //public static class TestCacheHelper
    //{
    //    public static List<string> Get()
    //    {
    //        return new TestCache<List<string>>().Get();
    //    }
    //}

    //public class TestCache<T> : SqlDependencyCache<T>
    //{

    //    private const string ConnectionString = @"data source=hpdesktop\sqlexpress;initial catalog=ToracGolf;integrated security=True;Trusted_Connection=True;";

    //    public TestCache()
    //        : base(ConnectionString)
    //    {
    //        CacheKey = "TestCache";
    //        DatabaseSchema = "dbo";
    //        SqlToRun = "SELECT UpdatedDate FROM dbo.TestSql";
    //    }

    //    protected override string SqlToRun { get; set; }

    //    protected override string DatabaseSchema { get; set; }

    //    protected override string CacheKey { get; set; }

    //    protected override T GetDataFromDataSource()
    //    {
    //        var l = new List<string>();
    //        l.Add("1");
    //        l.Add("2");

    //        return GenericsConversion.ConvertValue<T>(l);
    //    }

    //    public T Get()
    //    {
    //        return GetCacheItem();
    //    }
    //}

    /// <summary>
    /// Builds a sql cache dependency. If the data in the sql table changes, an event will be raised which will reload the cache
    /// </summary>
    /// <remarks>For More Directions And Information See Cache Base. Properties in class are immutable</remarks>
    public class SqlCacheDependency<T> : InMemoryCache<T>, ICacheImplementation<T>
    {

        #region Constructor

        /// <summary>
        /// Constructor Where The Absolute Expiration Date Is Not Set
        /// </summary>
        /// <param name="CacheKey">The Cache Key To Use For This Cache Item</param>
        /// <param name="ConnectionStringToSet">Connection String</param>
        /// <param name="DatabaseSchemaToSet">Database Schema Of Db And Query Tables</param>
        /// <param name="SqlToRunToSet">Sql to run to raise the cache event to reload</param>
        public SqlCacheDependency(string CacheKey, Func<T> GetFromDataSource, string ConnectionStringToSet, string DatabaseSchemaToSet, string SqlToRunToSet)
            : this(CacheKey, GetFromDataSource, ConnectionStringToSet, DatabaseSchemaToSet, SqlToRunToSet, null)
        {
        }

        /// <summary>
        /// Constructor Where The Absolute Expiration Date Is Set Based On The thisAbsoluteExpirationSpan Parameter
        /// </summary>
        /// <param name="CacheKey">The Cache Key To Use For This Cache Item</param>
        /// <param name="ConnectionStringToSet">Connection String</param>
        /// <param name="DatabaseSchemaToSet">Database Schema Of Db And Query Tables</param>
        /// <param name="SqlToRunToSet">Sql to run to raise the cache event to reload</param>
        /// <param name="AbsoluteExpirationSpan">Holds the max amount of time the item in the cache is valid for (AbsoluteExpiration). It gets calculated from the time its put in the cache plus the timespan</param>
        public SqlCacheDependency(string CacheKey, Func<T> GetFromDataSource, string ConnectionStringToSet, string DatabaseSchemaToSet, string SqlToRunToSet, TimeSpan AbsoluteExpirationSpan)
            : this(CacheKey, GetFromDataSource, ConnectionStringToSet, DatabaseSchemaToSet, SqlToRunToSet, new TimeSpan?(AbsoluteExpirationSpan))
        {
        }

        /// <summary>
        /// Helper Constructor to handle all the overloads
        /// </summary>
        /// <param name="CacheKey">The Cache Key To Use For This Cache Item</param>
        /// <param name="ConnectionStringToSet">Connection String</param>
        /// <param name="DatabaseSchemaToSet">Database Schema Of Db And Query Tables</param>
        /// <param name="SqlToRunToSet">Sql to run to raise the cache event to reload</param>
        /// <param name="AbsoluteExpirationSpan">Holds the max amount of time the item in the cache is valid for (AbsoluteExpiration). It gets calculated from the time its put in the cache plus the timespan</param>
        private SqlCacheDependency(string CacheKey, Func<T> GetFromDataSource, string ConnectionStringToSet, string DatabaseSchemaToSet, string SqlToRunToSet, TimeSpan? AbsoluteExpirationSpan)
            : base(CacheKey, GetFromDataSource, AbsoluteExpirationSpan)

        {
            //make sure it passes validation, it will raise an error if it fails
            PassesValidation(ConnectionStringToSet, SqlToRunToSet, DatabaseSchemaToSet);

            //set the connection string property
            ConnectionString = ConnectionStringToSet;

            //set the db schema
            DatabaseSchema = DatabaseSchemaToSet;

            //set the sql to run
            SqlToRun = SqlToRunToSet;

            //go start the sql depend
            Start(ConnectionString);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Example. dbo. You must set dbo.TableName in your sql. So this will validate that it's there. Otherwise the event won't get raised
        /// </summary>
        public string DatabaseSchema { get; }

        /// <summary>
        /// Connection String Which Connect To The Database
        /// </summary>
        public string ConnectionString { get; }

        /// <summary>
        /// Sql To Run To Check For The Updated Data
        /// </summary>
        /// <remarks>Table name must include schema. ie dbo.TableName. Can't use Select *. Use a specific query select Field1, Field2, etc.</remarks>
        public string SqlToRun { get; }

        #endregion

        #region Abstract Override Methods

        /// <summary>
        /// Add the item to the cache.  SqlCacheDependency invalidates the cache when the dependency changes vsSqlDependency, which notifies us that it changed, and we can then do what we want
        /// </summary>
        /// <param name="ItemToAddToCache">Item To Add To The Cache</param>
        public override void AddItemToCache(T ItemToAddToCache)
        {
            //validate all the parameters now (will raise an error if it fails)
            PassesValidation(ConnectionString, SqlToRun, DatabaseSchema);

            //go create the sql dependency using the sql data provider
            using (var DP = new SQLDataProvider(ConnectionString))
            {
                DP.BuildSqlDependency(SqlToRun, DependencyOnChange);
            }

            //add the item to the cache
            base.AddItemToCache(ItemToAddToCache);
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Starts The Listener On The Database
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns>Result Of Call</returns>
        /// <remarks>Usually put in global.asax - application start</remarks>
        private static bool Start(string ConnectionString)
        {
            return SqlDependency.Start(ConnectionString);
        }

        /// <summary>
        /// Stops The Listener On The Database
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns>Result Of Call</returns>
        /// <remarks>Usually put in global.asax - application start</remarks>
        private static bool Stop(string ConnectionString)
        {
            return SqlDependency.Stop(ConnectionString);
        }

        /// <summary>
        /// Run's validation and raises any error. Need to validate that dbo.TableName is set and not just table name
        /// </summary>
        /// <param name="ConnectionStringToTest">Connection String To Validate</param>
        /// <param name="DatabaseSchemaToTest">Database Schema. Usually dbo</param>
        /// <param name="SqlQueryToTest">Query Query</param>
        /// <returns>Passes Validation</returns>
        private static bool PassesValidation(string ConnectionStringToTest, string SqlQueryToTest, string DatabaseSchemaToTest)
        {
            if (string.IsNullOrEmpty(ConnectionStringToTest))
            {
                throw new ArgumentNullException("ConnectionString", "Property Connection String Is Not Set.");
            }

            if (string.IsNullOrEmpty(DatabaseSchemaToTest))
            {
                throw new ArgumentNullException("DatabaseSchema", "Property DatabaseSchema Is Not Set. Default Is dbo In Sql Server");
            }

            if (string.IsNullOrEmpty(SqlQueryToTest))
            {
                throw new ArgumentNullException("SqlToRun", "Must Pass In A Database Schema. Default Is dbo In Sql Server");
            }

            if (DatabaseSchemaToTest.Contains("."))
            {
                throw new IndexOutOfRangeException("Database Can't Contain '.' Just Enter The Schema Text");
            }

            //does it use the dbo.TableName (I guess you could use something other than dbo.)
            if (!SqlQueryToTest.Contains(DatabaseSchemaToTest, StringComparison.OrdinalIgnoreCase))
            {
                throw new IndexOutOfRangeException("Query Table Name Must Include Schema. Example: SELECT LastUpdatedDate FROM dbo.TableName");
            }

            //passed validation
            return true;
        }

        #endregion

        #region Events

        /// <summary>
        /// When the dependency command signals a change, SQL Server fires the OnChange event and is captured here. This handler will invalidate and reload the cache item using the RefreshCacheItem base method.
        /// </summary>
        private void DependencyOnChange(object sender, SqlNotificationEventArgs e)
        {
            //go kill the cache
            RemoveCacheItem();

            //now go refresh the cache 
            //note *** Sql Dependency is only good for 1 notification so we need to setup the next go around
            //By calling RefreshCacheItem this will also setup another Sql Dependency to handle the next refresh
            RefreshCacheItem();
        }

        #endregion

    }

}
