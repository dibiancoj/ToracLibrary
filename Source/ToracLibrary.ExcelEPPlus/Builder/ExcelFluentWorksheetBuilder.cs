using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.EnumUtilities;
using ToracLibrary.ExcelEPPlus.Builder.Attributes;
using ToracLibrary.ExcelEPPlus.Builder.Configuration;
using ToracLibrary.ExcelEPPlus.Builder.Writers;
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

            if (!ColumnConfiguration.Any())
            {
                throw new ArgumentOutOfRangeException(nameof(ColumnConfiguration), "No Column Configuration Have Been Generated. Please Call AddDataMapping For Each Column");
            }

            if (DataSet == null)
            {
                throw new NullReferenceException(nameof(DataSet));
            }

            //get the starting data row index
            int StartToWriteBodyAtRowIndex = BodyStartRowIndex();

            //add the worksheet
            ExcelCreator.AddWorkSheet(WorkSheetName);

            //grab the working spreadsheet
            var WorkingSpreadSheet = ExcelCreator.WorkSheetSelect(WorkSheetName);

            //add the headers
            HeaderWriter.WriteHeadersToWorksheet<TColumnEnum>(WorkingSpreadSheet, HeaderConfiguration.RowIndexToWriteInto, HeaderConfiguration.MakeBold, HeaderConfiguration.AddAutoFilter);

            //go add the data in the spreadsheet
            BodyWriter.WriteBodyInSpreadSheet(WorkingSpreadSheet, DataSet, StartToWriteBodyAtRowIndex, ColumnConfiguration);

            //do we want to autofit the columns
            if (HeaderConfiguration.AutoFitTheColumns)
            {
                ExcelCreator.AutoFitColumnsInASpreadSheet(WorkingSpreadSheet);
            }

            //any formatters we need to apply
            AddColumnFormatters(WorkingSpreadSheet, StartToWriteBodyAtRowIndex);

            //any widths we need to apply
            AddColumnWidth(WorkingSpreadSheet);
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
        /// what is the row index that the body starts at
        /// </summary>
        /// <returns>row index</returns>
        private int BodyStartRowIndex()
        {
            return HeaderConfiguration.RowIndexToWriteInto + 1;
        }

        /// <summary>
        /// Add a column formatter to a worksheet
        /// </summary>
        /// <param name="WorkSheetToWriteTo">SpreadhSheet to write into</param>
        /// <param name="BodyStartRowIndex">The row index where the body starts to write into</param>
        private void AddColumnFormatters(ExcelWorksheet WorkSheetToWriteTo, int BodyStartRowIndex)
        {
            //loop through all the configurations (will never be null)
            foreach (var ColumnToFormat in ColumnConfiguration.Where(x => x.Value.Formatter.HasValue))
            {
                //Grab the column index that we are up to
                int ColumnIndex = Convert.ToInt32(ColumnToFormat.Key);

                //format the cell
                WorkSheetToWriteTo.Cells[BodyStartRowIndex, ColumnIndex, WorkSheetToWriteTo.Dimension.End.Row, ColumnIndex].Style.Numberformat.Format = EnumUtility.CustomAttributeGet<ExcelFormatterAttribute>(ColumnToFormat.Value.Formatter.Value).FormatterToUse;
            }
        }

        /// <summary>
        /// Set a column width
        /// </summary>
        /// <param name="WorkSheetToWriteTo">SpreadhSheet to write into</param>
        private void AddColumnWidth(ExcelWorksheet WorkSheetToWriteTo)
        {
            //loop through all the configurations (will never be null)
            foreach (var ColumnToSetWidth in ColumnConfiguration.Where(x => x.Value.ColumnWidth.HasValue))
            {
                //Grab the column index that we are up to
                int ColumnIndex = Convert.ToInt32(ColumnToSetWidth.Key);

                //format the cell
                WorkSheetToWriteTo.Column(ColumnIndex).Width = ColumnToSetWidth.Value.ColumnWidth.Value;
            }
        }

        #endregion

    }

}
