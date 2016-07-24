using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.Excel;
using Xunit;

namespace ToracLibrary.UnitTest.Core
{
    
    /// <summary>
    /// Unit test for excel helper methods
    /// </summary>
    public class ExcelUtilityTest
    {

        #region Column Name To Int

        /// <summary>
        /// Test column name to number (for excel)
        /// </summary>
        [Fact]
        public void ExcelToolsColumnNameToIntTest1()
        {
            Assert.Equal(1, ExcelUtilities.ColumnLetterToColumnIndex("A"));
            Assert.Equal(27, ExcelUtilities.ColumnLetterToColumnIndex("AA"));
            Assert.Equal(4, ExcelUtilities.ColumnLetterToColumnIndex("D"));

        }

        #endregion

        #region Column Number To Letter

        /// <summary>
        /// Test column number to letter (for excel)
        /// </summary>
        [Fact]
        public void ExcelToolsColumnNumberToLetterTest1()
        {
            Assert.Equal("AA", ExcelUtilities.ColumnIndexToColumnLetter(27));
            Assert.Equal("A", ExcelUtilities.ColumnIndexToColumnLetter(1));
            Assert.Equal("D", ExcelUtilities.ColumnIndexToColumnLetter(4));
        }

        #endregion

    }

}
