using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.IEnumerableExtensions;

namespace ToracLibrary.Core.DateTimeHelpers.BusinessHours
{

    /// <summary>
    /// Calculates Business Hours Between 2 Dates
    /// </summary>
    public static class BusinessHoursCalculator
    {

        #region Public Methods

        /// <summary>
        /// Calculates The Number Of Business Hours Between 2 Dates.
        /// </summary>
        /// <param name="StartDate">Start Date To Calculate From</param>
        /// <param name="EndDate">End Date To Calculate To</param>
        /// <param name="BusinessDayStartHour">Hour Of When The Business Day Starts (24 Hour Clock - ie. 1pm is 13)</param>
        /// <param name="BusinessDayEndHour">Hour Of When The Business Day Ends (24 Hour Clock - ie. 1pm is 13)</param>
        /// <returns>Number Of Business Hours. Supports Going Backwards</returns>
        public static double BusinessHoursBetweenDates(DateTime StartDate, DateTime EndDate, int BusinessDayStartHour, int BusinessDayEndHour)
        {
            //use the helper method
            return BusinessHoursBetweenDatesHelper(StartDate, EndDate, BusinessDayStartHour, BusinessDayEndHour, null);
        }

        /// <summary>
        /// Calculates The Number Of Business Hours Between 2 Dates When You Want To Account For Holidays
        /// </summary>
        /// <param name="StartDate">Start Date To Calculate From</param>
        /// <param name="EndDate">End Date To Calculate To</param>
        /// <param name="BusinessDayStartHour">Hour Of When The Business Day Starts (24 Hour Clock - ie. 1pm is 13)</param>
        /// <param name="BusinessDayEndHour">Hour Of When The Business Day Ends (24 Hour Clock - ie. 1pm is 13)</param>
        /// <param name="HolidayListing">Holiday Listing. This Works With Only Dates. Don't Specify July 4th at 4pm. There Is A Holiday Length For Half Days</param>
        /// <returns>Number Of Business Hours. Supports Going Backwards</returns>
        public static double BusinessHoursBetweenDates(DateTime StartDate, DateTime EndDate, int BusinessDayStartHour, int BusinessDayEndHour, IEnumerable<BusinessHourHoliday> HolidayListing)
        {
            //use the helper method
            return BusinessHoursBetweenDatesHelper(StartDate, EndDate, BusinessDayStartHour, BusinessDayEndHour, HolidayListing);
        }

        #endregion

        #region Private Static Helper Methods

        /// <summary>
        /// Calculates The Number Of Business Hours Between 2 Dates
        /// </summary>
        /// <param name="StartDate">Start Date To Calculate From</param>
        /// <param name="EndDate">End Date To Calculate To</param>
        /// <param name="BusinessDayStartHour">Hour Of When The Business Day Starts (24 Hour Clock - ie. 1pm is 13)</param>
        /// <param name="BusinessDayEndHour">Hour Of When The Business Day Ends (24 Hour Clock - ie. 1pm is 13)</param>
        /// <param name="HolidayListing">Holiday Listing. This Works With Only Dates. Don't Specify July 4th at 4pm. There Is A Holiday Length For Half Days</param>
        /// <returns>Number Of Business Hours. Supports Going Backwards</returns>
        private static double BusinessHoursBetweenDatesHelper(DateTime StartDate, DateTime EndDate, int BusinessDayStartHour, int BusinessDayEndHour, IEnumerable<BusinessHourHoliday> HolidayListing)
        {
            //make sure the start hour and end hour are between and 1 and 24
            if (!HourPassesValidation24HourValue(BusinessDayStartHour))
            {
                //start hour is not between 1 and 24
                throw new ArgumentOutOfRangeException("BusinessDayStartHour", "Start Hour Must Be Between 0-24 (Hour Value In A Day)");
            }

            //make sure the start hour and end hour are between and 1 and 24
            if (!HourPassesValidation24HourValue(BusinessDayEndHour))
            {
                //end hour is not between 1 and 24
                throw new ArgumentOutOfRangeException("BusinessDayEndHour", "End Hour Must Be Between 0-24 (Hour Value In A Day)");
            }

            //make sure the end hour is not earlier then the start hour
            if (BusinessDayEndHour < BusinessDayStartHour)
            {
                //throw an out of range argument exception because the end of the day needs to be after the start of the day
                throw new ArgumentOutOfRangeException("BusinessDayEndHour Can't Be Before BusinessDayStartHour");
            }

            //are we going backwards? (start date is after end date)
            bool AreWeGoingBackwards = false;

            //holds the start date so we can always go forwards
            DateTime StartDateConverted = StartDate;

            //holds the end date so we can always go forwards
            DateTime EndDateConverted = EndDate;

            //holds the trim down version of the holiday list for this period (using an array to speed it up - instead of IEnumerable)
            BusinessHourHoliday[] HolidayListingForThisPeriod = null;

            //let's check if we need to go backwards
            if (StartDateConverted > EndDateConverted)
            {
                //we need to go backwards flip the flag and the dates
                AreWeGoingBackwards = true;

                //now flip the dates
                StartDateConverted = EndDate;
                EndDateConverted = StartDate;
            }

            //do we have any holidays? if so, grab just the holidays for this period
            if (HolidayListing.AnyWithNullCheck())
            {
                //go grab the holidays for this period
                HolidayListingForThisPeriod = HolidayListing.Where(x => x.StartDateOfHoliday.Date >= StartDateConverted.Date &&
                                                                        x.EndDateOfHoliday.Date <= EndDateConverted.Date).ToArray();
            }

            //holds the working date which we will keep incrementing
            DateTime WorkingDate = StartDateConverted;

            //holds the hour count that we will return
            double WorkBusinessHourCount = 0;

            //loop until we get the end of the time
            while (WorkingDate < EndDateConverted)
            {
                //is it the weekend
                if (WorkingDate.DayOfWeek == DayOfWeek.Saturday || WorkingDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    //its the weekend...just fast foward to the Monday
                    while (WorkingDate.DayOfWeek == DayOfWeek.Saturday || WorkingDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        //fast foward to the next day
                        WorkingDate = GetNextDayAtStartOfBusinessDay(WorkingDate, BusinessDayStartHour);
                    }
                }

                //is it a holiday
                else if (HolidayListingForThisPeriod.AnyWithNullCheck(x => WorkingDate >= x.StartDateOfHoliday && WorkingDate < x.EndDateOfHoliday))
                {
                    //it's a working holiday...so don't increment it and increase the working date
                    WorkingDate = WorkingDate.AddHours(1);
                }

                //is it a work hour (between 8 and 7 pm)
                else if (WorkingDate.Hour >= BusinessDayStartHour && WorkingDate.Hour < BusinessDayEndHour)
                {
                    //are we less then an hour from the end
                    double WorkingDateFromEnd = EndDateConverted.Subtract(WorkingDate).TotalHours;

                    //is it a full hour?
                    if (WorkingDateFromEnd >= 1)
                    {
                        //we are a full hour
                        WorkBusinessHourCount += 1;
                    }
                    else
                    {
                        //we are less then an hour so increment the different
                        WorkBusinessHourCount += WorkingDateFromEnd;
                    }

                    //add an hour to the working date
                    WorkingDate = WorkingDate.AddHours(1);
                }
                else
                {
                    //we are at a non business hour...let's fast foward to the next day at 8am
                    WorkingDate = GetNextDayAtStartOfBusinessDay(WorkingDate, BusinessDayStartHour);
                }
            }

            //if we are going to backwards then we want to make it a negative number
            if (AreWeGoingBackwards)
            {
                //we need to flip the sign because we are going backwards
                WorkBusinessHourCount *= -1;
            }

            //now return the count
            return WorkBusinessHourCount;
        }

        /// <summary>
        /// Fast Forward The Date To The Next Day At The Start Of The Business Day
        /// </summary>
        /// <param name="DateToGetNextDay">Date To Get The Next Business Day</param>
        /// <param name="StartBusinessHour">Start Business Hour</param>
        /// <returns>Will Return The Next Day Of The Date Passed In At The Start Business Hour Time</returns>
        private static DateTime GetNextDayAtStartOfBusinessDay(DateTime DateToGetNextDay, int StartBusinessHour)
        {
            //add a day to the next day (we will set 8 am of this date below)
            DateTime NextDay = DateToGetNextDay.AddDays(1);

            //we are at a non business hour...let's fast foward to the next day at 8am
            return new DateTime(NextDay.Year, NextDay.Month, NextDay.Day, StartBusinessHour, 0, 0);
        }

        /// <summary>
        /// Method validates that the hour passed in is between 0-24 (hours in a day)
        /// </summary>
        /// <param name="HourToCheck">Hour to check</param>
        /// <returns>If it passes validation</returns>
        private static bool HourPassesValidation24HourValue(int HourToCheck)
        {
            //is it between 0 and 24 hour.
            return HourToCheck >= 0 && HourToCheck < 24;
        }

        #endregion

    }

}
