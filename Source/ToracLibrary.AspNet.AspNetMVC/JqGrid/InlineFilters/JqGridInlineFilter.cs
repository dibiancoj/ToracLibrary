using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.AspNet.AspNetMVC.JqGrid.InlineFilters
{

    /// <summary>
    /// Holds the necessary data to search for where the user has entered in the inline filters. (for each column, this object will be filled out)
    /// </summary>
    public class JqGridInlineFilter
    {

        #region Enum

        /// <summary>
        /// Holds the type of search to run on a column
        /// </summary>
        [Flags]
        public enum JqGridOperations
        {
            //eq, // "equal"
            //ne, // "not equal"
            //lt, // "less"
            //le, // "less or equal"
            //gt, // "greater"
            //ge, // "greater or equal"
            //bw, // "begins with"
            //bn, // "does not begin with"
            ////in, // "in"
            ////ni, // "not in"
            //ew, // "ends with"
            //en, // "does not end with"
            //cn, // "contains"
            //nc  // "does not contain"

            /// <summary>
            /// Equal
            /// </summary>
            Eq = 1,

            /// <summary>
            /// Not equal
            /// </summary>
            Ne = 2,

            /// <summary>
            /// Combines equal and not equal
            /// </summary>
            EqualOrNotEqual = Eq | Ne,

            /// <summary>
            /// Less
            /// </summary>
            Lt = 4,

            /// <summary>
            /// Less or equal
            /// </summary>
            Le = 8,

            /// <summary>
            /// Greater
            /// </summary>
            Gt = 16,

            /// <summary>
            /// Greater or equal
            /// </summary>
            Ge = 32,

            /// <summary>
            /// Begins with
            /// </summary>
            Bw = 64,

            /// <summary>
            /// Does not begin with
            /// </summary>
            Bn = 128,

            /// <summary>
            /// Is in
            /// </summary>
            In = 256,

            /// <summary>
            /// Is not in
            /// </summary>
            Ni = 512,

            /// <summary>
            /// Ends with
            /// </summary>
            Ew = 1024,

            /// <summary>
            /// Does not end with
            /// </summary>
            En = 2048,

            /// <summary>
            /// Contains
            /// </summary>
            Cn = 4096,

            /// <summary>
            /// Does not contain
            /// </summary>
            Nc = 8192,

            /// <summary>
            /// Combines equal, not equal, less, less or equal, greater, greater or equal
            /// </summary>
            NoTextOperators = Eq | Ne | Lt | Le | Gt | Ge,

            /// <summary>
            /// Combines equal, not equal, begins with, does not begin with, ends with, does not end with, contains and does not contain.
            /// </summary>
            TextOperators = Eq | Ne | Bw | Bn | Ew | En | Cn | Nc

        }

        #endregion

        #region Properties

        /// <summary>
        /// Holds the column name to search for
        /// </summary>
        [JsonProperty(PropertyName = "field")]
        public string ColumnName { get; set; }

        /// <summary>
        /// Which operation should we run (configured in jqgrid - otherwise do whatever the operation you would like)
        /// </summary>
        [JsonProperty(PropertyName = "op")]
        public JqGridOperations Operation { get; set; }

        /// <summary>
        /// The value the user entered to search for
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public string UserEnteredValue { get; set; }

        #endregion

    }

}
