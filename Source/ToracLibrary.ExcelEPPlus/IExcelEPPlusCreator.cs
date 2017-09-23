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
        /// Write to a cell. Mainly used for unit test moqing
        /// </summary>
        /// <param name="SpreadSheetToWriteInto">Spreadsheet to write into</param>
        /// <param name="ColumnIndex">Column index</param>
        /// <param name="RowIndex">Row Index</param>
        /// <param name="ValueToWrite">Value To Write</param>
        void WriteToCell(ExcelWorksheet SpreadSheetToWriteInto, int ColumnIndex, int RowIndex, object ValueToWrite);

        /// <summary>
        /// Make an excel range bold
        /// </summary>
        /// <param name="RangeToMakeBold">Range to make bold</param>
        void MakeRangeBold(ExcelRange RangeToMakeBold);

        /// <summary>
        /// Add an auto filter to an excel range
        /// </summary>
        /// <param name="RangeToAddFilterIn">Range to add the auto filter in</param>
        void AddAutoFilter(ExcelRange RangeToAddFilterIn);

        /// <summary>
        /// Save the workbook and returns the byte array.
        /// </summary>
        /// <returns>Byte Array</returns>
        /// <remarks>See ToracTechnologies.Library.IO To Save A File From Byte Array</remarks>
        byte[] SaveWorkBook();

    }

}
