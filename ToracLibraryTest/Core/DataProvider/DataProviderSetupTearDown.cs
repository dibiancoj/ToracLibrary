using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.DataProviders.ADO;
using ToracLibraryTest.Framework;

namespace ToracLibraryTest.UnitsTest.Core.DataProviders
{

    /// <summary>
    /// Common methods to setup the database tables. Build up or tear down
    /// </summary>
    public static class DataProviderSetupTearDown
    {

        //Sql Data Provider uses the DI container. Check DiUnitTestContainer for the build up of the SqlDataProvider

        #region Constants

        /// <summary>
        /// How many records to add by default
        /// </summary>
        public const int DefaultRecordsToInsert = 10;

        #endregion

        #region Methods

        /// <summary>
        /// Tear down and build up the envi. So cleanup up the db rows and then add the default amount of rows
        /// </summary>
        internal static void TearDownAndBuildUpDbEnvironment()
        {
            //delete all the records
            TruncateTable();

            //add the default number of rows
            AddRows();
        }

        /// <summary>
        /// Truncates the table down to 0 rows
        /// </summary>
        internal static void TruncateTable()
        {
            //grab the sql data provider
            using (var DP = DIUnitTestContainer.DIContainer.Resolve<IDataProvider>())
            {
                DP.ExecuteNonQuery("TRUNCATE TABLE Ref_Test", CommandType.Text);
            }
        }

        #region Add Rows

        /// <summary>
        /// Add the default amount of rows to Ref_Test (10)
        /// </summary>
        internal static void AddRows()
        {
            //add the default x amount of rows (10)
            AddRows(DefaultRecordsToInsert);
        }

        /// <summary>
        /// Add x amount of rows to Ref_Test
        /// </summary>
        /// <param name="HowManyRowsToAdd"></param>
        internal static void AddRows(int HowManyRowsToAdd)
        {
            //go truncate the table first
            TruncateTable();

            //create the data provider
            using (var DP = DIUnitTestContainer.DIContainer.Resolve<IDataProvider>())
            {
                //loop through however many records you want to add
                for (var i = 0; i < HowManyRowsToAdd; i++)
                {
                    //build the sql
                    string sql = string.Format($"Insert Into Ref_Test (Description,Description2) Values ('i.ToString()','i.ToString()')");

                    //insert the record now
                    DP.ExecuteNonQuery(sql, CommandType.Text);
                }
            }
        }

        #endregion

        #endregion

    }

}
