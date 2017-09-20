using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.ExcelEPPlus.Builder.Attributes
{

    /// <summary>
    /// Contains a string for Excel to output the data type in a specified format.
    /// </summary>
    internal class ExcelFormatterAttribute : Attribute
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="FormatterToSet">Format to use for this configuration</param>
        public ExcelFormatterAttribute(string FormatterToSet)
        {
            FormatterToUse = FormatterToSet;
        }

        /// <summary>
        /// Column output to use when outputting the header
        /// </summary>
        public string FormatterToUse { get; set; }

    }

}
