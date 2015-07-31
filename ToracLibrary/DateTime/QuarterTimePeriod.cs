using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.DateTimeHelpers
{

    /// <summary>
    /// Calculate which quarter a time period falls in.
    /// </summary>
    public static class QuarterTimePeriod
    {

        /// <summary>
        /// Figure out which quarter this time period falls in
        /// </summary>
        /// <param name="WhichQuarterIsDateTimeIn">Date time to figure out which quarter this falls in</param>
        /// <returns>Which Quarter 1 through 4</returns>
        public static int QuarterIsInTimePeriod(DateTime WhichQuarterIsDateTimeIn)
        {
            //determine which quarter by the month
            switch (WhichQuarterIsDateTimeIn.Month)
            {
                case 1:
                case 2:
                case 3:
                    return 1;

                case 4:
                case 5:
                case 6:
                    return 2;

                case 7:
                case 8:
                case 9:
                    return 3;

                case 10:
                case 11:
                case 12:
                    return 4;

                default:
                    throw new NotImplementedException();
            }
        }

    }

}
