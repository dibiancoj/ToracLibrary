using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.Delimiter
{

    /// <summary>
    /// Holds a row of data for the delimiter namespace. Shared between the creator and parser
    /// </summary>
    /// <remarks>Class Is Immutable</remarks>
    public class DelimiterRow
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ColumnDataForThisRow">Column Data For This Row</param>
        public DelimiterRow(IEnumerable<string> ColumnDataForThisRow)
        {
            //set the property
            ColumnData = ColumnDataForThisRow;
        }

        #endregion

        #region Column Data

        /// <summary>
        /// Holds the column data split out for this specific row. Each record in the list is a column
        /// </summary>
        /// <remarks>Is immutable</remarks>
        public IEnumerable<string> ColumnData { get; }

        #endregion

    }

}
