using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using ToracLibrary.Core.DataTableHelpers;

namespace ToracLibraryTest.UnitsTest.Core
{

    /// <summary>
    /// Unit test for data table functionality
    /// </summary>
    [TestClass]
    public class DataTableTest
    {

        #region To Data Table Test

        #region Framework

        private class DataTableTestClass
        {

            #region Properties

            public int Id { get; set; }
            public string Txt { get; set; }
            public IEnumerable<DataTableTestClass> TestCollection { get; set; }

            #endregion

            #region Static Methods

            public static IEnumerable<DataTableTestClass> BuildListOfTLazy()
            {
                //Go build the items to add to the table
                yield return new DataTableTestClass { Id = 1, Txt = "1", TestCollection = new List<DataTableTestClass>() { new DataTableTestClass() } };
                yield return new DataTableTestClass { Id = 2, Txt = "2", TestCollection = new List<DataTableTestClass>() { new DataTableTestClass() } };
                yield return new DataTableTestClass { Id = 3, Txt = "3", TestCollection = new List<DataTableTestClass>() { new DataTableTestClass() } };
            }

            #endregion

        }

        #endregion

        #region Test

        /// <summary>
        /// test the conversion from IEnumerable of T to a data table. This tests the single object method
        /// </summary>
        [TestCategory("Core.DataTableHelpers")]
        [TestCategory("Core")]
        [TestMethod]
        public void ToDataTableTest1()
        {
            //go build 1 single item
            var SingleObject = new DataTableTestClass
            {
                Id = 1,
                Txt = "1",
                TestCollection = new List<DataTableTestClass> { new DataTableTestClass() }
            };

            //give it a name
            const string TableNameToUse = "TestTableName";

            //go grab the result of the method
            var DataTableResult = ToDataTables.BuildDataTableFromObject(SingleObject, TableNameToUse);

            //check the table name
            Assert.AreEqual(TableNameToUse, DataTableResult.TableName);

            //check the count of how many rows we have
            Assert.AreEqual(1, DataTableResult.Rows.Count);

            //check the column count 
            Assert.AreEqual(2, DataTableResult.Columns.Count);

            //check the id field value
            Assert.AreEqual(1, DataTableResult.Rows[0]["Id"]);

            //check the txt field value
            Assert.AreEqual("1", DataTableResult.Rows[0]["Txt"]);
        }

        /// <summary>
        /// test the conversion from IEnumerable of T to a data table. This tests when you pass in a list of T
        /// </summary>
        [TestCategory("Core.DataTableHelpers")]
        [TestCategory("Core")]
        [TestMethod]
        public void ToDataTableTest2()
        {
            //go get the list of T to add to the data table
            var RowsToTest = DataTableTestClass.BuildListOfTLazy().ToArray();

            //give it a name
            const string TableNameToUse = "TestTableName";

            //go grab the result of the method
            var DataTableResult = ToDataTables.BuildDataTableFromListOfObjects(RowsToTest, TableNameToUse);

            //check the table name
            Assert.AreEqual(TableNameToUse, DataTableResult.TableName);

            //check how many rows we have
            Assert.AreEqual(RowsToTest.Count(), DataTableResult.Rows.Count);

            //check the column count
            Assert.AreEqual(2, DataTableResult.Columns.Count);

            //check row 1
            Assert.AreEqual(RowsToTest.First().Id, DataTableResult.Rows[0]["Id"]);
            Assert.AreEqual(RowsToTest.First().Txt, DataTableResult.Rows[0]["Txt"]);

            //check row 2
            Assert.AreEqual(RowsToTest[1].Id, DataTableResult.Rows[1]["Id"]);
            Assert.AreEqual(RowsToTest[1].Txt, DataTableResult.Rows[1]["Txt"]);

            //check row 3
            Assert.AreEqual(RowsToTest[2].Id, DataTableResult.Rows[2]["Id"]);
            Assert.AreEqual(RowsToTest[2].Txt, DataTableResult.Rows[2]["Txt"]);
        }

        /// <summary>
        /// make sure if you pass a list in to the single method name, it will raise an derror
        /// </summary>

        [TestCategory("Core.DataTableHelpers")]
        [TestCategory("Core")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ToDataTableTest3()
        {
            //run the single T method with a list, it should raise an error
            ToDataTables.BuildDataTableFromObject(DataTableTestClass.BuildListOfTLazy().ToArray(), "TestTableName");
        }

        #endregion

        #endregion

    }

}