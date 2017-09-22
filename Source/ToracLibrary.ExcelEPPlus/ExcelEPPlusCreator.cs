using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.ExcelEPPlus
{

    //*****Notes 
    //make sure you reference ep plus from the source project when calling this library.
    //import OfficeOpenXml. Then you will see the references to the worksheet when you cal createworksheet

    //Example of a fast spreadsheet
    //var j = new ExcelCreator();
    //var ws = j.AddWorkSheet("jason");
    //var rng = ws.Cells["A1:A10"];
    //rng[1,1].Value = "Jason";

    //Some examples:
    //ws.Cells["A1"].Value = "Sample 2";
    //ws.Cells["A1:C2"].Value = "Sample 2";
    //ws.Cells["A1"].Style.Font.Bold = true;
    //ws.Cells[row, 1, row, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
    //ws.Cells[row, 1, row, 5].Style.Fill.BackgroundColor.SetColor(Color.LightGoldenrodYellow);
    //ws.Name = sheetName; //Setting Sheet's name
    //ws.Cells.Style.Font.Size = 11; //Default font size for whole sheet
    //ws.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet
    //ws.Cells[1, 1, 1, dt.Columns.Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Aligmnet is center
    //cell.Formula = "Sum(" + ws.Cells[3, colIndex].Address + ":" + ws.Cells[rowIndex - 1, colIndex].Address + ")";
    //var border = cell.Style.Border;
    //border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

    //how to use conditional formatting
    //1. How to make the cell font red if the font value is < 0 (fixed value, no formula)
    //var range = worksheet.Cells[startOfData, thisDecimalColumn, lastRow, thisDecimalColumn];
    //range.Style.Numberformat.Format = AppConstants.ExcelAccountingFormat;
    //var condExpression = worksheet.ConditionalFormatting.AddLessThan(new ExcelAddress(range.Address)); *exceladdress takes a string so "A1"...range is a cell range variable i had
    //condExpression.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
    //condExpression.Style.Font.Color.Color = Color.Red;
    //less than 0
    //condExpression.Formula = "0";

    //2. if we want to make it red based on a formula
    //thisCell = ws.Cells[thisTotalColumn + rowIndex];
    //thisCell.Formula = string.Format("=Sum({0}2:{0}{1})", thisTotalColumn, lastRowOfDataRowIndex);
    //thisCell.Style.Numberformat.Format = AppConstants.ExcelAccountingFormat;
    //var _statement = "IF(" + string.Format("Sum({0}2:{0}{1})", thisTotalColumn, lastRowOfDataRowIndex) + "<0,1,0)";
    //var _cond3 = ws.ConditionalFormatting.AddExpression(new ExcelAddress(thisCell.Address));
    //_cond3.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
    //_cond3.Style.Font.Color.Color = Color.Red;
    //_cond3.Formula = _statement;

    //3. for performance always write cells from a1, a2, a3...b1, b2, b3...so build up a dictionary then 
    //do a sort and write otherwise epplus has to reorder the cells to the get them in the write sort order.
    //for big worksheets this gets really slow...so  cells.OrderBy(x => x.RowIndex).ThenBy(x => x.ColumnIndex).ToArray()

    //style as date with no time
    //thisCell.Style.Numberformat.Format = "mm-dd-yyyy";
    //style with date and time
    //thisCell.Style.Numberformat.Format = "mm-dd-yyyy hh:mm AM/PM";

    /// <summary>
    /// Creates Excel Files Using EPPlus
    /// </summary>
    /// <remarks>Is Server safe and you don't need Excel installed. Uses the Open XML SDK. Class is immutable</remarks>
    public class ExcelEPPlusCreator : IExcelEPPlusCreator, IDisposable
    {

        #region Constructor

        /// <summary>
        /// Constructor If You Are Starting From Scratch
        /// </summary>
        public ExcelEPPlusCreator()
        {
            //create a new xlpackage
            ExcelCreatorPackage = new ExcelPackage();
        }

        /// <summary>
        /// Constructor To Load A File (If you want to use a template)
        /// </summary>
        /// <param name="FilePath">File Path To Load</param>
        public ExcelEPPlusCreator(string FilePath)
        {
            //make sure we can find the template
            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException("Can't Find Template To Load. Location: " + FilePath);
            }

            //create a new xlpackage
            ExcelCreatorPackage = new ExcelPackage(new FileInfo(FilePath));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Holds the object which creates everything. Sort of Excel.exe
        /// </summary>
        public ExcelPackage ExcelCreatorPackage { get; }

        /// <summary>
        /// Holds a flag if the class has been disposed yet or called to be disposed yet
        /// </summary>
        /// <remarks>Used IDisposable</remarks>
        private bool disposed { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a work sheet to the work book and return
        /// </summary>
        /// <param name="WorkSheetName">Work sheet name</param>
        /// <returns>ExcelWorkbook</returns>
        public ExcelWorksheet AddWorkSheet(string WorkSheetName)
        {
            //create the work sheet and return it
            return ExcelCreatorPackage.Workbook.Worksheets.Add(WorkSheetName);
        }

        /// <summary>
        /// Select A WorkSheet That Is Already In The Workbook
        /// </summary>
        /// <param name="WorkSheetName">Worksheet Name</param>
        /// <returns>Excel Worksheet</returns>
        public ExcelWorksheet WorkSheetSelect(string WorkSheetName)
        {
            //go try to find the worksheet
            return ExcelCreatorPackage.Workbook.Worksheets[WorkSheetName];
        }

        /// <summary>
        /// Auto fit all the columns in all worksheets
        /// </summary>
        public void AutoFitColumns()
        {
            foreach (var WorkSheetToFormat in ExcelCreatorPackage.Workbook.Worksheets)
            {
                AutoFitColumnsInASpreadSheet(WorkSheetToFormat);
            }
        }

        /// <summary>
        /// Auto fit the columns in a spreadsheet
        /// </summary>
        /// <param name="SpreadSheetToAutoFit">Spreadsheet to auto fit all the columns</param>
        public void AutoFitColumnsInASpreadSheet(ExcelWorksheet SpreadSheetToAutoFit)
        {
            SpreadSheetToAutoFit.Cells[SpreadSheetToAutoFit.Dimension.Start.Row, SpreadSheetToAutoFit.Dimension.Start.Column, SpreadSheetToAutoFit.Dimension.End.Row, SpreadSheetToAutoFit.Dimension.End.Column].AutoFitColumns();
        }

        /// <summary>
        /// Write to a cell. Mainly used for unit test moqing
        /// </summary>
        /// <param name="SpreadSheetToWriteInto">Spreadsheet to write into</param>
        /// <param name="ColumnIndex">Column index</param>
        /// <param name="RowIndex">Row Index</param>
        /// <param name="ValueToWrite">Value To Write</param>
        public void WriteToCell(ExcelWorksheet SpreadSheetToWriteInto, int ColumnIndex, int RowIndex, object ValueToWrite)
        {
            SpreadSheetToWriteInto.Cells[RowIndex, ColumnIndex].Value = ValueToWrite;
        }

        /// <summary>
        /// Save the workbook and returns the byte array.
        /// </summary>
        /// <returns>Byte Array</returns>
        /// <remarks>See ToracTechnologies.Library.IO To Save A File From Byte Array</remarks>
        public byte[] SaveWorkBook()
        {
            //See ToracTechnologies.Library.IO To Save A File From Byte Array

            //go grab the package and return the byte array
            return ExcelCreatorPackage.GetAsByteArray();
        }

        #endregion

        #region Dispose Method

        /// <summary>
        /// Disposes My Object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose Overload. Ensures my database connection is closed
        /// </summary>
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    //dispose of the xl package
                    ExcelCreatorPackage.Dispose();
                }
            }
            disposed = true;
        }

        #endregion
    }

}
