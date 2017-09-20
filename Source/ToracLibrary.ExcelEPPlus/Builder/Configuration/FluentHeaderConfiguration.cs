using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.ExcelEPPlus.Builder.Configuration
{

    /// <summary>
    /// Contains the internal object that holds the header information
    /// </summary>
    internal class FluentHeaderConfiguration
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="RowIndexToWriteIntoToSet">Row index to write the header into</param>
        /// <param name="MakeBoldToSet">make the headers bold</param>
        /// <param name="AddAutoFilterToSet">add the auto filter to the headers</param>
        /// <param name="AutoFitColumnsToSet">auto fit the columns</param>
        public FluentHeaderConfiguration(int RowIndexToWriteIntoToSet, bool MakeBoldToSet, bool AddAutoFilterToSet, bool AutoFitColumnsToSet)
        {
            RowIndexToWriteInto = RowIndexToWriteIntoToSet;
            MakeBold = MakeBoldToSet;
            AddAutoFilter = AddAutoFilterToSet;
            AutoFitTheColumns = AutoFitColumnsToSet;
        }

        /// <summary>
        /// Row index to write the header into
        /// </summary>
        internal int RowIndexToWriteInto { get; set; }

        /// <summary>
        /// make the headers bold
        /// </summary>
        internal bool MakeBold { get; set; }

        /// <summary>
        /// add the auto filter to the headers
        /// </summary>
        internal bool AddAutoFilter { get; set; }

        /// <summary>
        /// auto fit the columns
        /// </summary>
        internal bool AutoFitTheColumns { get; set; }

    }

}
