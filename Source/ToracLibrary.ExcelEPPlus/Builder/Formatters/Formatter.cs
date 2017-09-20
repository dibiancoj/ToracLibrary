using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.ExcelEPPlus.Builder.Attributes;

namespace ToracLibrary.ExcelEPPlus.Builder.Formatters
{

    /// <summary>
    /// Contains different formatters for excel
    /// </summary>
    public static class Formatter
    {

        /// <summary>
        /// Excel date formats
        /// </summary>
        public enum ExcelBuilderFormatters
        {
            [ExcelFormatter("mm-dd-yyyy")]
            Date = 0,

            [ExcelFormatter("mm-dd-yyyy hh:mm AM/PM")]
            DateAndTime = 1
        }

    }

}
