using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.Excel;

namespace ToracLibraryTest.UnitsTest.Core
{
    
    /// <summary>
    /// Unit test for excel helper methods
    /// </summary>
    [TestClass]
    public class ExcelUtilityTest
    {

        #region Column Name To Int

        /// <summary>
        /// Test column name to number (for excel)
        /// </summary>
        [TestMethod]
        public void ExcelToolsColumnNameToIntTest1()
        {
            Assert.AreEqual(1, ExcelUtilities.ColumnLetterToColumnIndex("A"));
            Assert.AreEqual(27, ExcelUtilities.ColumnLetterToColumnIndex("AA"));
            Assert.AreEqual(4, ExcelUtilities.ColumnLetterToColumnIndex("D"));

        }

        #endregion

        #region Column Number To Letter

        /// <summary>
        /// Test column number to letter (for excel)
        /// </summary>
        [TestMethod]
        public void ExcelToolsColumnNumberToLetterTest1()
        {
            Assert.AreEqual("AA", ExcelUtilities.ColumnIndexToColumnLetter(27));
            Assert.AreEqual("A", ExcelUtilities.ColumnIndexToColumnLetter(1));
            Assert.AreEqual("D", ExcelUtilities.ColumnIndexToColumnLetter(4));
        }

        #endregion

    }

}
