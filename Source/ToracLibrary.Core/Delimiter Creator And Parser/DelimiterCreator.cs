using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.IEnumerableExtensions;
using ToracLibrary.Core.ExtensionMethods.StringExtensions;

namespace ToracLibrary.Core.Delimiter
{

    /* Notes
   * I usually like to write methods where they don't touch class level properties. ie. WriteOutRowOfData writing to a shared string builder.
   * However, I think if you have a ton of rows, building a string builder for each call to build a row would create too much memory overhead.
   * If you have 100,000 rows, then you are creating 100,000 string builder objects. So just call the string builder from the method.
   * This is memory optimized vs better code
   */

    /// <summary>
    /// Helps You Build A CSV File
    /// </summary>
    /// <remarks>Uses Environment.NewLine For Line Breaks. Class has immutable properties</remarks>
    public class DelimiterCreator
    {

        #region Constructor

        /// <summary>
        /// Constructor when you want to add column headers
        /// </summary>
        /// <param name="ColumnHeaders">Headers To Add To The Csv. Will Be The First Row Outputted</param>
        /// <param name="DelimiterBetweenColumns">Delimiter To Use Between Columns</param>
        public DelimiterCreator(IList<string> ColumnHeaders, string DelimiterBetweenColumns)
        {
            //create the string builder
            WorkingOutputWriter = new StringBuilder();

            //set the delimiter first
            ColumnDelimiter = DelimiterBetweenColumns;

            //if we don't come from the other overload (where we pass in null). So make sure we have columns before trying to write them out
            if (ColumnHeaders.AnyWithNullCheck())
            {
                WriteOutRowOfData(new DelimiterRow(ColumnHeaders));
            }
        }

        /// <summary>
        /// Constructor when you dont want to add column headers
        /// </summary>
        /// <param name="DelimiterBetweenColumns">Delimiter To Use Between Columns</param>
        public DelimiterCreator(string DelimiterBetweenColumns)
            : this(null, DelimiterBetweenColumns)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Holds the string builder that we write to as we are building up the return value
        /// </summary>
        private StringBuilder WorkingOutputWriter { get; }

        /// <summary>
        /// Delimiter to use between columns
        /// </summary>
        private string ColumnDelimiter { get; }

        #endregion

        #region Public Methods

        #region Add A Row Of Data

        /// <summary>
        /// Add A Row To The Output Of The CSV Data
        /// </summary>
        /// <param name="ColumnDataForThisRow">Row's column data</param>
        public void AddRow(IList<string> ColumnDataForThisRow)
        {
            //Add the row's column data
            WriteOutRowOfData(new DelimiterRow(ColumnDataForThisRow));
        }

        /// <summary>
        /// Adds Multiple Rows Of Data. (Bulk Add)
        /// </summary>
        /// <param name="MultipleRowsOfColumnData">Multiple Rows Of Column Data</param>
        public void AddRowRange(IEnumerable<DelimiterRow> MultipleRowsOfColumnData)
        {
            //for each row add it to the list
            foreach (DelimiterRow RowOfDataToAdd in MultipleRowsOfColumnData)
            {
                //use the single method and add the item
                WriteOutRowOfData(RowOfDataToAdd);
            }
        }

        #endregion

        #region Write Data

        /// <summary>
        /// Gather the parsed CSV Output Data. Will Not Write Out The Column Header Names In The Data
        /// </summary>
        /// <returns>String To Be Outputted</returns>
        /// <remarks>Uses Environment.NewLine For Line Breaks</remarks>
        public string WriteData()
        {
            //just remove the last new line statement and return the stringbuilder in a string
            return WorkingOutputWriter.ToString(0, WorkingOutputWriter.Length - Environment.NewLine.Length);
        }

        #endregion

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Builds an individual row of data and returns the built up delimited value
        /// </summary>
        /// <param name="ColumnsOfDataToOutput">list of column data to output</param>
        /// <returns>Delimited String Value To Be Outputted</returns>
        private void WriteOutRowOfData(DelimiterRow ColumnsOfDataToOutput)
        {
            //we don't have any columns, just return an empty string
            if (!ColumnsOfDataToOutput.ColumnData.AnyWithNullCheck())
            {
                //have nothing, return out of the method
                return;
            }

            //loop through each of the columns and add it to the string to be returned
            foreach (string ColumnToWriteOut in ColumnsOfDataToOutput.ColumnData)
            {
                //let's just make sure we don't have a null column
                if (ColumnToWriteOut.HasValue())
                {
                    //add the column data
                    WorkingOutputWriter.Append(ColumnToWriteOut.Replace(ColumnDelimiter, string.Empty));
                }

                //add the delimiter even if its null.
                WorkingOutputWriter.Append(ColumnDelimiter);
            }

            //let's remove the last , now
            WorkingOutputWriter.Remove(WorkingOutputWriter.Length - ColumnDelimiter.Length, ColumnDelimiter.Length);

            //add a new line now (will remove when we return the string)
            WorkingOutputWriter.Append(Environment.NewLine);
        }

        #endregion

    }

}
