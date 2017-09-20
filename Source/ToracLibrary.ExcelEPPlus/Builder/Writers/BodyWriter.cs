using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.EnumUtilities;
using ToracLibrary.ExcelEPPlus.Builder.Configuration;

namespace ToracLibrary.ExcelEPPlus.Builder.Writers
{

    /// <summary>
    /// Renders the data to the excel body.
    /// </summary>
    internal static class BodyWriter
    {

        /// <summary>
        /// Add the data in the body of the spreadsheet
        /// </summary>
        /// <typeparam name="TColumnEnum">Contains the enum type where the columns are specified. This approach means each column is an enum value</typeparam>
        /// <typeparam name="TDataRowType">Each row of the spreadsheet will be mapped to this data type</typeparam>
        /// <param name="WorkSheetToWriteInto">spreadsheet to write into</param>
        /// <param name="DataSet">DataSet to write</param>
        /// <param name="RowIndexToStartWriting">Row index to starting writing at</param>
        /// <param name="ColumnConfiguration">Column configuration for this spreadsheet</param>
        /// <returns>Row index that we are done writing</returns>
        internal static int WriteBodyInSpreadSheet<TColumnEnum, TDataRowType>(ExcelWorksheet WorkSheetToWriteInto, 
                                                                              IEnumerable<TDataRowType> DataSet, 
                                                                              int RowIndexToStartWriting, 
                                                                              Dictionary<TColumnEnum, FluentColumnConfiguration<TColumnEnum, TDataRowType>> ColumnConfiguration)
               where TColumnEnum : struct
        {
            //the row index we write to
            int CurrentRowIndex = RowIndexToStartWriting;

            //loop through the dataset
            foreach (var RecordToWrite in DataSet)
            {
                //loop through the columns now
                foreach (var ColumnToWrite in EnumUtility.GetValuesLazy<TColumnEnum>())
                {
                    WorkSheetToWriteInto.Cells[CurrentRowIndex, Convert.ToInt32(ColumnToWrite)].Value = ColumnConfiguration[ColumnToWrite].DataMapper(RecordToWrite);
                }

                //increase the row index
                CurrentRowIndex++;
            }

            return CurrentRowIndex - 1;
        }

    }

}
