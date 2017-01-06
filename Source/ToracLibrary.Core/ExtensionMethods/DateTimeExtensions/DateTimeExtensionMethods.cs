using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.ExtensionMethods.DateTimeExtensions
{

    /// <summary>
    /// Extension methods for date time variables
    /// </summary>
    public static class DateTimeExtensionMethods
    {

        /// <summary>
        /// Evalulate if the date time is between the start and end date. Essentially ValueToEvaluate >= BeginningStartDate && ValueToEvaluate < EndStartDate
        /// </summary>
        /// <param name="ValueToEvaluate">Value to determine if its between the 2 time periods specified in BeginningStartDate and EndStartDate</param>
        /// <param name="BeginningStartDate">Start date range</param>
        /// <param name="EndStartDate">End date to range</param>
        /// <returns>True if the ValueToEvaluate is between the specified date range</returns>
        public static bool IsBetween(this DateTime ValueToEvaluate, DateTime BeginningStartDate, DateTime EndStartDate)
        {
            //make sure the start is before the end
            if (EndStartDate < BeginningStartDate)
            {
                throw new ArgumentOutOfRangeException(nameof(BeginningStartDate), "Start Date Is After End Date");
            }

            //is the value between the 2 dates passed in
            return ValueToEvaluate >= BeginningStartDate && ValueToEvaluate < EndStartDate;
        }

    }

}
