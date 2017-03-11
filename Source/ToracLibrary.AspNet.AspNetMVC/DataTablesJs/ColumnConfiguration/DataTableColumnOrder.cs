using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.AspNet.AspNetMVC.DataTablesJs.ColumnConfiguration
{

    /// <summary>
    /// How to sort the grid
    /// </summary>
    public class DataTableColumnOrder
    {

        #region Enum

        /// <summary>
        /// Sort order 
        /// </summary>
        public enum DataTableSortOrder
        {

            /// <summary>
            /// Ascending
            /// </summary>
            asc,

            /// <summary>
            /// Descending
            /// </summary>
            desc

        }

        #endregion

        #region Properties

        /// <summary>
        /// Column index that is contained in this record
        /// </summary>
        [JsonProperty(PropertyName = "column")]
        public int ColumnIndex { get; set; }

        /// <summary>
        /// Direction which this column is sorted
        /// </summary>
        [JsonProperty(PropertyName = "dir")]
        public DataTableSortOrder SortOrder { get; set; }

        #endregion

    }

}
