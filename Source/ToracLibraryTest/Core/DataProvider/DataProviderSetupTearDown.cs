using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.DataProviders.ADO;
using ToracLibrary.Core.DataProviders.EntityFrameworkDP;
using ToracLibraryTest.Framework;
using ToracLibraryTest.UnitsTest.Core.DataProviders.EntityFrameworkDP;
using ToracLibraryTest.UnitsTest.EntityFramework.DataContext;

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
        internal const int DefaultRecordsToInsert = 10;

        #endregion

        #region Methods

        /// <summary>
        /// Tear down and build up the envi. So cleanup up the db rows and then add the default amount of rows
        /// </summary>
        internal static void TearDownAndBuildUpDbEnvironment()
        {
            //add the default number of rows (pass in true to truncate the table)
            AddRows(true);
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
        /// <param name="TruncateTable">Truncate the table first</param>
        internal static void AddRows(bool TruncateTable)
        {
            //add the default x amount of rows (10)
            AddRows(DefaultRecordsToInsert, TruncateTable);
        }

        /// <summary>
        /// Add x amount of rows to Ref_Test
        /// </summary>
        /// <param name="HowManyRowsToAdd"></param>
        /// <param name="TruncateTableBeforeLoadingData">Truncate the table before loading rows</param>
        internal static void AddRows(int HowManyRowsToAdd, bool TruncateTableBeforeLoadingData)
        {
            //go truncate the table first (if they want too)
            if (TruncateTableBeforeLoadingData)
            {
                TruncateTable();
            }

            //create the data provider
            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(EntityFrameworkTest.WritableDataProviderName))
            {
                //add the number of rows we need
                DP.AddRange(BuildRowsToInsertLazy(HowManyRowsToAdd), false);

                //let's save now
                DP.SaveChanges();
            }
        }

        /// <summary>
        /// Build the rows with an iterator to be inserted into ef. This way we can use addrange
        /// </summary>
        /// <param name="HowManyRowsToAdd"></param>
        /// <returns>Iterator of Ref_Test</returns>
        private static IEnumerable<Ref_Test> BuildRowsToInsertLazy(int HowManyRowsToAdd)
        {
            for (var i = 0; i < HowManyRowsToAdd; i++)
            {
                //push the record to the context (don't save yet)
                yield return new Ref_Test { Description = i.ToString() };
            }
        }

        #endregion

        #endregion

    }

}
