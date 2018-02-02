namespace ToracLibrary.ExcelEPPlus.Builder
{

    //*** note: EPPlus has worksheet.Cells.LoadFromCollection or worksheet.Cells.LoadFromDataReader
    //this fluent builder is really for when you need full control over the render

    /// <summary>
    /// Contains the fluent api to build a worksheet
    /// </summary>
    public class ExcelFluentBuilder
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ExcelCreatorToSet">Excel Creator to use to render the excel file</param>
        public ExcelFluentBuilder(IExcelEPPlusCreator ExcelCreatorToSet)
        {
            ExcelCreator = ExcelCreatorToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Contains the EP Plus wrapper that we use to actually add and save to the spreadsheet
        /// </summary>
        internal IExcelEPPlusCreator ExcelCreator { get; }

        #endregion

        #region Fluent API Methods

        /// <summary>
        /// Add a worksheet to the builder and returns the worksheet builder api
        /// </summary>
        /// <typeparam name="TDataRowType">Each row of the spreadsheet will be mapped to this data type</typeparam>
        /// <param name="WorkSheetNameToAdd">worksheet name to add</param>
        /// <returns>Fluent API Object</returns>
        public ExcelFluentWorkSheetBuilder<TDataRowType> AddWorkSheet<TDataRowType>(string WorkSheetNameToAdd)
        {
            return new ExcelFluentWorkSheetBuilder<TDataRowType>(this, WorkSheetNameToAdd);
        }

        #endregion

    }

}
