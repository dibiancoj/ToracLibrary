using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.ExcelEPPlus
{

    /// <summary>
    /// Excel EP Plus Creator Interface For Testing
    /// </summary>
    public interface IExcelEPPlusCreator : IDisposable
    {

        /// <summary>
        /// Holds the object which creates everything. Sort of Excel.exe
        /// </summary>
        ExcelPackage ExcelCreatorPackage { get; }

        /// <summary>
        /// Adds a work sheet to the work book and return
        /// </summary>
        /// <param name="WorkSheetName">Work sheet name</param>
        /// <returns>ExcelWorkbook</returns>
        ExcelWorksheet AddWorkSheet(string WorkSheetName);

        /// <summary>
        /// Select A WorkSheet That Is Already In The Workbook
        /// </summary>
        /// <param name="WorkSheetName">Worksheet Name</param>
        /// <returns>Excel Worksheet</returns>
        ExcelWorksheet WorkSheetSelect(string WorkSheetName);

        /// <summary>
        /// Auto fit all the columns in all worksheets
        /// </summary>
        void AutoFitColumns();

        /// <summary>
        /// Auto fit the columns in a spreadsheet
        /// </summary>
        /// <param name="SpreadSheetToAutoFit">Spreadsheet to auto fit all the columns</param>
        void AutoFitColumnsInASpreadSheet(ExcelWorksheet SpreadSheetToAutoFit);

        /// <summary>
        /// Save the workbook and returns the byte array.
        /// </summary>
        /// <returns>Byte Array</returns>
        /// <remarks>See ToracTechnologies.Library.IO To Save A File From Byte Array</remarks>
        byte[] SaveWorkBook();

    }

}
