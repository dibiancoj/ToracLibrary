using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ICSAppointments;

namespace ToracLibraryTest.UnitsTest.Core
{

    /// <summary>
    /// Unit test for .ics appointment creation
    /// </summary>
    [TestClass]
    public class ICSAppointmentCreatorTest
    {

        #region Unit Test Methods

        /// <summary>
        /// Test the creation of the appointment with full dates
        /// </summary>
        [TestCategory("Core.ICS")]
        [TestCategory("Core")]
        [TestMethod]
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
            Assert.AreEqual("DTSTART;VALUE=DATE:" + StartDate.ToString(FormatOfDateTime), SplitByLine[4]);
            Assert.AreEqual("DTEND;VALUE=DATE:" + EndDate.ToString(FormatOfDateTime), SplitByLine[5]);
            Assert.AreEqual("SUMMARY:" + SummaryText, SplitByLine[6]);
            Assert.AreEqual("LOCATION:" + LocationText, SplitByLine[7]);
            Assert.AreEqual("DESCRIPTION:" + BodyOfReminder, SplitByLine[8]);
        }

        /// <summary>
        /// Test the creation of the appointment with specific date times
        /// </summary>
        [TestCategory("Core.ICS")]
        [TestCategory("Core")]
        [TestMethod]
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
            Assert.AreEqual("DTSTART:" + StartDate.ToString(FormatOfDateTime), SplitByLine[4]);
            Assert.AreEqual("DTEND:" + EndDate.ToString(FormatOfDateTime), SplitByLine[5]);
            Assert.AreEqual("SUMMARY:" + SummaryText, SplitByLine[6]);
            Assert.AreEqual("LOCATION:" + LocationText, SplitByLine[7]);
            Assert.AreEqual("DESCRIPTION:" + BodyOfReminder, SplitByLine[8]);
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
            Assert.AreEqual(11, ResultsSplitByLine.Count);

            //top of the items
            Assert.AreEqual("BEGIN:VCALENDAR", ResultsSplitByLine[0]);
            Assert.AreEqual("VERSION:2.0", ResultsSplitByLine[1]);
            Assert.AreEqual("PRODID:-//hacksw/handcal//NONSGML v1.0//EN", ResultsSplitByLine[2]);
            Assert.AreEqual("BEGIN:VEVENT", ResultsSplitByLine[3]);

            //bottom of the items
            Assert.AreEqual("END:VEVENT", ResultsSplitByLine[9]);
            Assert.AreEqual("END:VCALENDAR", ResultsSplitByLine[10]);
        }

        /// <summary>
        /// Gets the constant value from the ics class in core which is private
        /// </summary>
        /// <returns>format value to use</returns>
        private static string FormatDateFromCore()
        {
            return FormatForPrivateField("FormatDate");
        }

        /// <summary>
        /// Gets the constant value from the ics class in core which is private
        /// </summary>
        /// <returns>format value to use</returns>
        private static string FormatSpecificDateTimeFromCore()
        {
            return FormatForPrivateField("FormatSpecificDateTime");
        }

        /// <summary>
        /// Helper method to grab the field for a specified name
        /// </summary>
        /// <param name="FieldName">field name to retrieve</param>
        /// <returns>format string to use</returns>
        private static string FormatForPrivateField(string FieldName)
        {
            return typeof(ICSAppointmentCreator).GetField(FieldName, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).GetValue(null).ToString();
        }

        #endregion

    }

}
