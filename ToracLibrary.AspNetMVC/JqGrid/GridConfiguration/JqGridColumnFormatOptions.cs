using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.AspNetMVC.JqGrid.GridConfiguration
{

    /// <summary>
    /// Holds custom format options for a specific column configuration
    /// </summary>
    public class JqGridColumnFormatOptions
    {

        #region Properties

        /// <summary>
        /// source format
        /// </summary>
        [JsonProperty("srcformat")]
        public string SourceFormat { get; set; }

        /// <summary>
        /// new format
        /// </summary>
        [JsonProperty("newformat")]
        public string NewFormat { get; set; }

        #endregion

        #region Methods

        //This works but it seems it too customized, leaving it in here for an example
        ///// <summary>
        ///// Builds a JqGridColumnFormatOptions for a date time column
        ///// </summary>
        ///// <returns></returns>
        //public static JqGridColumnFormatOptions DateTimeFormatter()
        //{
        //    //go build the object and return it
        //    return new JqGridColumnFormatOptions
        //    {
        //        SourceFormat = "Y-m-d H:i:s",
        //        NewFormat = "m-d-Y g:i:s A"
        //    };
        //}

        #endregion

    }

}
