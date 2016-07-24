using System;
using System.Collections.Generic;
using System.Linq;
using ToracLibrary.Core.Delimiter;
using Xunit;

namespace ToracLibrary.UnitTest.Core
{

    /// <summary>
    /// Unit test to create and parse delimiter files
    /// </summary>
    public class DelimiterTest
    {

        #region Delimiter Creator Framework

        #region Private Constants

        /// <summary>
        /// CSV Delimiter
        /// </summary>
        private const string CSVDelimiter = ",";

        /// <summary>
        /// Hold to split the values
        /// </summary>
        private static readonly string[] SplitForTest = { Environment.NewLine, CSVDelimiter.ToString() };

        #endregion

        #region Private Helpers

        /// <summary>
        /// Split the output text
        /// </summary>
        /// <param name="OutputValue">Value to output</param>
        /// <returns>column data</returns>
        private static IEnumerable<DelimiterRow> ParseResultsLazy(string OutputValue)
        {
            return DelimiterParser.ParseFromTextLazy(OutputValue, CSVDelimiter);
        }

        #endregion

        #endregion

        #region Without Headers With A Single Line

        [Fact]
        public void CSVWithoutHeadersTest1()
        {
            //create the delimiter builder
            var DelimiterBuilder = new DelimiterCreator(CSVDelimiter);

            //add a row of data
            DelimiterBuilder.AddRow(new string[] { "1", "2", "", null });

            //what is the final output of creator
            string Result = DelimiterBuilder.WriteData();

            //split the text result into columns
            var SplitTextResult = ParseResultsLazy(Result).ToArray();

            //check the results
            Assert.Equal("1", SplitTextResult[0].ColumnData.ElementAt(0));
            Assert.Equal("2", SplitTextResult[0].ColumnData.ElementAt(1));
            Assert.Equal("", SplitTextResult[0].ColumnData.ElementAt(2));
            Assert.Equal("", SplitTextResult[0].ColumnData.ElementAt(3));
        }

        [Fact]
        public void CSVWithoutHeadersTest2()
        {
            //create the delimiter builder
            var DelimiterBuilder = new DelimiterCreator(CSVDelimiter);

            //add a row
            DelimiterBuilder.AddRow(new string[] { "1", "2", "3", "4" });

            //what is the final output of creator
            string Result = DelimiterBuilder.WriteData();

            //split the text result into columns
            var SplitTextResult = ParseResultsLazy(Result).ToArray();

            //check the results
            Assert.Equal("1", SplitTextResult[0].ColumnData.ElementAt(0));
            Assert.Equal("2", SplitTextResult[0].ColumnData.ElementAt(1));
            Assert.Equal("3", SplitTextResult[0].ColumnData.ElementAt(2));
            Assert.Equal("4", SplitTextResult[0].ColumnData.ElementAt(3));
        }

        #endregion

        #region Without Headers With A Single Line (Bulk Load)

        [Fact]
        public void CSVWithoutHeadersBulkLoadTest1()
        {
            //create the delimiter builder
            var DelimiterBuilder = new DelimiterCreator(CSVDelimiter);

            //create list of rows
            var RowsToAdd = new List<DelimiterRow>();

            //add a row
            RowsToAdd.Add(new DelimiterRow(new string[] { "1", "2", "", null }));

            //add the list (range of rows)
            DelimiterBuilder.AddRowRange(RowsToAdd);

            //what is the final output of creator
            string Result = DelimiterBuilder.WriteData();

            //split the text result into columns
            var SplitTextResult = ParseResultsLazy(Result).ToArray();

            //check the results
            Assert.Equal("1", SplitTextResult[0].ColumnData.ElementAt(0));
            Assert.Equal("2", SplitTextResult[0].ColumnData.ElementAt(1));
            Assert.Equal(string.Empty, SplitTextResult[0].ColumnData.ElementAt(2));
            Assert.Equal(string.Empty, SplitTextResult[0].ColumnData.ElementAt(3));
        }

        [Fact]
        public void CSVWithoutHeadersBulkLoadTest2()
        {
            //create the delimiter builder
            var DelimiterBuilder = new DelimiterCreator(CSVDelimiter);

            //create list of rows
            var RowsToAdd = new List<DelimiterRow>();

            //add a row
            RowsToAdd.Add(new DelimiterRow(new string[] { "1", "2", "3", "4" }));

            //add the list (range of rows)
            DelimiterBuilder.AddRowRange(RowsToAdd);

            //what is the final output of creator
            string Result = DelimiterBuilder.WriteData();

            //split the text result into columns
            var SplitTextResult = ParseResultsLazy(Result).ToArray();

            //check the results
            Assert.Equal("1", SplitTextResult[0].ColumnData.ElementAt(0));
            Assert.Equal("2", SplitTextResult[0].ColumnData.ElementAt(1));
            Assert.Equal("3", SplitTextResult[0].ColumnData.ElementAt(2));
            Assert.Equal("4", SplitTextResult[0].ColumnData.ElementAt(3));
        }

        #endregion

        #region Without Headers With Multiple Rows

        [Fact]
        public void CSVWithoutHeadersMultiRowTest1()
        {
            //create the delimiter builder
            var DelimiterBuilder = new DelimiterCreator(CSVDelimiter);

            //add 2 rows to the builder
            DelimiterBuilder.AddRow(new string[] { "1", "2", "3", "4" });
            DelimiterBuilder.AddRow(new string[] { string.Empty, "6", null, "8" });

            //get the result
            string Result = DelimiterBuilder.WriteData();

            //split the text result into columns
            var SplitTextResult = ParseResultsLazy(Result).ToArray();

            //check the result now
            Assert.Equal("1", SplitTextResult[0].ColumnData.ElementAt(0));
            Assert.Equal("2", SplitTextResult[0].ColumnData.ElementAt(1));
            Assert.Equal("3", SplitTextResult[0].ColumnData.ElementAt(2));
            Assert.Equal("4", SplitTextResult[0].ColumnData.ElementAt(3));

            Assert.Equal(string.Empty, SplitTextResult[1].ColumnData.ElementAt(0));
            Assert.Equal("6", SplitTextResult[1].ColumnData.ElementAt(1));
            Assert.Equal(string.Empty, SplitTextResult[1].ColumnData.ElementAt(2));
            Assert.Equal("8", SplitTextResult[1].ColumnData.ElementAt(3));
        }

        #endregion

        #region Without Headers With Multiple Rows (Bulk Load)

        /// <summary>
        /// Test the delimiter using a csv format
        /// </summary>
        [Fact]
        public void CSVWithoutHeadersMultiRowBulkLoadTest1()
        {
            //create the delimiter builder
            var DelimiterBuilder = new DelimiterCreator(CSVDelimiter);

            //create list of rows
            var RowsToAdd = new List<DelimiterRow>();

            //add 2 rows
            RowsToAdd.Add(new DelimiterRow(new string[] { "1", "2", "3", "4" }));
            RowsToAdd.Add(new DelimiterRow(new string[] { string.Empty, "6", null, "8" }));

            //add those rows to the builder
            DelimiterBuilder.AddRowRange(RowsToAdd);

            //grab the resdult
            string Result = DelimiterBuilder.WriteData();

            //split the text result into columns
            var SplitTextResult = ParseResultsLazy(Result).ToArray();

            //check the result now
            Assert.Equal("1", SplitTextResult[0].ColumnData.ElementAt(0));
            Assert.Equal("2", SplitTextResult[0].ColumnData.ElementAt(1));
            Assert.Equal("3", SplitTextResult[0].ColumnData.ElementAt(2));
            Assert.Equal("4", SplitTextResult[0].ColumnData.ElementAt(3));

            Assert.Equal(string.Empty, SplitTextResult[1].ColumnData.ElementAt(0));
            Assert.Equal("6", SplitTextResult[1].ColumnData.ElementAt(1));
            Assert.Equal(string.Empty, SplitTextResult[1].ColumnData.ElementAt(2));
            Assert.Equal("8", SplitTextResult[1].ColumnData.ElementAt(3));
        }

        #endregion

        #region With Headers With A Single Line

        /// <summary>
        /// Test the delimiter using a csv format
        /// </summary>
        [Fact]
        public void CSVWithHeadersTest1()
        {
            //create the delimiter builder
            var DelimiterBuilder = new DelimiterCreator(new string[] { "column1", "column2", "column3", "column4" }, CSVDelimiter);

            //add a row
            DelimiterBuilder.AddRow(new string[] { "1", "2", "3", "4" });

            //grab the resdult
            string Result = DelimiterBuilder.WriteData();

            //split the text result into columns
            var SplitTextResult = ParseResultsLazy(Result).ToArray();

            //check the results
            Assert.Equal("column1", SplitTextResult[0].ColumnData.ElementAt(0));
            Assert.Equal("column2", SplitTextResult[0].ColumnData.ElementAt(1));
            Assert.Equal("column3", SplitTextResult[0].ColumnData.ElementAt(2));
            Assert.Equal("column4", SplitTextResult[0].ColumnData.ElementAt(3));

            Assert.Equal("1", SplitTextResult[1].ColumnData.ElementAt(0));
            Assert.Equal("2", SplitTextResult[1].ColumnData.ElementAt(1));
            Assert.Equal("3", SplitTextResult[1].ColumnData.ElementAt(2));
            Assert.Equal("4", SplitTextResult[1].ColumnData.ElementAt(3));
        }

        /// <summary>
        /// Test the delimiter using a csv format
        /// </summary>
        [Fact]
        public void CSVWithHeadersTest2()
        {
            //create the delimiter builder
            var DelimiterBuilder = new DelimiterCreator(new string[] { "column1", "column2", "column3", "column4" }, CSVDelimiter);

            //add a row
            DelimiterBuilder.AddRow(new string[] { "1", "2", null, string.Empty });

            //grab the resdult
            string Result = DelimiterBuilder.WriteData();

            //split the text result into columns
            var SplitTextResult = ParseResultsLazy(Result).ToArray();

            //check the results
            Assert.Equal("column1", SplitTextResult[0].ColumnData.ElementAt(0));
            Assert.Equal("column2", SplitTextResult[0].ColumnData.ElementAt(1));
            Assert.Equal("column3", SplitTextResult[0].ColumnData.ElementAt(2));
            Assert.Equal("column4", SplitTextResult[0].ColumnData.ElementAt(3));

            Assert.Equal("1", SplitTextResult[1].ColumnData.ElementAt(0));
            Assert.Equal("2", SplitTextResult[1].ColumnData.ElementAt(1));
            Assert.Equal("", SplitTextResult[1].ColumnData.ElementAt(2));
            Assert.Equal("", SplitTextResult[1].ColumnData.ElementAt(3));
        }

        #endregion

        #region With Headers With Muliple Line

        /// <summary>
        /// Test the delimiter using a csv format
        /// </summary>
        [Fact]
        public void CSVWithHeadersMultiRowTest1()
        {
            //create the delimiter builder
            var DelimiterBuilder = new DelimiterCreator(new string[] { "column1", "column2", "column3", "column4" }, CSVDelimiter);

            //add 2 rows
            DelimiterBuilder.AddRow(new string[] { "1", "2", "3", "4" });
            DelimiterBuilder.AddRow(new string[] { string.Empty, "6", null, "8" });

            //grab the resdult
            string Result = DelimiterBuilder.WriteData();

            //split the text result into columns
            var SplitTextResult = ParseResultsLazy(Result).ToArray();

            //check the results
            Assert.Equal("column1", SplitTextResult[0].ColumnData.ElementAt(0));
            Assert.Equal("column2", SplitTextResult[0].ColumnData.ElementAt(1));
            Assert.Equal("column3", SplitTextResult[0].ColumnData.ElementAt(2));
            Assert.Equal("column4", SplitTextResult[0].ColumnData.ElementAt(3));

            Assert.Equal("1", SplitTextResult[1].ColumnData.ElementAt(0));
            Assert.Equal("2", SplitTextResult[1].ColumnData.ElementAt(1));
            Assert.Equal("3", SplitTextResult[1].ColumnData.ElementAt(2));
            Assert.Equal("4", SplitTextResult[1].ColumnData.ElementAt(3));

            Assert.Equal(string.Empty, SplitTextResult[2].ColumnData.ElementAt(0));
            Assert.Equal("6", SplitTextResult[2].ColumnData.ElementAt(1));
            Assert.Equal(string.Empty, SplitTextResult[2].ColumnData.ElementAt(2));
            Assert.Equal("8", SplitTextResult[2].ColumnData.ElementAt(3));
        }

        #endregion

        #region With Headers With Muliple Line (Bulk Load)

        /// <summary>
        /// Test the delimiter using a csv format
        /// </summary>
        [Fact]
        public void CSVWithHeadersMultiRowBulkLoadTest1()
        {
            //create the delimiter builder
            var DelimiterBuilder = new DelimiterCreator(new string[] { "column1", "column2", "column3", "column4" }, CSVDelimiter);

            //create list of rows
            var RowsToAdd = new List<DelimiterRow>();

            //RowsToAdd 2 rows to the list
            RowsToAdd.Add(new DelimiterRow(new string[] { "1", "2", "3", "4" }));
            RowsToAdd.Add(new DelimiterRow(new string[] { string.Empty, "6", null, "8" }));

            //push those rows now
            DelimiterBuilder.AddRowRange(RowsToAdd);

            //grab the resdult
            string Result = DelimiterBuilder.WriteData();

            //split the text result into columns
            var SplitTextResult = ParseResultsLazy(Result).ToArray();

            //check the results
            Assert.Equal("column1", SplitTextResult[0].ColumnData.ElementAt(0));
            Assert.Equal("column2", SplitTextResult[0].ColumnData.ElementAt(1));
            Assert.Equal("column3", SplitTextResult[0].ColumnData.ElementAt(2));
            Assert.Equal("column4", SplitTextResult[0].ColumnData.ElementAt(3));

            Assert.Equal("1", SplitTextResult[1].ColumnData.ElementAt(0));
            Assert.Equal("2", SplitTextResult[1].ColumnData.ElementAt(1));
            Assert.Equal("3", SplitTextResult[1].ColumnData.ElementAt(2));
            Assert.Equal("4", SplitTextResult[1].ColumnData.ElementAt(3));

            Assert.Equal(string.Empty, SplitTextResult[2].ColumnData.ElementAt(0));
            Assert.Equal("6", SplitTextResult[2].ColumnData.ElementAt(1));
            Assert.Equal(string.Empty, SplitTextResult[2].ColumnData.ElementAt(2));
            Assert.Equal("8", SplitTextResult[2].ColumnData.ElementAt(3));
        }

        #endregion

    }

}