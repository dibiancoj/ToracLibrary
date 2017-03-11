using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.AspNet.AspNetMVC.DataTablesJs.ColumnConfiguration;
using ToracLibrary.AspNet.AspNetMVC.DataTablesJs.Search;

namespace ToracLibrary.AspNet.AspNetMVC.DataTablesJs.Request
{

    /// <summary>
    /// Contains the parameters that the client side grid passes to the server side with all parameters to render the grid for each action
    /// </summary>
    public class DataTableParameters
    {

        /// <summary>
        /// Used by data tables to keep track of ajax requests. Just set this value to whatever was passed in
        /// </summary>
        [JsonProperty(PropertyName = "draw")]
        public int Draw { get; set; }

        /// <summary>
        /// How many records to process
        /// </summary>
        [JsonProperty(PropertyName = "length")]
        public int RecordsPerPage { get; set; }

        /// <summary>
        /// The record number to render from..ie: Skip()
        /// </summary>
        [JsonProperty(PropertyName = "start")]
        public int StartRecordNumber { get; set; }

        /// <summary>
        /// Did the user search for anything
        /// </summary>
        [JsonProperty(PropertyName = "search")]
        public DataTableSearch Search { get; set; }

        /// <summary>
        /// Column configuration
        /// </summary>
        [JsonProperty(PropertyName = "columns")]
        public IList<DataTableColumnConfiguration> ColumnConfiguration { get; set; }

        /// <summary>
        /// Which way is the grid sorted
        /// </summary>
        [JsonProperty(PropertyName = "order")]
        public IList<DataTableColumnOrder> SortOrder { get; set; }

    }

}
