using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ICSAppointments;
using ToracLibrary.UnitTest.Framework;
using Xunit;

namespace ToracLibrary.UnitTest.Core
{

    /// <summary>
    /// Unit test for .ics appointment creation
    /// </summary>
    public class ICSAppointmentCreatorTest
    {

        #region Unit Test Methods

        /// <summary>
        /// Test the creation of the appointment with full dates
        /// </summary>
        [Fact]
        public void ICSCreationWithFullDatesTest1()
        {
            //start date
            DateTime StartDate = new DateTime(2015, 3, 5);

            //end date
            DateTime EndDate = new DateTime(2015, 3, 10);

            //summary text
            const string SummaryText = "Test Summary";

            //location text
            const string LocationText = "Location Text";

            //body
            const string BodyOfReminder = "BodyOfReminder";

            //go run it and grab the results
            var ICSCreatedFile = ICSAppointmentCreator.CreateICSAppointment(true, StartDate, EndDate, SummaryText, LocationText, BodyOfReminder);

            //split the lines
            var SplitByLine = ICSCreatedFile.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            //check the standard fields
            ICSStandardFormatTest(SplitByLine);

            //grab the format we need
            var FormatOfDateTime = FormatDateFromCore();

            //check the values that actually change
            Assert.Equal("DTSTART;VALUE=DATE:" + StartDate.ToString(FormatOfDateTime), SplitByLine[4]);
            Assert.Equal("DTEND;VALUE=DATE:" + EndDate.ToString(FormatOfDateTime), SplitByLine[5]);
            Assert.Equal("SUMMARY:" + SummaryText, SplitByLine[6]);
            Assert.Equal("LOCATION:" + LocationText, SplitByLine[7]);
            Assert.Equal("DESCRIPTION:" + BodyOfReminder, SplitByLine[8]);
        }

        /// <summary>
        /// Test the creation of the appointment with specific date times
        /// </summary>
        [Fact]
        public void ICSCreationWithSpecificDateTimesTest1()
        {
            //start date
            DateTime StartDate = new DateTime(2015, 3, 5, 3, 55, 0);

            //end date
            DateTime EndDate = new DateTime(2015, 3, 10, 4, 58, 0);

            //summary text
            const string SummaryText = "Test Summary 123";

            //location text
            const string LocationText = "Location Text 123";

            //body
            const string BodyOfReminder = "BodyOfReminder 123";

            //go run it and grab the results
            var ICSCreatedFile = ICSAppointmentCreator.CreateICSAppointment(false, StartDate, EndDate, SummaryText, LocationText, BodyOfReminder);

            //split the lines
            var SplitByLine = ICSCreatedFile.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            //check the standard fields
            ICSStandardFormatTest(SplitByLine);

            //grab the format we need
            var FormatOfDateTime = FormatSpecificDateTimeFromCore();

            //check the values now
            Assert.Equal("DTSTART:" + StartDate.ToString(FormatOfDateTime), SplitByLine[4]);
            Assert.Equal("DTEND:" + EndDate.ToString(FormatOfDateTime), SplitByLine[5]);
            Assert.Equal("SUMMARY:" + SummaryText, SplitByLine[6]);
            Assert.Equal("LOCATION:" + LocationText, SplitByLine[7]);
            Assert.Equal("DESCRIPTION:" + BodyOfReminder, SplitByLine[8]);
        }

        #endregion

        #region Helper Unit Test Methods

        /// <summary>
        /// Will test the standard fields (excludes the date, summary, and location fields)
        /// </summary>
        /// <param name="ResultsSplitByLine">The results split by new line</param>
        /// <remarks>Will assert and raise whatever it needs too</remarks>
        private void ICSStandardFormatTest(IList<string> ResultsSplitByLine)
        {
            //make sure we have 11 items
            Assert.Equal(11, ResultsSplitByLine.Count);

            //top of the items
            Assert.Equal("BEGIN:VCALENDAR", ResultsSplitByLine[0]);
            Assert.Equal("VERSION:2.0", ResultsSplitByLine[1]);
            Assert.Equal("PRODID:-//hacksw/handcal//NONSGML v1.0//EN", ResultsSplitByLine[2]);
            Assert.Equal("BEGIN:VEVENT", ResultsSplitByLine[3]);

            //bottom of the items
            Assert.Equal("END:VEVENT", ResultsSplitByLine[9]);
            Assert.Equal("END:VCALENDAR", ResultsSplitByLine[10]);
        }

        /// <summary>
        /// Gets the constant value from the ics class in core which is private
        /// </summary>
        /// <returns>format value to use</returns>
        private static string FormatDateFromCore()
        {
            return FrameworkHelperMethods.GetPrivateFieldValue(typeof(ICSAppointmentCreator), "FormatDate");
        }

        /// <summary>
        /// Gets the constant value from the ics class in core which is private
        /// </summary>
        /// <returns>format value to use</returns>
        private static string FormatSpecificDateTimeFromCore()
        {
            return FrameworkHelperMethods.GetPrivateFieldValue(typeof(ICSAppointmentCreator), "FormatSpecificDateTime");
        }

        #endregion

    }

}
