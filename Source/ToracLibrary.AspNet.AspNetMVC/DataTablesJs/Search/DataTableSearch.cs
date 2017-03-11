using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.AspNet.AspNetMVC.DataTablesJs.Search
{

    /// <summary>
    /// Any search text filled out
    /// </summary>
    public class DataTableSearch
    {

        /// <summary>
        /// Search text
        /// </summary>
        [JsonProperty(PropertyName = "value")]
        public string SearchText { get; set; }

        /// <summary>
        /// Not sure what this is for. Lookup in databases js documentation if we need it
        /// </summary>
        [JsonProperty(PropertyName = "regex")]
        public bool RegEx { get; set; }

    }

}
