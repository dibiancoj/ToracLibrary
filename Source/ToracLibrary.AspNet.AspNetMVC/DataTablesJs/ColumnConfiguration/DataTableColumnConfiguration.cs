using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.AspNet.AspNetMVC.DataTablesJs.Search;

namespace ToracLibrary.AspNet.AspNetMVC.DataTablesJs.ColumnConfiguration
{

    /// <summary>
    /// Column configuration
    /// </summary>
    public class DataTableColumnConfiguration
    {

        /// <summary>
        /// Column name
        /// </summary>

        [JsonProperty(PropertyName = "name")]
        public string ColumnName { get; set; }

        /// <summary>
        /// Property name that  maps to this column
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public string PropertyNameMapping { get; set; }

        /// <summary>
        /// Can order or sort the column
        /// </summary>
        [JsonProperty(PropertyName = "orderable")]
        public bool IsOrderable { get; set; }

        /// <summary>
        /// is searchable
        /// </summary>
        [JsonProperty(PropertyName = "searchable")]
        public bool IsSearchable { get; set; }

        /// <summary>
        /// I believe used for when you add search for each column. Not the default 1 text box to search on
        /// </summary>
        [JsonProperty(PropertyName = "search")]
        public DataTableSearch Search { get; set; }

    }

}
