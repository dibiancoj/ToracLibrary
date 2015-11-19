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

        /*Syntax should be something like this
            *BEGIN:VCALENDAR
            *VERSION:2.0
            *PRODID:-//hacksw/handcal//NONSGML v1.0//EN
            *BEGIN:VEVENT
            *DTSTART: UTC Time  ==> if this is a full day use DTSTART;VALUE=DATE:20150930
            *DTEND: (Same as DTStart)
            *SUMMARY: bla bla
            *LOCATION: New York
            *END:VEVENT
            *END:VCALENDAR
            */

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

            //go run it and grab the results
            var ICSCreatedFile = ICSAppointmentCreator.CreateICSAppointment(true, StartDate, EndDate, SummaryText, LocationText);

            //split the lines
            var SplitByLine = ICSCreatedFile.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            //check the standard fields
            ICSStandardFormatTest(SplitByLine);

            //check the values that actually change
            Assert.AreEqual("DTSTART;VALUE=DATE:" + StartDate.ToString(ICSAppointmentCreator.FormatDate), SplitByLine[4]);
            Assert.AreEqual("DTEND;VALUE=DATE:" + EndDate.ToString(ICSAppointmentCreator.FormatDate), SplitByLine[5]);
            Assert.AreEqual("SUMMARY:" + SummaryText, SplitByLine[6]);
            Assert.AreEqual("LOCATION:" + LocationText, SplitByLine[7]); 
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

            //go run it and grab the results
            var ICSCreatedFile = ICSAppointmentCreator.CreateICSAppointment(false, StartDate, EndDate, SummaryText, LocationText);

            //split the lines
            var SplitByLine = ICSCreatedFile.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            //check the standard fields
            ICSStandardFormatTest(SplitByLine);

            //check the values now
            Assert.AreEqual("DTSTART:" + StartDate.ToString(ICSAppointmentCreator.FormatSpecificDateTime), SplitByLine[4]);
            Assert.AreEqual("DTEND:" + EndDate.ToString(ICSAppointmentCreator.FormatSpecificDateTime), SplitByLine[5]);
            Assert.AreEqual("SUMMARY:" + SummaryText, SplitByLine[6]);
            Assert.AreEqual("LOCATION:" + LocationText, SplitByLine[7]);
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
            //make sure we have 10 items
            Assert.AreEqual(10, ResultsSplitByLine.Count);

            //top of the items
            Assert.AreEqual("BEGIN:VCALENDAR", ResultsSplitByLine[0]);
            Assert.AreEqual("VERSION:2.0", ResultsSplitByLine[1]);
            Assert.AreEqual("PRODID:-//hacksw/handcal//NONSGML v1.0//EN", ResultsSplitByLine[2]);
            Assert.AreEqual("BEGIN:VEVENT", ResultsSplitByLine[3]);

            //bottom of the items
            Assert.AreEqual("END:VEVENT", ResultsSplitByLine[8]);
            Assert.AreEqual("END:VCALENDAR", ResultsSplitByLine[9]);
        }

        #endregion

    }

}
