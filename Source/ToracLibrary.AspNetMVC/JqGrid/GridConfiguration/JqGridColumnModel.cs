using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.AspNet.AspNetMVC.JqGrid.GridConfiguration
{

    /// <summary>
    /// Holds a configuration for a column in a JqGrid Configuration
    /// </summary>
    public class JqGridColumnModel
    {

        /// <summary>
        /// Set the unique name in the grid for the column. This property is required
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Set the index name when sorting. Passed as sidx parameter.
        /// </summary>
        [JsonProperty("index")]
        public string Index { get; set; }

        /// <summary>
        /// Should this column be hidden?
        /// </summary>
        [JsonProperty("hidden")]
        public bool? Hidden { get; set; }

        /// <summary>
        /// If set to true this column will not appear in the modal dialog where users can choose which columns to show or hide.
        /// </summary>
        [JsonProperty("hidedlg")]
        public bool? HideDialog { get; set; }

        /// <summary>
        /// Width of the column
        /// </summary>
        [JsonProperty("width")]
        public int? Width { get; set; }

        /// <summary>
        /// Can this column be sorted?
        /// </summary>
        [JsonProperty("sortable")]
        public bool? Sortable { get; set; }

        /// <summary>
        /// Is this column searchable?
        /// </summary>
        [JsonProperty("search")]
        public bool? Search { get; set; }

        /// <summary>
        /// The predefined types (string) or custom function name that controls the format of this field
        /// </summary>
        [JsonProperty("formatter")]
        public string Formatter { get; set; }

        /// <summary>
        /// Format options can be defined for particular columns, overwriting the defaults from the language file
        /// </summary>
        [JsonProperty("formatoptions")]
        public JqGridColumnFormatOptions FormatOptions { get; set; }

    }

}
