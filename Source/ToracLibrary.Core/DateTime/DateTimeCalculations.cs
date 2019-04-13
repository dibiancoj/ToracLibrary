using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.DateTimeHelpers
{

    /// <summary>
    /// Calculations For Date Times
    /// </summary>
    public static class DateTimeCalculations
    {

        /// <summary>
        /// Get the number of months between 2 dates. Timespan won't give you months. You can use a timespan to get the number of days and assume its 30 days but that is not 100% accurate
        /// </summary>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns>How Many Months Between 2 Dates</returns>
        public static double HowManyMonthsBetween2Dates(DateTime StartDate, DateTime EndDate)
        {
            //Excel Formula To Validate
            //=(YEAR(EndDateCell)-YEAR(StartDateCell))*12+MONTH(EndDateCell)-MONTH(StartDateCell)

            //for the remainder 
            //=DAY(EndDateCell) (number of days in end date)
            //=DAY(DATE(YEAR(EndDateCell),MONTH(EndDateCell)+1,1)-1) (number of days in month)
            //=D19/D20 (Remainder Calculation)

            //validate that the start date is older than the end date
            if (StartDate > EndDate)
            {
                throw new ArgumentOutOfRangeException(nameof(StartDate), "Start Date Can't Be After End Date");
            }

            //get how many years between the 2
            int YearDifference = EndDate.Year - StartDate.Year;

            //get the month difference between the 2
            int MonthDifference = EndDate.Month - StartDate.Month;

            //multiple years by 12 months then add the month difference
            double WorkingFigure = (YearDifference * 12) + MonthDifference;

            //add the number of months then the remainder of days (need to convert it to a double)
            //we subtract 1 because the 1st day of the month is essentially 0 remainder
            return WorkingFigure + (((double)EndDate.Day - 1) / DateTime.DaysInMonth(EndDate.Year, EndDate.Month));
        }

        /// <summary>
        /// Calculate a persons age
        /// </summary>
        /// <param name="DateOfBirth">Person's date of birth. We will calculate age from this date</param>
        /// <returns>What is the current age of the person</returns>
        public static int CalculateAge(DateTime DateOfBirth)
        {
            // Save today's date. 
            var Today = DateTime.Today;

            // Calculate the age. 
            var Age = Today.Year - DateOfBirth.Year;

            // Go back to the year the person was born in case of a leap year 
            if (DateOfBirth > Today.AddYears(-Age))
            {
                Age--;
            }

            return Age;
        }

    }

}
