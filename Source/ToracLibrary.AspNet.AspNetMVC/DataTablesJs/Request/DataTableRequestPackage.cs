using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.AspNet.AspNetMVC.DataTablesJs.Request
{

    /// <summary>
    /// Contains all parameters when the client side passes in the parameters
    /// </summary>
    /// <remarks>Application will probably need to inherit to add any additional data you need</remarks>
    public class DataTableRequestPackage
    {

        //public IActionResult GridDataSelect([FromBody] DataTableRequestPackage gridParameters)

        /// <summary>
        /// Contains the parameters that are passed from the client to the server
        /// </summary>
        [JsonProperty(PropertyName = "parameters")]
        public DataTableParameters Parameters { get; set; }

    }

}
