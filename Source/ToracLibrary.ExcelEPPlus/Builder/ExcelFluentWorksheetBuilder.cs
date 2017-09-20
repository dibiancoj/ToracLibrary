using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.EnumUtilities;
using ToracLibrary.ExcelEPPlus.Builder.Attributes;
using ToracLibrary.ExcelEPPlus.Builder.Configuration;
using static ToracLibrary.ExcelEPPlus.Builder.Formatters.Formatter;

namespace ToracLibrary.ExcelEPPlus.Builder
{

    /// <summary>
    /// Contains the fluent api to build a worksheet
    /// </summary>
    /// <typeparam name="TColumnEnum">Contains the enum type where the columns are specified. This approach means each column is an enum value</typeparam>
    /// <typeparam name="TDataRowType">Each row of the spreadsheet will be mapped to this data type</typeparam>
    public class ExcelFluentWorksheetBuilder<TColumnEnum, TDataRowType>
        where TColumnEnum : struct
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ExcelCreatorToSet">Excel Creator to use to render the excel file</param>
        public ExcelFluentWorksheetBuilder(IExcelEPPlusCreator ExcelCreatorToSet)
        {
            ExcelCreator = ExcelCreatorToSet;
            ColumnConfiguration = new Dictionary<TColumnEnum, FluentColumnConfiguration<TColumnEnum, TDataRowType>>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Contains the EP Plus wrapper that we use to actually add and save to the spreadsheet
        /// </summary>
        private IExcelEPPlusCreator ExcelCreator { get; }

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
        internal Dictionary<TColumnEnum, FluentColumnConfiguration<TColumnEnum, TDataRowType>> ColumnConfiguration { get; private set; }

        #endregion

        #region Fluent API Methods

        /// <summary>
        /// Add a worksheet to the builder
        /// </summary>
        /// <param name="WorkSheetNameToAdd">worksheet name to add</param>
        /// <returns>Fluent API Object</returns>
        public ExcelFluentWorksheetBuilder<TColumnEnum, TDataRowType> AddWorkSheet(string WorkSheetNameToAdd)
        {
            WorkSheetName = WorkSheetNameToAdd;

            return this;
        }

        /// <summary>
        /// Add the header configuration into the builder
        /// </summary>
        /// <param name="RowIndexToWriteTo">Row index to write the header into</param>
        /// <param name="MakeBold">make the headers bold</param>
        /// <param name="AddAutoFilter">add the auto filter to the headers</param>
        /// <param name="AutoFitColumns">auto fit the columns</param>
        /// <returns>Fluent API Object</returns>
        public ExcelFluentWorksheetBuilder<TColumnEnum, TDataRowType> AddHeader(int RowIndexToWriteTo, bool MakeBold, bool AddAutoFilter, bool AutoFitColumns)
        {
            HeaderConfiguration = new FluentHeaderConfiguration(RowIndexToWriteTo, MakeBold, AddAutoFilter, AutoFitColumns);

            return this;
        }

        /// <summary>
        /// Add a formatter for the specified column
        /// </summary>
        /// <param name="ColumnToSet">Column to set</param>
        /// <param name="FormatterToAdd">formatter to add to that column</param>
        /// <returns>Fluent API Object</returns>
        public ExcelFluentWorksheetBuilder<TColumnEnum, TDataRowType> AddColumnFormatter(TColumnEnum ColumnToSet, ExcelBuilderFormatters FormatterToAdd)
        {
            AddOrUpdateColumnSetting(ColumnToSet, x => x.Formatter = FormatterToAdd);

            return this;
        }

        /// <summary>
        /// Set the width of a specific column
        /// </summary>
        /// <param name="ColumnToSet">Column to set</param>
        /// <param name="WidthToSet">Width to set on that column</param>
        /// <returns>Fluent API Object</returns>
        public ExcelFluentWorksheetBuilder<TColumnEnum, TDataRowType> AddColumnWidth(TColumnEnum ColumnToSet, double WidthToSet)
        {
            AddOrUpdateColumnSetting(ColumnToSet, x => x.ColumnWidth = WidthToSet);

            return this;
        }

        /// <summary>
        /// Set the mapper which will build the value to output for the specific column
        /// </summary>
        /// <param name="ColumnToSet">column to map</param>
        /// <param name="Mapper">mapper to execute to retrieve the values to output</param>
        /// <returns>Fluent API Object</returns>
        public ExcelFluentWorksheetBuilder<TColumnEnum, TDataRowType> AddDataMapping(TColumnEnum ColumnToSet, Func<TDataRowType, object> Mapper)
        {
            AddOrUpdateColumnSetting(ColumnToSet, x => x.DataMapper = Mapper);

            return this;
        }

        #endregion

        #region Builder Methods

        /// <summary>
        /// Build the worksheet and add it to the ExcelEPPlusCreator to be saved
        /// </summary>
        /// <param name="DataSet">Dataset to build the excel sheet with</param>
        public void Build(IEnumerable<TDataRowType> DataSet)
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

            //we are going to cache the enum to column index so we don't have to keep calc in writeheaders and writebody
            var ColumnEnumCachedLookup = EnumUtility.GetValuesLazy<TColumnEnum>().ToDictionary(x => x, x => Convert.ToInt32(x));

            //get the starting data row index
            int StartToWriteBodyAtRowIndex = HeaderConfiguration.RowIndexToWriteInto + 1;

            //add the worksheet
            ExcelCreator.AddWorkSheet(WorkSheetName);

            //grab the working spreadsheet
            var WorkingSpreadSheet = ExcelCreator.WorkSheetSelect(WorkSheetName);

            //add the headers
            WriteHeadersToWorksheet(WorkingSpreadSheet, ColumnEnumCachedLookup);

            //go add the data in the spreadsheet
            WriteBodyInSpreadSheet(WorkingSpreadSheet, ColumnEnumCachedLookup, DataSet, StartToWriteBodyAtRowIndex);

            //do we want to autofit the columns
            if (HeaderConfiguration.AutoFitTheColumns)
            {
                ExcelCreator.AutoFitColumnsInASpreadSheet(WorkingSpreadSheet);
            }

            //any formatters we need to apply
            AddColumnFormatters(WorkingSpreadSheet, StartToWriteBodyAtRowIndex);

            //any widths we need to apply
            AddColumnWidth(WorkingSpreadSheet, ColumnEnumCachedLookup);
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Add or update the column configuration object
        /// </summary>
        /// <param name="ColumnToSet">Column to set</param>
        /// <param name="UpdateConfiguration">Action to update the column configuration object</param>
        private void AddOrUpdateColumnSetting(TColumnEnum ColumnToSet, Action<FluentColumnConfiguration<TColumnEnum, TDataRowType>> UpdateConfiguration)
        {
            //try to grab the value from the dictionary
            if (ColumnConfiguration.TryGetValue(ColumnToSet, out var TryToGetValue))
            {
                //go update the configuration
                UpdateConfiguration(TryToGetValue);

                //exit the method
                return;
            }

            //we don't have a configuration...go add it to the dictionary
            TryToGetValue = new FluentColumnConfiguration<TColumnEnum, TDataRowType>(ColumnToSet);

            //update the property
            UpdateConfiguration(TryToGetValue);

            //put it in the dictionary
            ColumnConfiguration.Add(ColumnToSet, TryToGetValue);
        }

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
            foreach (var ColumnToFormat in ColumnConfiguration.Where(x => x.Value.Formatter.HasValue))
            {
                //Grab the column index that we are up to
                int ColumnIndex = Convert.ToInt32(ColumnToFormat.Key);

                //format the cell
                WorkSheetToWriteTo.Cells[BodyStartRowIndex, ColumnIndex, WorkSheetToWriteTo.Dimension.End.Row, ColumnIndex].Style.Numberformat.Format = FormatterLookup[ColumnToFormat.Value.Formatter.Value];
            }
        }

        /// <summary>
        /// Set a column width
        /// </summary>
        /// <param name="WorkSheetToWriteTo">SpreadhSheet to write into</param>
        /// <param name="ColumnIndexCacheLookup">Contains a mapping of TColumnEnum and the column index so we dont need to re-calc everything for multiple methods</param>
        private void AddColumnWidth(ExcelWorksheet WorkSheetToWriteTo, IDictionary<TColumnEnum, int> ColumnIndexCacheLookup)
        {
            //loop through all the configurations (will never be null)
            foreach (var ColumnToSetWidth in ColumnConfiguration.Where(x => x.Value.ColumnWidth.HasValue))
            {
                //Grab the column index that we are up to
                int ColumnIndex = ColumnIndexCacheLookup[ColumnToSetWidth.Key];

                //format the cell
                WorkSheetToWriteTo.Column(ColumnIndex).Width = ColumnToSetWidth.Value.ColumnWidth.Value;
            }
        }

        /// <summary>
        /// Write the headers into the worksheet
        /// </summary>
        /// <param name="WorkSheet">worksheet to write into</param>
        /// <param name="ColumnIndexCacheLookup">Contains a mapping of TColumnEnum and the column index so we dont need to re-calc everything for multiple methods</param>
        private void WriteHeadersToWorksheet(ExcelWorksheet WorkSheet, IDictionary<TColumnEnum, int> ColumnIndexCacheLookup)
        {
            //loop through the values
            foreach (var EnumValue in ColumnIndexCacheLookup)
            {
                //grab the enum value
                Enum EnumValueAsEnum = EnumValue.Key as Enum;

                //make sure we have valid entries
                if (EnumValue.Value == 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(EnumValue.Value), "Column index must be greater then 0. Please check enum value = " + EnumValue + " for an invalid enum value.");
                }

                if (EnumValueAsEnum == null)
                {
                    throw new ArgumentOutOfRangeException(nameof(EnumValueAsEnum), "EnumValue is null when converted to an enum. TColumnEnum must be an enum.");
                }

                //write the header
                WorkSheet.Cells[HeaderConfiguration.RowIndexToWriteInto, EnumValue.Value].Value = EnumUtility.CustomAttributeGet<ExcelColumnHeaderAttribute>(EnumValueAsEnum).ColumnHeader;
            }

            if (HeaderConfiguration.MakeBold || HeaderConfiguration.AddAutoFilter)
            {
                var HeaderRowRange = WorkSheet.Cells[HeaderConfiguration.RowIndexToWriteInto, ColumnIndexCacheLookup.Values.Min(), HeaderConfiguration.RowIndexToWriteInto, ColumnIndexCacheLookup.Values.Max()];

                HeaderRowRange.Style.Font.Bold = HeaderConfiguration.MakeBold;
                HeaderRowRange.AutoFilter = HeaderConfiguration.AddAutoFilter;
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
        private int WriteBodyInSpreadSheet(ExcelWorksheet WorkSheetToWriteInto, IDictionary<TColumnEnum, int> ColumnIndexCacheLookup, IEnumerable<TDataRowType> DataSet, int RowIndexToStartWriting)
        {
            //the row index we write to
            int CurrentRowIndex = RowIndexToStartWriting;

            //loop through the dataset
            foreach (var RecordToWrite in DataSet)
            {
                //loop through the columns now
                foreach (var ColumnToWrite in ColumnIndexCacheLookup)
                {
                    WorkSheetToWriteInto.Cells[CurrentRowIndex, ColumnToWrite.Value].Value = ColumnConfiguration[ColumnToWrite.Key].DataMapper(RecordToWrite);
                }

                //increase the row index
                CurrentRowIndex++;
            }

            //return the current row - 1 (because we incremented after the last row)
            return CurrentRowIndex - 1;
        }

        #endregion

    }

}
