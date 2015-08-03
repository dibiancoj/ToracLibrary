using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ToracLibrary.Core.DataProviders.ADO;
using ToracLibraryTest.Framework;
using static ToracLibraryTest.UnitsTest.Core.DataProviders.DataProviderSetupTearDown;

namespace ToracLibraryTest.UnitsTest.Core.DataProviders
{

    /// <summary>
    /// Unit test to test the sql data provider
    /// </summary>
    [TestClass]
    public class SqlDataProviderTest
    {

        #region Utility Methods

        /// <summary>
        /// Can we connect to the database
        /// </summary>
        [TestMethod]
        public void CanConnect()
        {
            //go create the sql data provider
            using (var DP = DIUnitTestContainer.DIContainer.Resolve<IDataProvider>())
            {
                //make sure we can connect
                Assert.AreEqual(true, DP.CanConnectToDatabase());
            }
        }

        #endregion

        #region Data Sets

        /// <summary>
        /// Let's test the data set with sql text
        /// </summary>
        [TestMethod]
        public void DataSetWithText()
        {
            TruncateTable();
            AddRows();

            using (var DP = DIUnitTestContainer.DIContainer.Resolve<IDataProvider>())
            {
                var DataSetToTest = DP.GetDataSet("SELECT * FROM Ref_Test", CommandType.Text);

                Assert.AreEqual(1, DataSetToTest.Tables.Count);
                Assert.AreEqual(DefaultRecordsToInsert, DataSetToTest.Tables[0].Rows.Count);
            }
        }

        #endregion

        #region Data Tables

        /// <summary>
        ///  Let's test the data table with sql text
        /// </summary>
        [TestMethod]
        public void DataTableWithText()
        {
            //tear down and build up
            TearDownAndBuildUpDbEnvironment();

            //create the data provider
            using (var DP = DIUnitTestContainer.DIContainer.Resolve<IDataProvider>())
            {
                //go grab the data table
                var DataTableToTest = DP.GetDataTable("SELECT * FROM Ref_Test", CommandType.Text);

                //now lets check the results
                Assert.AreEqual(DefaultRecordsToInsert, DataTableToTest.Rows.Count);
            }
        }

        #endregion

        #region Data Readers

        /// <summary>
        /// Let's test the data reader with sql text
        /// </summary>
        [TestMethod]
        public void DataReaderWithText()
        {
            //tear down and build up
            TearDownAndBuildUpDbEnvironment();

            //create the data provider
            using (var DP = DIUnitTestContainer.DIContainer.Resolve<IDataProvider>())
            {
                //grab the data reader
                var DataReaderToTest = DP.GetDataReader("SELECT * FROM Ref_Test", CommandType.Text, CommandBehavior.CloseConnection);

                //tally on how many records we have
                int RecordCount = 0;

                //make sure we have rows
                Assert.AreEqual(true, DataReaderToTest.HasRows);

                //loop through the rows
                while (DataReaderToTest.Read())
                {
                    //increase the record tally
                    RecordCount++;
                }

                //let's check how many rows we should have now
                Assert.AreEqual(DefaultRecordsToInsert, RecordCount);
            }
        }

        #endregion

        #region Xml Element

        /// <summary>
        /// Let's test the xml data fetch
        /// </summary>
        [TestMethod]
        public void XElementWithText()
        {
            //tear down and build up
            TearDownAndBuildUpDbEnvironment();

            //create the data provider
            using (var DP = (SQLDataProvider)DIUnitTestContainer.DIContainer.Resolve<IDataProvider>())
            {
                //let's grab the xelement
                var XDocumentResults = DP.GetXMLData("SELECT * FROM Ref_Test FOR XML PATH, ROOT('root')", CommandType.Text);

                //let's check how many records we have
                Assert.AreEqual(DefaultRecordsToInsert, XDocumentResults.Elements().Count());
            }
        }

        #endregion

        #region Get Scalar

        /// <summary>
        /// Test the get scalar with a text sql command
        /// </summary>
        [TestMethod]
        public void GetScalarWithText()
        {
            //tear down and build up
            TearDownAndBuildUpDbEnvironment();

            //create the data provider
            using (var DP = DIUnitTestContainer.DIContainer.Resolve<IDataProvider>())
            {
                //declare the id we want to grab
                const int IdToFetch = 1;

                //let's go build the sql for this id
                string sql = $"SELECT T.Id FROM Ref_Test AS T WHERE T.id = " + IdToFetch;

                //go fetch the record using the object return overload
                Assert.AreEqual(IdToFetch, (int)DP.GetScalar(sql, CommandType.Text));

                //use the generic method to test
                Assert.AreEqual(IdToFetch, DP.GetScalar<int>(sql, CommandType.Text));
            }
        }

        #endregion

    }

}