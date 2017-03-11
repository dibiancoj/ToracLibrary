using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.AspNet.AspNetMVC.DataTablesJs.Response
{

    /// <summary>
    /// Contains the return object for service side processing
    /// </summary>
    /// <typeparam name="T">Type of each row</typeparam>
    public class DataTableResultSet<T> where T : class
    {

        /// <summary>
        /// Each of the rows in the grid
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public IList<T> DataSet { get; set; }

        /// <summary>
        /// Used by data tables to keep track of ajax requests. Just set this value to whatever was passed in
        /// </summary>
        [JsonProperty(PropertyName = "draw")]
        public int DrawNumber { get; set; }

        /// <summary>
        /// Filtered Record Count
        /// </summary>
        [JsonProperty(PropertyName = "recordsFiltered")]
        public int FilteredRecordCount { get; set; }

        /// <summary>
        /// Total record count in DataSet
        /// </summary>
        [JsonProperty(PropertyName = "recordsTotal")]
        public int DataSetRecordCount { get; set; }

    }

}
