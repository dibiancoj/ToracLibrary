using System;
using System.Collections.Generic;
using System.Linq;
using ToracLibrary.Core.DataTableHelpers;
using Xunit;

namespace ToracLibrary.UnitTest.Core
{

    /// <summary>
    /// Unit test for data table functionality
    /// </summary>
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
        [Fact]
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
            var DataTableResult = ToDataTable.BuildDataTableFromObject(SingleObject, TableNameToUse);

            //check the table name
            Assert.Equal(TableNameToUse, DataTableResult.TableName);

            //check the count of how many rows we have
            Assert.Equal(1, DataTableResult.Rows.Count);

            //check the column count 
            Assert.Equal(2, DataTableResult.Columns.Count);

            //check the id field value
            Assert.Equal(1, DataTableResult.Rows[0]["Id"]);

            //check the txt field value
            Assert.Equal("1", DataTableResult.Rows[0]["Txt"]);
        }

        /// <summary>
        /// test the conversion from IEnumerable of T to a data table. This tests when you pass in a list of T
        /// </summary>
        [Fact]
        public void ToDataTableTest2()
        {
            //go get the list of T to add to the data table
            var RowsToTest = DataTableTestClass.BuildListOfTLazy().ToArray();

            //give it a name
            const string TableNameToUse = "TestTableName";

            //go grab the result of the method
            var DataTableResult = ToDataTable.BuildDataTableFromListOfObjects(RowsToTest, TableNameToUse);

            //check the table name
            Assert.Equal(TableNameToUse, DataTableResult.TableName);

            //check how many rows we have
            Assert.Equal(RowsToTest.Length, DataTableResult.Rows.Count);

            //check the column count
            Assert.Equal(2, DataTableResult.Columns.Count);

            //check row 1
            Assert.Equal(RowsToTest.First().Id, DataTableResult.Rows[0]["Id"]);
            Assert.Equal(RowsToTest.First().Txt, DataTableResult.Rows[0]["Txt"]);

            //check row 2
            Assert.Equal(RowsToTest[1].Id, DataTableResult.Rows[1]["Id"]);
            Assert.Equal(RowsToTest[1].Txt, DataTableResult.Rows[1]["Txt"]);

            //check row 3
            Assert.Equal(RowsToTest[2].Id, DataTableResult.Rows[2]["Id"]);
            Assert.Equal(RowsToTest[2].Txt, DataTableResult.Rows[2]["Txt"]);
        }

        /// <summary>
        /// make sure if you pass a list in to the single method name, it will raise an derror
        /// </summary>
        [Fact]
        public void ToDataTableTest3()
        {
            //run the single T method with a list, it should raise an error
            Assert.Throws<ArgumentOutOfRangeException>(() => ToDataTable.BuildDataTableFromObject(DataTableTestClass.BuildListOfTLazy().ToArray(), "TestTableName"));

        }

        #endregion

        #endregion

    }

}