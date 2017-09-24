using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using ToracLibrary.Core.EnumUtilities;
using ToracLibrary.ExcelEPPlus.Builder.Attributes;
using ToracLibrary.ExcelEPPlus.Builder.Configuration;
using static ToracLibrary.ExcelEPPlus.Builder.Formatters.Formatter;

namespace ToracLibrary.ExcelEPPlus.Builder
{

    /// <summary>
    /// Provides a fluent api to build a specific workbook
    /// </summary>
    /// <typeparam name="TDataRowType">Each row of the spreadsheet will be mapped to this data type</typeparam>
    public class ExcelFluentWorkSheetBuilder<TDataRowType>
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ExcelBuilderToSet">Excel builder to use</param>
        /// <param name="WorkSheetNameToSet">Contains the worksheet name to use</param>
        public ExcelFluentWorkSheetBuilder(ExcelFluentBuilder ExcelBuilderToSet, string WorkSheetNameToSet)
        {
            ExcelBuilder = ExcelBuilderToSet;
            WorkSheetName = WorkSheetNameToSet;

            ColumnConfiguration = new List<FluentColumnConfiguration<TDataRowType>>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Excel builder
        /// </summary>
        private ExcelFluentBuilder ExcelBuilder { get; }

        /// <summary>
        /// Contains the worksheet name to use
        /// </summary>
        internal string WorkSheetName { get; private set; }

        /// <summary>
        /// Holds the configuration for the headers
        /// </summary>
        internal FluentHeaderConfiguration HeaderConfiguration { get; private set; }

        /// <summary>
        /// Contains the configuration for a colummn
        /// </summary>
        internal List<FluentColumnConfiguration<TDataRowType>> ColumnConfiguration { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Add the header configuration into the builder
        /// </summary>
        /// <param name="RowIndexToWriteTo">Row index to write the header into</param>
        /// <param name="MakeBold">make the headers bold</param>
        /// <param name="AddAutoFilter">add the auto filter to the headers</param>
        /// <param name="AutoFitColumns">auto fit the columns</param>
        /// <returns>Fluent API Object</returns>
        public ExcelFluentWorkSheetBuilder<TDataRowType> AddHeader(int RowIndexToWriteTo, bool MakeBold, bool AddAutoFilter, bool AutoFitColumns)
        {
            HeaderConfiguration = new FluentHeaderConfiguration(RowIndexToWriteTo, MakeBold, AddAutoFilter, AutoFitColumns);

            return this;
        }

        /// <summary>
        /// Set the mapper which will build the value to output for the specific column
        /// </summary>
        /// <param name="ColumnIndex">Index of the column this is going to. First column is 1</param>
        /// <param name="HeaderDisplayTextToSet">Header text to display</param>
        /// <param name="DataMapperToSet">Mapper which takes the row object and returns the column value</param>
        /// <param name="FormatterToSet">Any additional formatter settings for this column. ie: date column</param>
        /// <param name="ColumnWidthToSet">A specific column width. This will overwrite auto width column setting for the individual column</param>
        /// <returns>Fluent API Object</returns>
        public ExcelFluentWorkSheetBuilder<TDataRowType> AddColumnConfiguration(int ColumnIndex, string HeaderName, Func<TDataRowType, object> Mapper, ExcelBuilderFormatters? FormatterToAdd = null, double? WidthToSet = null)
        {
            if (ColumnConfiguration.Any(x => x.ColumnIndex == ColumnIndex))
            {
                throw new ArgumentOutOfRangeException(nameof(ColumnIndex), "Column Index Of Value = " + ColumnIndex + " Has Already Been Set");
            }

            ColumnConfiguration.Add(new FluentColumnConfiguration<TDataRowType>(ColumnIndex, HeaderName, Mapper, FormatterToAdd, WidthToSet));

            return this;
        }

        #endregion

        #region Builder Methods

        /// <summary>
        /// Build the worksheet and add it to the ExcelEPPlusCreator to be saved
        /// </summary>
        /// <param name="DataSet">Dataset to build the excel sheet with</param>
        /// <returns>The main excel builder worksheet so you can add more worksheets</returns>
        public ExcelFluentBuilder Build(IEnumerable<TDataRowType> DataSet)
        {
            //make sure there is a specified worksheet name
            if (string.IsNullOrEmpty(WorkSheetName))
            {
                throw new NullReferenceException(nameof(WorkSheetName));
            }

            //make sure we have a header configuration
            if (HeaderConfiguration == null)
            {
                throw new ArgumentNullException("Please call addHeader before building");
            }

            if (HeaderConfiguration.RowIndexToWriteInto < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(HeaderConfiguration.RowIndexToWriteInto), "Header must write into a cell that is greater then 0");
            }

            if (!ColumnConfiguration.Any())
            {
                throw new ArgumentOutOfRangeException(nameof(ColumnConfiguration), "No Column Configuration Have Been Generated. Please Call AddDataMapping For Each Column");
            }

            if (DataSet == null)
            {
                throw new NullReferenceException(nameof(DataSet));
            }

            //get the starting data row index
            int StartToWriteBodyAtRowIndex = HeaderConfiguration.RowIndexToWriteInto + 1;

            //add the worksheet
            ExcelBuilder.ExcelCreator.AddWorkSheet(WorkSheetName);

            //grab the working spreadsheet
            var WorkingSpreadSheet = ExcelBuilder.ExcelCreator.WorkSheetSelect(WorkSheetName);

            //add the headers
            WriteHeadersToWorksheet(WorkingSpreadSheet);

            //go add the data in the spreadsheet
            WriteBodyInSpreadSheet(WorkingSpreadSheet, DataSet, StartToWriteBodyAtRowIndex);

            //do we want to autofit the columns
            if (HeaderConfiguration.AutoFitTheColumns)
            {
                ExcelBuilder.ExcelCreator.AutoFitColumnsInASpreadSheet(WorkingSpreadSheet);
            }

            //any formatters we need to apply
            AddColumnFormatters(WorkingSpreadSheet, StartToWriteBodyAtRowIndex);

            //any widths we need to apply
            AddColumnWidth(WorkingSpreadSheet);

            //return the builder so the end user can add more worksheets
            return ExcelBuilder;
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Add a column formatter to a worksheet
        /// </summary>
        /// <param name="WorkSheetToWriteTo">SpreadhSheet to write into</param>
        /// <param name="BodyStartRowIndex">The row index where the body starts to write into</param>
        private void AddColumnFormatters(ExcelWorksheet WorkSheetToWriteTo, int BodyStartRowIndex)
        {
            //we are going to grab all the formatters and cache them this way we don't have to keep checking the attribute which is costly
            var FormatterLookup = EnumUtility.GetValuesLazy<ExcelBuilderFormatters>().ToDictionary(x => x, x => EnumUtility.CustomAttributeGet<ExcelFormatterAttribute>(x).FormatterToUse);

            //loop through all the configurations (will never be null)
            foreach (var ColumnToFormat in ColumnConfiguration.Where(x => x.Formatter.HasValue))
            {
                //format the cell
                WorkSheetToWriteTo.Cells[BodyStartRowIndex, ColumnToFormat.ColumnIndex, WorkSheetToWriteTo.Dimension.End.Row, ColumnToFormat.ColumnIndex].Style.Numberformat.Format = FormatterLookup[ColumnToFormat.Formatter.Value];
            }
        }

        /// <summary>
        /// Set a column width
        /// </summary>
        /// <param name="WorkSheetToWriteTo">SpreadhSheet to write into</param>
        /// <param name="ColumnIndexCacheLookup">Contains a mapping of TColumnEnum and the column index so we dont need to re-calc everything for multiple methods</param>
        private void AddColumnWidth(ExcelWorksheet WorkSheetToWriteTo)
        {
            //loop through all the configurations (will never be null)
            foreach (var ColumnToSetWidth in ColumnConfiguration.Where(x => x.ColumnWidth.HasValue))
            {
                //format the cell
                WorkSheetToWriteTo.Column(ColumnToSetWidth.ColumnIndex).Width = ColumnToSetWidth.ColumnWidth.Value;
            }
        }

        /// <summary>
        /// Write the headers into the worksheet
        /// </summary>
        /// <param name="WorkSheet">worksheet to write into</param>
        /// <param name="ColumnIndexCacheLookup">Contains a mapping of TColumnEnum and the column index so we dont need to re-calc everything for multiple methods</param>
        private void WriteHeadersToWorksheet(ExcelWorksheet WorkSheet)
        {
            //grab the min and max column indexes
            var MinColumnIndex = ColumnConfiguration.Min(x => x.ColumnIndex);
            var MaxColumnIndex = ColumnConfiguration.Max(x => x.ColumnIndex);

            //loop through the values
            foreach (var ColumnToWrite in ColumnConfiguration)
            {
                //write the header
                ExcelBuilder.ExcelCreator.WriteToCell(WorkSheet, ColumnToWrite.ColumnIndex, HeaderConfiguration.RowIndexToWriteInto, ColumnToWrite.HeaderDisplayText);
            }

            if (HeaderConfiguration.MakeBold || HeaderConfiguration.AddAutoFilter)
            {
                var HeaderRowRange = WorkSheet.Cells[HeaderConfiguration.RowIndexToWriteInto, MinColumnIndex, HeaderConfiguration.RowIndexToWriteInto, MaxColumnIndex];

                if (HeaderConfiguration.MakeBold)
                {
                    ExcelBuilder.ExcelCreator.MakeRangeBold(HeaderRowRange);
                }

                if (HeaderConfiguration.AddAutoFilter)
                {
                    ExcelBuilder.ExcelCreator.AddAutoFilter(HeaderRowRange);
                }
            }
        }

        /// <summary>
        /// Add the data in the body of the spreadsheet
        /// </summary>
        /// <param name="WorkSheetToWriteInto">spreadsheet to write into</param>
        /// <param name="ColumnIndexCacheLookup">Contains a mapping of TColumnEnum and the column index so we dont need to re-calc everything for multiple methods</param>
        /// <param name="DataSet">DataSet to write</param>
        /// <param name="RowIndexToStartWriting">Row index to starting writing at</param>
        /// <returns>Row index that we are done writing</returns>
        private int WriteBodyInSpreadSheet(ExcelWorksheet WorkSheetToWriteInto, IEnumerable<TDataRowType> DataSet, int RowIndexToStartWriting)
        {
            //the row index we write to
            int CurrentRowIndex = RowIndexToStartWriting;

            //sorted column index - epplus is always faster when you load top to bottom and left to right. So we will sort the column by index
            var SortedColumnIndex = ColumnConfiguration.OrderBy(x => x.ColumnIndex).ToList();

            //loop through the dataset
            foreach (var RecordToWrite in DataSet)
            {
                //loop through the columns now
                foreach (var ColumnToWrite in SortedColumnIndex)
                {
                    ExcelBuilder.ExcelCreator.WriteToCell(WorkSheetToWriteInto, ColumnToWrite.ColumnIndex, CurrentRowIndex, ColumnToWrite.DataMapper(RecordToWrite));
                }

                //increase the row index
                CurrentRowIndex++;
            }

            return CurrentRowIndex;
        }

        #endregion

    }

}
