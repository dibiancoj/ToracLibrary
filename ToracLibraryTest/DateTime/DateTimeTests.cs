using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToracLibrary.DateTimeHelpers;
using System.Collections.Generic;
using System.Linq;
using ToracLibrary.DateTimeHelpers.BusinessHours;

namespace ToracLibraryTest.UnitsTest
{

    /// <summary>
    /// Unit test to test date specific functionality
    /// </summary>
    [TestClass]
    public class DateTimeTests
    {

        #region Months Between 2 Dates

        /// <summary>
        /// Test how many months between 2 dates
        /// </summary>
        [TestMethod]
        public void HowManyMonthsBetween2DatesTest1()
        {
            Assert.AreEqual(1.129032258064516, DateTimeCalculations.HowManyMonthsBetween2Dates(new DateTime(2012, 12, 1), new DateTime(2013, 1, 5)));
            Assert.AreEqual(1, DateTimeCalculations.HowManyMonthsBetween2Dates(new DateTime(2012, 12, 1), new DateTime(2013, 1, 1)));
            Assert.AreEqual(12, DateTimeCalculations.HowManyMonthsBetween2Dates(new DateTime(2012, 12, 1), new DateTime(2013, 12, 1)));
        }

        #endregion

        #region Business Hours

        #region Constants

        /// <summary>
        /// Start hour of the work day
        /// </summary>
        private const int StartWorkDayHour = 8;

        /// <summary>
        /// End hour of the work day
        /// </summary>
        private const int EndWorkDayHour = 17;

        /// <summary>
        /// How long is a normal work day
        /// </summary>
        private const int HowLongIsWorkDayInHour = 9;

        #endregion

        /// <summary>
        /// Builds holidays
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<BusinessHourHoliday> BuildHolidaysLazy()
        {
            //just return 1 holiday
            yield return new BusinessHourHoliday(new DateTime(2014, 7, 4), new DateTime(2014, 7, 5));
        }

        /// <summary>
        /// Test method for business hours, going forwards
        /// </summary>
        [TestMethod]
        public void BusinessHoursPositiveNumbersTest1()
        {
            //grab the holidays to test with
            var HolidaysToTestWith = BuildHolidaysLazy().ToArray();

            //test 1 hour
            Assert.AreEqual(1, BusinessHoursCalculator.BusinessHoursBetweenDates(new DateTime(2014, 1, 6, 8, 0, 0), new DateTime(2014, 1, 6, 9, 0, 0), StartWorkDayHour, EndWorkDayHour));

            //test 1 hour with 30: after destination
            Assert.AreEqual(1.5, BusinessHoursCalculator.BusinessHoursBetweenDates(new DateTime(2014, 1, 6, 8, 0, 0), new DateTime(2014, 1, 6, 9, 30, 0), StartWorkDayHour, EndWorkDayHour));

            //test 1 full day
            Assert.AreEqual(HowLongIsWorkDayInHour, BusinessHoursCalculator.BusinessHoursBetweenDates(new DateTime(2014, 1, 6, 8, 0, 0), new DateTime(2014, 1, 6, 17, 0, 0), StartWorkDayHour, EndWorkDayHour));

            //test 1 full day after business hours have ended
            Assert.AreEqual(HowLongIsWorkDayInHour, BusinessHoursCalculator.BusinessHoursBetweenDates(new DateTime(2014, 1, 6, 8, 0, 0), new DateTime(2014, 1, 6, 20, 0, 0), StartWorkDayHour, EndWorkDayHour));

            //test 2 full day after business hours have ended
            Assert.AreEqual(HowLongIsWorkDayInHour * 2, BusinessHoursCalculator.BusinessHoursBetweenDates(new DateTime(2014, 1, 6, 8, 0, 0), new DateTime(2014, 1, 7, 20, 0, 0), StartWorkDayHour, EndWorkDayHour));

            //test weekends
            Assert.AreEqual(HowLongIsWorkDayInHour, BusinessHoursCalculator.BusinessHoursBetweenDates(new DateTime(2014, 1, 10, 8, 0, 0), new DateTime(2014, 1, 11, 20, 0, 0), StartWorkDayHour, EndWorkDayHour));

            //test weekends (sat)
            Assert.AreEqual(HowLongIsWorkDayInHour, BusinessHoursCalculator.BusinessHoursBetweenDates(new DateTime(2014, 1, 10, 8, 0, 0), new DateTime(2014, 1, 12, 20, 0, 0), StartWorkDayHour, EndWorkDayHour));

            //test weekends (sun)
            Assert.AreEqual(HowLongIsWorkDayInHour * 2, BusinessHoursCalculator.BusinessHoursBetweenDates(new DateTime(2014, 1, 10, 8, 0, 0), new DateTime(2014, 1, 13, 20, 0, 0), StartWorkDayHour, EndWorkDayHour));

            //test holidays (no days)
            Assert.AreEqual(0, BusinessHoursCalculator.BusinessHoursBetweenDates(new DateTime(2014, 7, 4, 8, 0, 0), new DateTime(2014, 7, 5, 8, 0, 0), StartWorkDayHour, EndWorkDayHour, HolidaysToTestWith));

            //test holidays (1 day)
            Assert.AreEqual(HowLongIsWorkDayInHour, BusinessHoursCalculator.BusinessHoursBetweenDates(new DateTime(2014, 7, 3, 8, 0, 0), new DateTime(2014, 7, 5, 8, 0, 0), StartWorkDayHour, EndWorkDayHour, HolidaysToTestWith));

            //test holidays (after hour on last day day)
            Assert.AreEqual(0, BusinessHoursCalculator.BusinessHoursBetweenDates(new DateTime(2014, 7, 3, EndWorkDayHour + 1, 0, 0), new DateTime(2014, 7, 5, 8, 0, 0), StartWorkDayHour, EndWorkDayHour, HolidaysToTestWith));

            //test holidays (holiday on friday with weekend)
            Assert.AreEqual(HowLongIsWorkDayInHour - 1, BusinessHoursCalculator.BusinessHoursBetweenDates(new DateTime(2014, 7, 3, 9, 0, 0), new DateTime(2014, 7, 6, 8, 0, 0), StartWorkDayHour, EndWorkDayHour, HolidaysToTestWith));

            //test holidays (holiday on friday with weekend and then next week)
            Assert.AreEqual(HowLongIsWorkDayInHour * 2, BusinessHoursCalculator.BusinessHoursBetweenDates(new DateTime(2014, 7, 3, StartWorkDayHour, 0, 0), new DateTime(2014, 7, 8, StartWorkDayHour, 0, 0), StartWorkDayHour, EndWorkDayHour, HolidaysToTestWith));

        }

        /// <summary>
        /// Test the business hours, going backwards
        /// </summary>
        [TestMethod]
        public void BusinessHoursNegativeNumbersTest1()
        {
            //grab the holidays to test with
            var HolidaysToTestWith = BuildHolidaysLazy().ToArray();

            //test 1 hour
            Assert.AreEqual(1 * -1, BusinessHoursCalculator.BusinessHoursBetweenDates(new DateTime(2014, 1, 6, 9, 0, 0), new DateTime(2014, 1, 6, 8, 0, 0), StartWorkDayHour, EndWorkDayHour));

            //test 1 hour with 30: after destination
            Assert.AreEqual(1.5 * -1, BusinessHoursCalculator.BusinessHoursBetweenDates(new DateTime(2014, 1, 6, 9, 30, 0), new DateTime(2014, 1, 6, 8, 0, 0), StartWorkDayHour, EndWorkDayHour));

            //test 1 full day
            Assert.AreEqual(HowLongIsWorkDayInHour * -1, BusinessHoursCalculator.BusinessHoursBetweenDates(new DateTime(2014, 1, 6, 17, 0, 0), new DateTime(2014, 1, 6, 8, 0, 0), StartWorkDayHour, EndWorkDayHour));

            //test 1 full day after business hours have ended
            Assert.AreEqual(HowLongIsWorkDayInHour * -1, BusinessHoursCalculator.BusinessHoursBetweenDates(new DateTime(2014, 1, 6, 20, 0, 0), new DateTime(2014, 1, 6, 8, 0, 0), StartWorkDayHour, EndWorkDayHour));

            //test 2 full day after business hours have ended
            Assert.AreEqual((HowLongIsWorkDayInHour * 2) * -1, BusinessHoursCalculator.BusinessHoursBetweenDates(new DateTime(2014, 1, 7, 20, 0, 0), new DateTime(2014, 1, 6, 8, 0, 0), StartWorkDayHour, EndWorkDayHour));

            //test weekends
            Assert.AreEqual(HowLongIsWorkDayInHour * -1, BusinessHoursCalculator.BusinessHoursBetweenDates(new DateTime(2014, 1, 11, 20, 0, 0), new DateTime(2014, 1, 10, 8, 0, 0), StartWorkDayHour, EndWorkDayHour));

            //test weekends (sat)
            Assert.AreEqual(HowLongIsWorkDayInHour * -1, BusinessHoursCalculator.BusinessHoursBetweenDates(new DateTime(2014, 1, 12, 20, 0, 0), new DateTime(2014, 1, 10, 8, 0, 0), StartWorkDayHour, EndWorkDayHour));

            //test weekends (sun)
            Assert.AreEqual((HowLongIsWorkDayInHour * 2) * -1, BusinessHoursCalculator.BusinessHoursBetweenDates(new DateTime(2014, 1, 13, 20, 0, 0), new DateTime(2014, 1, 10, 8, 0, 0), StartWorkDayHour, EndWorkDayHour));

            //test holidays (no days)
            Assert.AreEqual(0 * -1, BusinessHoursCalculator.BusinessHoursBetweenDates(new DateTime(2014, 7, 5, 8, 0, 0), new DateTime(2014, 7, 4, 8, 0, 0), StartWorkDayHour, EndWorkDayHour, HolidaysToTestWith));

            //test holidays (1 day)
            Assert.AreEqual(HowLongIsWorkDayInHour * -1, BusinessHoursCalculator.BusinessHoursBetweenDates(new DateTime(2014, 7, 5, 8, 0, 0), new DateTime(2014, 7, 3, 8, 0, 0), StartWorkDayHour, EndWorkDayHour, HolidaysToTestWith));

            //test holidays (after hour on last day day)
            Assert.AreEqual(0 * -1, BusinessHoursCalculator.BusinessHoursBetweenDates(new DateTime(2014, 7, 5, 8, 0, 0), new DateTime(2014, 7, 3, EndWorkDayHour + 1, 0, 0), StartWorkDayHour, EndWorkDayHour, HolidaysToTestWith));

            //test holidays (holiday on friday with weekend)
            Assert.AreEqual((HowLongIsWorkDayInHour - 1) * -1, BusinessHoursCalculator.BusinessHoursBetweenDates(new DateTime(2014, 7, 6, 8, 0, 0), new DateTime(2014, 7, 3, 9, 0, 0), StartWorkDayHour, EndWorkDayHour, HolidaysToTestWith));

            //test holidays (holiday on friday with weekend and then next week)
            Assert.AreEqual((HowLongIsWorkDayInHour * 2) * -1, BusinessHoursCalculator.BusinessHoursBetweenDates(new DateTime(2014, 7, 8, StartWorkDayHour, 0, 0), new DateTime(2014, 7, 3, StartWorkDayHour, 0, 0), StartWorkDayHour, EndWorkDayHour, HolidaysToTestWith));
        }

        #endregion

        #region Quarter Time Period

        /// <summary>
        /// Test which quarter this date is in
        /// </summary>
        [TestMethod]
        public void QuarterTimePeriodTest1()
        {
            Assert.AreEqual(1, QuarterTimePeriod.QuarterIsInTimePeriod(new DateTime(2014, 1, 1)));
            Assert.AreEqual(1, QuarterTimePeriod.QuarterIsInTimePeriod(new DateTime(2014, 2, 1)));
            Assert.AreEqual(1, QuarterTimePeriod.QuarterIsInTimePeriod(new DateTime(2014, 3, 1)));

            Assert.AreEqual(2, QuarterTimePeriod.QuarterIsInTimePeriod(new DateTime(2014, 4, 1)));
            Assert.AreEqual(2, QuarterTimePeriod.QuarterIsInTimePeriod(new DateTime(2014, 5, 1)));
            Assert.AreEqual(2, QuarterTimePeriod.QuarterIsInTimePeriod(new DateTime(2014, 6, 1)));

            Assert.AreEqual(3, QuarterTimePeriod.QuarterIsInTimePeriod(new DateTime(2014, 7, 1)));
            Assert.AreEqual(3, QuarterTimePeriod.QuarterIsInTimePeriod(new DateTime(2014, 8, 1)));
            Assert.AreEqual(3, QuarterTimePeriod.QuarterIsInTimePeriod(new DateTime(2014, 9, 1)));

            Assert.AreEqual(4, QuarterTimePeriod.QuarterIsInTimePeriod(new DateTime(2014, 10, 1)));
            Assert.AreEqual(4, QuarterTimePeriod.QuarterIsInTimePeriod(new DateTime(2014, 11, 1)));
            Assert.AreEqual(4, QuarterTimePeriod.QuarterIsInTimePeriod(new DateTime(2014, 12, 1)));
        }

        #endregion

    }

}