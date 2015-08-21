using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.AspNetMVC.JqGrid.GridConfiguration
{

    /// <summary>
    /// Holds the grid configuration for a JqGrid
    /// </summary>
    public class JqGridConfiguration
    {

        #region Properties

        /// <summary>
        /// Holds just the column names that will be outputted as the column headers
        /// </summary>
        [JsonProperty("colNames")]
        public IEnumerable<string> ColumnNames { get; set; }

        /// <summary>
        /// Holds the column configuration which contains all the information jqGrid needs to output the column
        /// </summary>
        [JsonProperty("colModel")]
        public IEnumerable<JqGridColumnModel> ColumnModels { get; set; }

        #endregion

    }

}
