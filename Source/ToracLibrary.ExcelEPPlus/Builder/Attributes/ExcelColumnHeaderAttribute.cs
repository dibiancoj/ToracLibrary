using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.ExcelEPPlus.Builder.Attributes
{

    /// <summary>
    /// Column the column header information when building the headers in EP plus. This takes the enum approach where each column is an enum value with an int
    /// </summary>
    public class ExcelColumnHeaderAttribute : Attribute
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ColumnHeaderToSet">Column output to use when outputting the header</param>
        public ExcelColumnHeaderAttribute(string ColumnHeaderToSet)
        {
            ColumnHeader = ColumnHeaderToSet;
        }

        /// <summary>
        /// Column output to use when outputting the header
        /// </summary>
        public string ColumnHeader { get; set; }

    }

}
