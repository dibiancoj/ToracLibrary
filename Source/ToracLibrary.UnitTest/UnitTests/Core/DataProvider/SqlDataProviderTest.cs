using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ToracLibrary.Core.DataProviders.ADO;
using ToracLibrary.UnitTest.EntityFramework.DataContext;
using ToracLibrary.UnitTest.Framework;
using Xunit;

namespace ToracLibrary.UnitTest.Core.DataProviders
{

    /// <summary>
    /// Unit test to test the sql data provider
    /// </summary>
    [Collection("DatabaseUnitTests")]
    public class SqlDataProviderTest
    {

        #region IDependency Injection Methods

        /// <summary>
        /// Get the connection string to use
        /// </summary>
        /// <returns>Connection string</returns>
        internal static string ConnectionStringToUse()
        {
            //grab the connection string from the ef model
            using (var EFDataContext = new EntityFrameworkEntityDP())
            {
                //set the connection string
                return EFDataContext.Database.Connection.ConnectionString;
            }
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Can we connect to the database
        /// </summary>
        [Fact(Skip = DisableSpecificUnitTestAreas.DatabaseAvailableForUnitTestFlag)]
        public void CanConnect()
        {
            //go create the sql data provider
            using (var DP = DIUnitTestContainer.DIContainer.Resolve<IDataProvider>())
            {
                //make sure we can connect
                Assert.True(DP.CanConnectToDatabase());
            }
        }

        #endregion

        #region Data Sets

        /// <summary>
        /// Test using a dataset with sql parameters
        /// </summary>
        [Fact(Skip = DisableSpecificUnitTestAreas.DatabaseAvailableForUnitTestFlag)]
        public void DataSetWithTextAndParameters()
        {
            //tear down and build up
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            //create the data provider
            using (var DP = DIUnitTestContainer.DIContainer.Resolve<IDataProvider>())
            {
                //id to query
                const int IdToQuery = 1;

                //go grab the data set
                var DataSetToTest = DP.GetDataSet("SELECT * FROM Ref_Test AS T WHERE T.Id = @Id", CommandType.Text, new SqlParameter[] { new SqlParameter("@Id", IdToQuery) });

                //make sure we have 1 table
                Assert.Equal(1, DataSetToTest.Tables.Count);

                //check the row count now
                Assert.Equal(1, DataSetToTest.Tables[0].Rows.Count);

                //make sure the id is 1
                Assert.Equal(IdToQuery, Convert.ToInt32(DataSetToTest.Tables[0].Rows[0]["Id"]));
            }
        }

        /// <summary>
        /// Let's test the data set with sql text
        /// </summary>
        [Fact(Skip = DisableSpecificUnitTestAreas.DatabaseAvailableForUnitTestFlag)]
        public void DataSetWithText()
        {
            //tear down and build up
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            //create the data provider
            using (var DP = DIUnitTestContainer.DIContainer.Resolve<IDataProvider>())
            {
                //go grab the data set
                var DataSetToTest = DP.GetDataSet("SELECT * FROM Ref_Test", CommandType.Text);

                //make sure we have 1 table
                Assert.Equal(1, DataSetToTest.Tables.Count);

                //check the row count now
                Assert.Equal(DataProviderSetupTearDown.DefaultRecordsToInsert, DataSetToTest.Tables[0].Rows.Count);
            }
        }

        #endregion

        #region Data Tables

        /// <summary>
        /// Test using a datatable with sql parameters
        /// </summary>
        [Fact(Skip = DisableSpecificUnitTestAreas.DatabaseAvailableForUnitTestFlag)]
        public void DataTableWithTextAndParameters()
        {
            //tear down and build up
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            //create the data provider
            using (var DP = DIUnitTestContainer.DIContainer.Resolve<IDataProvider>())
            {
                //id to query
                const int IdToQuery = 1;

                //go grab the data table
                var DataTableToTest = DP.GetDataTable("SELECT * FROM Ref_Test AS T WHERE T.Id = @Id", CommandType.Text, new SqlParameter[] { new SqlParameter("@Id", IdToQuery) });

                //now lets check the results
                Assert.Equal(1, DataTableToTest.Rows.Count);

                //make sure the id is 1
                Assert.Equal(IdToQuery, Convert.ToInt32(DataTableToTest.Rows[0]["Id"]));
            }
        }

        /// <summary>
        /// Let's test the data table with sql text
        /// </summary>
        [Fact(Skip = DisableSpecificUnitTestAreas.DatabaseAvailableForUnitTestFlag)]
        public void DataTableWithText()
        {
            //tear down and build up
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            //create the data provider
            using (var DP = DIUnitTestContainer.DIContainer.Resolve<IDataProvider>())
            {
                //go grab the data table
                var DataTableToTest = DP.GetDataTable("SELECT * FROM Ref_Test", CommandType.Text);

                //now lets check the results
                Assert.Equal(DataProviderSetupTearDown.DefaultRecordsToInsert, DataTableToTest.Rows.Count);
            }
        }

        #endregion

        #region Data Readers

        /// <summary>
        /// Let's test the data reader with sql text with sql parameters
        /// </summary>
        [Fact(Skip = DisableSpecificUnitTestAreas.DatabaseAvailableForUnitTestFlag)]
        public void DataReaderWithTextAndParameters()
        {
            //tear down and build up
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            //create the data provider
            using (var DP = DIUnitTestContainer.DIContainer.Resolve<IDataProvider>())
            {
                //id to query
                const int IdToQuery = 1;

                //grab the data reader
                var DataReaderToTest = DP.GetDataReader("SELECT * FROM Ref_Test AS T WHERE T.Id = @Id", CommandType.Text, CommandBehavior.CloseConnection, new SqlParameter[] { new SqlParameter("@Id", IdToQuery) });

                //tally on how many records we have
                int RecordCount = 0;

                //make sure we have rows
                Assert.True(DataReaderToTest.HasRows);

                //loop through the rows
                while (DataReaderToTest.Read())
                {
                    //increase the record tally
                    RecordCount++;

                    //make sure the id is 1 (there should only be 1 row
                    Assert.Equal(IdToQuery, Convert.ToInt32(DataReaderToTest["Id"]));
                }

                //let's check how many rows we should have now
                Assert.Equal(1, RecordCount);
            }
        }

        /// <summary>
        /// Let's test the data reader with sql text
        /// </summary>
        [Fact(Skip = DisableSpecificUnitTestAreas.DatabaseAvailableForUnitTestFlag)]
        public void DataReaderWithText()
        {
            //tear down and build up
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            //create the data provider
            using (var DP = DIUnitTestContainer.DIContainer.Resolve<IDataProvider>())
            {
                //grab the data reader
                var DataReaderToTest = DP.GetDataReader("SELECT * FROM Ref_Test", CommandType.Text, CommandBehavior.CloseConnection);

                //tally on how many records we have
                int RecordCount = 0;

                //make sure we have rows
                Assert.True(DataReaderToTest.HasRows);

                //loop through the rows
                while (DataReaderToTest.Read())
                {
                    //increase the record tally
                    RecordCount++;
                }

                //let's check how many rows we should have now
                Assert.Equal(DataProviderSetupTearDown.DefaultRecordsToInsert, RecordCount);
            }
        }

        #endregion

        #region Xml Element

        /// <summary>
        /// Let's test the xml data fetch
        /// </summary>
        [Fact(Skip = DisableSpecificUnitTestAreas.DatabaseAvailableForUnitTestFlag)]
        public void XElementWithText()
        {
            //tear down and build up
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            //create the data provider
            using (var DP = (SQLDataProvider)DIUnitTestContainer.DIContainer.Resolve<IDataProvider>())
            {
                //let's grab the xelement
                var XDocumentResults = DP.GetXMLData("SELECT * FROM Ref_Test FOR XML PATH, ROOT('root')", CommandType.Text);

                //let's check how many records we have
                Assert.Equal(DataProviderSetupTearDown.DefaultRecordsToInsert, XDocumentResults.Elements().Count());
            }
        }

        #endregion

        #region Get Scalar

        /// <summary>
        /// Test the get scalar with a text sql command with sql parameters
        /// </summary>
        [Fact(Skip = DisableSpecificUnitTestAreas.DatabaseAvailableForUnitTestFlag)]
        public void GetScalarWithTextAndParameters()
        {
            //tear down and build up
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            //create the data provider
            using (var DP = DIUnitTestContainer.DIContainer.Resolve<IDataProvider>())
            {
                //declare the id we want to grab
                const int IdToQuery = 1;

                //let's go build the sql for this id
                string sql = $"SELECT T.Id FROM Ref_Test AS T WHERE T.id = @Id";

                //go fetch the record using the object return overload
                Assert.Equal(IdToQuery, (int)DP.GetScalar(sql, CommandType.Text, new SqlParameter[] { new SqlParameter("@Id", IdToQuery) }));

                //use the generic method to test
                Assert.Equal(IdToQuery, DP.GetScalar<int>(sql, CommandType.Text, new SqlParameter[] { new SqlParameter("@Id", IdToQuery) }));
            }
        }

        /// <summary>
        /// Test the get scalar with a text sql command
        /// </summary>
        [Fact(Skip = DisableSpecificUnitTestAreas.DatabaseAvailableForUnitTestFlag)]
        public void GetScalarWithText()
        {
            //tear down and build up
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            //create the data provider
            using (var DP = DIUnitTestContainer.DIContainer.Resolve<IDataProvider>())
            {
                //declare the id we want to grab
                const int IdToFetch = 1;

                //let's go build the sql for this id
                string sql = $"SELECT T.Id FROM Ref_Test AS T WHERE T.id = {IdToFetch}";

                //go fetch the record using the object return overload
                Assert.Equal(IdToFetch, (int)DP.GetScalar(sql, CommandType.Text));

                //use the generic method to test
                Assert.Equal(IdToFetch, DP.GetScalar<int>(sql, CommandType.Text));
            }
        }

        #endregion

    }

}