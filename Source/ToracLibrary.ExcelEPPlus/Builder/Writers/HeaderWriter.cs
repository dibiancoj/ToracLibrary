using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.EnumUtilities;
using ToracLibrary.ExcelEPPlus.Builder.Attributes;

namespace ToracLibrary.ExcelEPPlus.Builder.Writers
{

    /// <summary>
    /// Class that builds the header for a defined configuration in EPPlus - Excel
    /// </summary>
    internal static class HeaderWriter
    {

        /// <summary>
        /// Write the headers into the worksheet
        /// </summary>
        /// <typeparam name="TColumnEnum">Enum with the specified columns</typeparam>
        /// <param name="WorkSheet">worksheet to write into</param>
        /// <param name="RowIndexToWriteTo">Row Index to write on</param>
        /// <param name="MakeBold">Make the header text bold</param>
        /// <param name="AddAutoFilter">Add auto filter in the header</param>
        internal static void WriteHeadersToWorksheet<TColumnEnum>(ExcelWorksheet WorkSheet, int RowIndexToWriteTo, bool MakeBold, bool AddAutoFilter)
             where TColumnEnum : struct
        {
            if (RowIndexToWriteTo == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(RowIndexToWriteTo), "Row index must be greater then 0");
            }

            //grab all the enum values in the enum
            var ValuesInEnum = EnumUtility.GetValuesLazy<TColumnEnum>().ToArray();

            //loop through the values
            foreach (var EnumValue in ValuesInEnum)
            {
                //grab the enum column index
                int EnumColumnIndex = Convert.ToInt32(EnumValue);

                //grab the enum value
                Enum EnumValueAsEnum = EnumValue as Enum;

                //make sure we have valid entries
                if (EnumColumnIndex == 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(EnumColumnIndex), "Column index must be greater then 0. Please check enum value = " + EnumValue + " for an invalid enum value.");
                }

                if (EnumValueAsEnum == null)
                {
                    throw new ArgumentOutOfRangeException(nameof(EnumValueAsEnum), "EnumValue is null when converted to an enum. TColumnEnum must be an enum.");
                }

                //grab the header text
                var ExcelHeaderAttribute = EnumUtility.CustomAttributeGet<ExcelColumnHeaderAttribute>(EnumValueAsEnum);

                //write the header
                WorkSheet.Cells[RowIndexToWriteTo, EnumColumnIndex].Value = ExcelHeaderAttribute.ColumnHeader;
            }

            if (MakeBold || AddAutoFilter)
            {
                //grab the enum values to an int so we don't have to do this twice
                var EnumValuesAsInt = ValuesInEnum.Select(x => Convert.ToInt32(x)).ToArray();

                var HeaderRowRange = WorkSheet.Cells[RowIndexToWriteTo, EnumValuesAsInt.Min(), RowIndexToWriteTo, EnumValuesAsInt.Max()];

                if (MakeBold)
                {
                    HeaderRowRange.Style.Font.Bold = true;
                }

                if (AddAutoFilter)
                {
                    HeaderRowRange.AutoFilter = true;
                }
            }
        }

    }

}
