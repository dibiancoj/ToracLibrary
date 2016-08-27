using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.Paging
{

    /// <summary>
    /// Helps page data for a data set
    /// </summary>
    public static class DataSetPaging
    {

        //** Paging of the data is in IOrderedEnumerable Extension Methods **

        /// <summary>
        /// Calculates Total Number Of Pages In This Grid (Property Of Total Above - Need Additional Data To Calculate)
        /// </summary>
        /// <param name="HowManyTotalRecordsInDataSet">Total Number Of Records (Not Just This Page But In The Entire RecordSet)</param>
        /// <param name="HowManyRecordsPerPage">How Many Records Per Page</param>
        /// <returns>Number Of Pages</returns>
        public static int CalculateTotalPages(int HowManyTotalRecordsInDataSet, int HowManyRecordsPerPage)
        {
            //calculate how many pages we have
            double Conversion = ((double)HowManyTotalRecordsInDataSet / (double)HowManyRecordsPerPage);

            //do we have an even amount
            if ((Conversion % 1) == 0)
            {
                //we have an even amount
                return Convert.ToInt32(Conversion);
            }

            //we have an uneven amount...so grab the floor then add 1
            return Convert.ToInt32(Math.Floor(Conversion)) + 1;
        }

    }

}
