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
    internal class FluentColumnConfiguration<TColumnEnum, TDataRowType>
           where TColumnEnum : struct
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ColumnValueToSet">Column we are configuring</param>
        public FluentColumnConfiguration(TColumnEnum ColumnValueToSet)
        {
            ColumnValue = ColumnValueToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Column that we are configuring
        /// </summary>
        internal TColumnEnum ColumnValue { get; set; }

        /// <summary>
        /// Format to output the columnw ith
        /// </summary>
        internal ExcelBuilderFormatters? Formatter { get; set; }

        /// <summary>
        /// Set a column to a specific width
        /// </summary>
        internal double? ColumnWidth { get; set; }

        /// <summary>
        /// Holds the data mapper that retrieves the value to output in excel
        /// </summary>
        internal Func<TDataRowType, object> DataMapper { get; set; }

        #endregion

    }

}
