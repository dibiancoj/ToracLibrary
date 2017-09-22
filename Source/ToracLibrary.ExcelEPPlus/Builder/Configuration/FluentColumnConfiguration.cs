using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ToracLibrary.ExcelEPPlus.Builder.Formatters.Formatter;

namespace ToracLibrary.ExcelEPPlus.Builder.Configuration
{

    /// <summary>
    /// Holds the configuration for a specific column
    /// </summary>
    /// <typeparam name="TColumnEnum">Column Enum Type</typeparam>
    /// <typeparam name="TDataRowType">Holds the row data type for each row outputted in excel</typeparam>
    internal class FluentColumnConfiguration<TDataRowType>
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ColumnIndexToSet">Index of the column this is going to. First column is 1</param>
        /// <param name="HeaderDisplayTextToSet">Header text to display</param>
        /// <param name="DataMapperToSet">Mapper which takes the row object and returns the column value</param>
        /// <param name="FormatterToSet">Any additional formatter settings for this column. ie: date column</param>
        /// <param name="ColumnWidthToSet">A specific column width. This will overwrite auto width column setting for the individual column</param>
        public FluentColumnConfiguration(int ColumnIndexToSet, string HeaderDisplayTextToSet, Func<TDataRowType, object> DataMapperToSet, ExcelBuilderFormatters? FormatterToSet, double? ColumnWidthToSet)
        {
            //ColumnKey = ColumnKeyToSet;
            ColumnIndex = ColumnIndexToSet;
            HeaderDisplayText = HeaderDisplayTextToSet;
            DataMapper = DataMapperToSet;
            Formatter = FormatterToSet;
            ColumnWidth = ColumnWidthToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Index of the column this is going to. First column is 1
        /// </summary>
        internal int ColumnIndex { get; }

        /// <summary>
        /// Header text to display
        /// </summary>
        internal string HeaderDisplayText { get; }

        /// <summary>
        /// Mapper which takes the row object and returns the column value
        /// </summary>
        internal Func<TDataRowType, object> DataMapper { get; }

        /// <summary>
        /// Any additional formatter settings for this column. ie: date column
        /// </summary>
        internal ExcelBuilderFormatters? Formatter { get; }

        /// <summary>
        /// A specific column width. This will overwrite auto width column setting for the individual column
        /// </summary>
        internal double? ColumnWidth { get; }

        #endregion

    }

}
