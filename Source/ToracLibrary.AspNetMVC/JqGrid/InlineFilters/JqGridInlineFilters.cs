using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Serialization.Json;

namespace ToracLibrary.AspNet.AspNetMVC.JqGrid.InlineFilters
{

    /// <summary>
    /// Holds the JqGrid Inline Filter Data Model
    /// </summary>
    public class JqGridInlineFilters
    {

        #region Enums

        /// <summary>
        /// Are we running And's or Or's
        /// </summary>
        public enum JqGridOperationType
        {

            /// <summary>
            /// And
            /// </summary>
            AND,

            /// <summary>
            /// Or
            /// </summary>
            OR
        }

        #endregion

        #region Properties

        /// <summary>
        /// Holds the type to run which was configured in jqGrid (And's or Or's)
        /// </summary>
        [JsonProperty(PropertyName = "groups")]
        public JqGridOperationType Operation { get; set; }

        /// <summary>
        /// Holds the filters that the user entered for each columns.
        /// </summary>
        [JsonProperty(PropertyName = "rules")]
        public IEnumerable<JqGridInlineFilter> Filters { get; set; }

        #endregion

        #region Static Methods

        /// <summary>
        /// If the string is filled out, will convert it to a grid filter
        /// </summary>
        /// <param name="JsonStringToConvert">JsonString passed in from JqGrid</param>
        /// <returns>GridFilters or null if the JsonStringToConvert string is empty</returns>
        public static JqGridInlineFilters Deserialize(string JsonStringToConvert)
        {
            //if the string is null or empty just return null
            if (string.IsNullOrEmpty(JsonStringToConvert))
            {
                return null;
            }

            //go deserialize it and return it
            return JsonNetSerializer.Deserialize<JqGridInlineFilters>(JsonStringToConvert);
        }

        #endregion

    }
}
